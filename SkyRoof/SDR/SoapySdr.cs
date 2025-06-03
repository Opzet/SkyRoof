﻿using System.Reflection;
using System.Runtime.InteropServices;
using Serilog;

namespace VE3NEA
{
  public static class SoapySdr
  {
    static SoapySdr()
    {
      SetSoapySdrPluginFolder();
    }

    // Force SoapySDR to look for the SDR drivers in a sub-folder of the app folder
    public static void SetSoapySdrPluginFolder()
    {
      // app directory to env. var.
      string? appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (appDir == null) throw new Exception("Cannot get assembly location");
      Environment.SetEnvironmentVariable("SOAPY_SDR_ROOT", appDir);

      // path to plugins folder
      var abiVersionPtr = NativeSoapySdr.SoapySDR_getABIVersion();
      string ABIVersion = Marshal.PtrToStringAnsi(abiVersionPtr)!;
      string pluginsPath = $"{appDir}\\lib\\SoapySDR\\modules{ABIVersion}";

      // add plugins folder to PATH env. var.
      string originalPath = Environment.GetEnvironmentVariable("PATH") ?? "";
      Environment.SetEnvironmentVariable("PATH", $"{pluginsPath};{originalPath}");
      Log.Information($"Setting SoapySDR plugin path: {pluginsPath}");
      if (!Directory.Exists(pluginsPath)) Log.Error("Plugins path does not exist");
    }

    public static SoapySdrDeviceInfo[] EnumerateDevices(string args = "")
    {
      IntPtr ptr = NativeSoapySdr.SoapySDRDevice_enumerateStrArgs(args, out IntPtr length);
      CheckError();

      if (ptr == IntPtr.Zero) return Array.Empty<SoapySdrDeviceInfo>();
      var kwargs = SoapySdrHelper.MarshalKwArgsArray(ptr, length);
      var result = kwargs.Select(args => new SoapySdrDeviceInfo(args)).ToArray();
      foreach (var dev in result) dev.Present = true;
      return result;
    }

    public static IntPtr CreateDevice(SoapySDRKwargs kwArgs)
    {
      IntPtr nativeKwargs = kwArgs.ToNative();
      var device = NativeSoapySdr.SoapySDRDevice_make(nativeKwargs);
      Marshal.FreeHGlobal(nativeKwargs);
      CheckError();
      return device;
    }

    public static void ReleaseDevice(IntPtr device)
    {
      if (device != IntPtr.Zero) NativeSoapySdr.SoapySDRDevice_unmake(device);
    }

    public static void CheckError()
    {
      int errorCode = NativeSoapySdr.SoapySDRDevice_lastStatus();
      if (errorCode != 0)
      {
        IntPtr ptr = NativeSoapySdr.SoapySDRDevice_lastError();
        string errorMessage = Marshal.PtrToStringAnsi(ptr) ?? $"Unknown error";
        throw new Exception($"SoapySDR error: {errorMessage}");
      }
    }

    internal static bool DeviceExists(SoapySDRKwargs kwArgs)
    {
      IntPtr nativeKwargs = kwArgs.ToNative();
      try
      {
        IntPtr ptr = NativeSoapySdr.SoapySDRDevice_enumerate(nativeKwargs, out nint length);
        CheckError();

        return ptr != IntPtr.Zero && length > 0;
      }
      finally
      {
        Marshal.FreeHGlobal(nativeKwargs);
      }
    }
  }
}