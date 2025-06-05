﻿using System.Data;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace SkyRoof
{
  public partial class SatelliteDetailsPanel : DockContent
  {
    private readonly Context ctx;
    private SatnogsDbSatellite? Satellite;
    private Size DesignedSize;

    public SatelliteDetailsPanel()
    {
      InitializeComponent();
    }
    public SatelliteDetailsPanel(Context ctx)
    {
      this.ctx = ctx;

      InitializeComponent();
      DesignedSize = Size;

      ctx.SatelliteDetailsPanel = this;
      ctx.MainForm.SatelliteDetailsMNU.Checked = true;

      SetSatellite();
    }

    public void SetSatellite(SatnogsDbSatellite? sat = null)
    {
      sat ??= ctx.SatelliteSelector.SelectedSatellite;
      sat.ComputeOrbitDetails();

      Satellite = sat;
      SatNameLabel.Text = sat.name;

      SatAkaLabel.Visible = sat.AllNames.Count > 1;
      SatAkaLabel.Text = $"a.k.a. {string.Join(", ", sat.AllNames.Where(s => s != sat.name))}";
      toolTip1.SetToolTip(SatAkaLabel, SatAkaLabel.Text);

      ImageLabel.Visible = !string.IsNullOrEmpty(sat.image);
      WebsiteLabel.Visible = !string.IsNullOrEmpty(sat.website);

      SatellitePropertyGrid.SelectedObject = sat;
    }

    private void SatelliteDetailsPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.SatelliteDetailsPanel = null;
      ctx.MainForm.SatelliteDetailsMNU.Checked = false;
    }

    private void ImageLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      ImageLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo($"https://db-satnogs.freetls.fastly.net/media/{Satellite.image}") { UseShellExecute = true });
    }

    private void WebsiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      WebsiteLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo(Satellite.website) { UseShellExecute = true });
    }

    private void SatnogsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      SatnogsLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo($"https://db.satnogs.org/satellite/{Satellite.sat_id}") { UseShellExecute = true });
    }
  }
}
