# Code Signing Policy

Free code signing is [*going to be*] provided by 
[SignPath.io](https://signpath.io/), certificate by
[SignPath Foundation](https://signpath.org/).

## Automatic downloads

The setup program automatically downloads and installs the following dependencies, provided by Microsoft,
if they are not present:

- [.NET 9.0 Desktop](https://dotnet.microsoft.com/en-us/download/dotnet/9.0);
- [MS VC++ Redistributable](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170).

Automatic downloads are implemented using
[InnoDependencyInstaller](https://github.com/DomGries/InnoDependencyInstaller)

## Third party libraries

### NuGet Packages

This project uses a number of NuGet packages, all packages are listed in the [SkyRoof.csproj](./SkyRoof/SkyRoof.csproj) file.

### DLL files

A number of third party dll files, all open source, are included with the setup program in the binary format.
These files are located in the [Vendor](./Vendor/) folder, and also listed in the [SkyRoof.csproj](SkyRoof/SkyRoof.csproj) file.
See the [Creadits](./docs/credits.md) page for the sources of the dll files.
