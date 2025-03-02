﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SGPdotNET.Observation;
using WeifenLuo.WinFormsUI.Docking;

namespace OrbiNom
{
  public partial class TransmittersPanel : DockContent
  {
    private Context ctx;
    private SatnogsDbSatellite Satellite;

    public TransmittersPanel()
    {
      InitializeComponent();
    }

    public TransmittersPanel(Context ctx)
    {
      InitializeComponent();

      this.ctx = ctx;
      ctx.TransmittersPanel = this;
      ctx.MainForm.TransmittersMNU.Checked = true;
      SetSatellite();
    }

    internal void SetSatellite(SatnogsDbSatellite? sat = null)
    {
      sat ??= ctx.SatelliteSelector.SelectedSatellite; ;
      sat.ComputeOrbitDetails();
      Satellite = sat;
      CreateTransmitterItems();

      SatNameLabel.Text = sat.name;
    }

    private void TransmittersPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.TransmittersPanel = null;
      ctx.MainForm.TransmittersMNU.Checked = false;
    }

    private void CreateTransmitterItems()
    {
      listView1.BeginUpdate();
      listView1.Items.Clear();
      listView1.Groups.Clear();
      listView1.Groups.Add(new ListViewGroup("SatNOGS"));
      listView1.Groups.Add(new ListViewGroup("JE9PEL"));

      // satnogs transmitters
      foreach (var tx in Satellite.Transmitters)
      {
        // columns
        var item = new ListViewItem([
          tx.description,
          SatnogsDbTransmitter.FormatFrequencyRange(tx.downlink_low, tx.downlink_high, tx.invert),
          SatnogsDbTransmitter.FormatFrequencyRange(tx.uplink_low, tx.uplink_high),
        ]);
        item.Group = listView1.Groups[0];
        item.Tag = tx;

        // highlighting
        if (tx.IsVhf()) item.BackColor = Color.LightGoldenrodYellow;
        if (tx.IsUhf()) item.BackColor = Color.LightCyan;
        if (tx.service == "Amateur") item.Font = new(item.Font, FontStyle.Bold);
        if (!tx.alive || tx.status != "active") item.ForeColor = Color.Silver; //item.Font = new(item.Font, FontStyle.Strikeout);

        // tooltip
        item.ToolTipText = tx.GetTooltipText();

        listView1.Items.Add(item);
      }

      // JE9PEL transmitters
      foreach (var t in Satellite.JE9PELtransmitters)
      {
        var item = new ListViewItem([t.Mode, t.Downlink, t.Uplink]);
        item.Group = listView1.Groups[1];
        if (t.Status != "active") item.ForeColor = Color.Silver;
        item.ToolTipText = t.GetTooltipText();

        // band color
        var match = Regex.Match(t.Downlink, "^[0-9.]+");
        if (match.Success && float.TryParse(match.Groups[0].Value, out float freq))
          if (freq >= 144 && freq <= 148) item.BackColor = Color.LightGoldenrodYellow;
          else if (freq >= 430 && freq <= 440) item.BackColor = Color.LightCyan;

        listView1.Items.Add(item);
      }

      listView1.EndUpdate();
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      listView1.Sort();
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      if (listView1.SelectedItems.Count == 0) return;

      var tx = listView1.SelectedItems[0].Tag as SatnogsDbTransmitter;
      if (tx == null) return;

      ctx.SatelliteSelector.SetSelectedSatellite(Satellite);
      ctx.SatelliteSelector.SetSelectedTransmitter(tx);
    }
  }
}
