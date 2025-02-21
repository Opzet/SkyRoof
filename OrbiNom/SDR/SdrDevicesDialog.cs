﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VE3NEA;

namespace OrbiNom
{
  public partial class SdrDevicesDialog : Form
  {
    private readonly Context ctx;
    private readonly Image RadioOnImage, RadioOffImage;
    public List<SoapySdrDeviceInfo> Devices;
    private string? SelectedDeviceName;

    public SdrDevicesDialog()
    {
      InitializeComponent();
    }

    public SdrDevicesDialog(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;

      using (var ms = new MemoryStream(Properties.Resources.radio_button_on)) { RadioOnImage = Image.FromStream(ms); }
      using (var ms = new MemoryStream(Properties.Resources.radio_button_off)) { RadioOffImage = Image.FromStream(ms); }

      SelectedDeviceName = ctx.Settings.Sdr.SelectedDeviceName;
      BuildDeviceList();
    }

    private void BuildDeviceList()
    {
      // previously detected devices
      Devices = Utils.DeepClone(ctx.Settings.Sdr.Devices);
      var oldNames = Devices.Select(x => x.Name).ToList();

      // currently available devices
      var presentDevices = SoapySdr.EnumerateDevices();
      var presentNames = presentDevices.Select(x => x.Name).ToList();

      // keep previous 
      foreach (var dev in Devices) dev.Present = presentNames.Contains(dev.Name);
      
      // add new
      var newDevices = presentDevices.Where(dev => !oldNames.Contains(dev.Name));
      foreach (var dev in newDevices) ctx.MainForm.SuggestSdrSettings(dev);
      Devices.AddRange(newDevices);

      // to listbox
      listBox1.Items.Clear();
      listBox1.Items.AddRange(Devices.ToArray());
      var index = Devices.FindIndex(dev => dev.Name == SelectedDeviceName);
      if (index == -1) index = 0;
      if (listBox1.Items.Count > index) listBox1.SelectedIndex = index;
    }




    //----------------------------------------------------------------------------------------------
    //                                       events
    //----------------------------------------------------------------------------------------------
    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listBox1.SelectedIndex == -1)
      {
        SelectedDeviceName = null;
        return;
      }

      var sdr = (SoapySdrDeviceInfo)listBox1.Items[listBox1.SelectedIndex];
      SelectedDeviceName = sdr.Name;
      Grid.SelectedObject = sdr.Properties;
      listBox1.Invalidate();
    }

    private void SdrDevicesDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        ctx.Settings.Sdr.Devices = Devices;
        ctx.Settings.Sdr.SelectedDeviceName = SelectedDeviceName;
      }
    }

    private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
    {
      var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
      Brush brush = selected ? Brushes.Lavender : Brushes.White;
      e.Graphics.FillRectangle(brush, e.Bounds);

      if (e.Index < 0 || e.Index >= Devices.Count) return;

      var image = e.Index == listBox1.SelectedIndex ? RadioOnImage : RadioOffImage;
      e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top);

      brush = Devices[e.Index].Present ? Brushes.Black : Brushes.Gray;
      e.Graphics.DrawString(Devices[e.Index].Name, listBox1.Font, brush, e.Bounds.Left + image.Width, e.Bounds.Top);
    }

    private void DeviceListMenu_Opening(object sender, CancelEventArgs e)
    {
      bool itemPresent = ClickedItemIndex >= 0 && ClickedItemIndex < listBox1.Items.Count;
      SelectSdrMNU.Enabled = itemPresent;
      DeleteSdrMNU.Enabled = itemPresent && !Devices[ClickedItemIndex].Present;
    }
    
    private void listBox1_MouseDown(object sender, MouseEventArgs e)
    {
      ClickedItemIndex = listBox1.IndexFromPoint(e.Location);
      listBox1.Invalidate();
    }

    int ClickedItemIndex;
    private void SelectSdrMNU_Click(object sender, EventArgs e)
    {
      listBox1.SelectedIndex = ClickedItemIndex;
    }

    private void DeleteSdrMNU_Click(object sender, EventArgs e)
    {
      Devices.RemoveAt(ClickedItemIndex);
      listBox1.Items.RemoveAt(ClickedItemIndex);
      if (listBox1.SelectedIndex == -1 && listBox1.Items.Count > 0)
        listBox1.SelectedIndex = 0;
    }




    //----------------------------------------------------------------------------------------------
    //                              usb device list changed
    //----------------------------------------------------------------------------------------------
    private const int WM_DEVICECHANGE = 0x219;
    private const int DBT_DEVNODES_CHANGED = 7;
    protected override void WndProc(ref Message m)
    {
      base.WndProc(ref m);
      if (m.Msg == WM_DEVICECHANGE && m.WParam == DBT_DEVNODES_CHANGED)
        Invoke(BuildDeviceList);
    }
  }
}
