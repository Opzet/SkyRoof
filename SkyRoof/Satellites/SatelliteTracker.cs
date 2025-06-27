﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using static SkyRoof.SatellitePass;

namespace SkyRoof
{
  public class SatelliteTracker
  {
    private Satellite? Satellite;
    public Tle? Tle;
    public bool Enabled { get; private set; }

    public SatelliteTracker(SatnogsDbTle? tle)
    {
      if (tle == null) return;

      Tle = CreateTle(tle);
      if (Tle == null) return;

      Satellite = new(Tle);
      Enabled = true;
    }

    public Tle? CreateTle(SatnogsDbTle tle)
    {
      try
      {
        return new Tle(tle.tle0, tle.tle1, tle.tle2);
      }
      catch (Exception ex)
      {
        Log.Error(ex, $"Error creating TLE object: |{tle.tle0}|{tle.tle1}|{tle.tle2}|.");
        return null;
      }
    }

    public GeodeticCoordinate? Predict(DateTime? utc = null)
    {
      if (!Enabled) return null;

      try
      {
        return Satellite!.Predict(utc ?? DateTime.UtcNow).ToGeodetic();
      }
      catch (Exception ex)
      {
        // preduction for current time failed, disable further tracking
        Enabled = false;
        Log.Error(ex, $"Predict failed for {Tle!.Name} ({Tle.NoradNumber}).");
        return null;
      }
    }

    internal TopocentricObservation? Observe(GroundStation groundStation, DateTime utc)
    {
      if (!Enabled) return null;

      try
      {
        return groundStation.Observe(Satellite, utc);
      }
      catch (Exception ex)
      {
        Enabled = false;
        Log.Error(ex, $"Observe failed for {Tle!.Name} ({Tle.NoradNumber}).");
        return null;
      }
    }

    internal List<SatelliteVisibilityPeriod> ComputePasses(GroundStation groundStation, DateTime startTime, DateTime endTime)
    {
      if (!Enabled) return new();

      try
      {
        return groundStation.Observe(Satellite, startTime, endTime, TimeSpan.FromSeconds(15), clipToStartTime: false);
      }
      catch (Exception ex)
      {
        Enabled = false;
        Log.Error(ex, $"ComputePasses failed for {Tle!.Name} ({Tle.NoradNumber}).");
        return new();
      }
    }

    internal bool IsGeoStationary()
    {
      if (!Enabled) return false;
      return Math.Abs(Tle!.MeanMotionRevPerDay - 1) < 0.1f;
    }

    internal List<SatelliteVisibilityPeriod> ComputeGeostationaryPasses(GroundStation groundStation)
    {
      var observation = Observe(groundStation, DateTime.UtcNow);
      if (observation == null || observation.Elevation < 0) return new();

      return new()
      {
        new SatelliteVisibilityPeriod(
          null,
          DateTime.UtcNow,
          DateTime.UtcNow + TimeSpan.FromDays(1),
          observation.Elevation,
          DateTime.UtcNow + TimeSpan.FromHours(12)
          )
      };
    }
  }
}
