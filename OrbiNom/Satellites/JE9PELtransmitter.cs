﻿namespace OrbiNom
{
  public class JE9PELtransmitter
  {
    public string Name;
    public int NoradId;
    public string Uplink;
    public string Downlink;
    public string Beacon;
    public string Mode;
    public string Call;
    public string Status;

    public JE9PELtransmitter() { }

    public JE9PELtransmitter(string csv)
    {
      var cols = csv.Split([';']);

      Name = cols[0];
      int.TryParse(cols[1], out NoradId);
      Uplink = cols[2];
      Downlink = cols[3];
      Beacon = cols[4];
      Mode = cols[5];
      Call = cols[6];
      Status = cols[7];
    }
  }
}
