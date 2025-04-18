﻿using System.Text.RegularExpressions;
using System.Web;
using Serilog;
using VE3NEA;

namespace OrbiNom
{
  public partial class AmsatReportDialog : Form
  {
    private Context ctx;
    private SatnogsDbSatellite Satellite;

    public AmsatReportDialog()
    {
      InitializeComponent();
    }

    public static void SendReport(Context ctx, SatnogsDbSatellite satellite)
    {
      var dialog = new AmsatReportDialog();
      dialog.ctx = ctx;
      dialog.Satellite = satellite;
      dialog.ShowDialog();
    }

    private void AmsatReportDialog_Load(object sender, EventArgs e)
    {
      comboBox1.Items.AddRange(Satellite.AmsatEntries.ToArray());
      comboBox1.SelectedIndex = 0;

      if (Satellite.norad_cat_id == 25544)
        comboBox2.Items.AddRange(["Heard", "Telemetry Only", "Not Heard", "Crew Active"]);
      else
        comboBox2.Items.AddRange(["Heard", "Telemetry Only", "Not Heard"]);

      comboBox2.SelectedIndex = 0;
    }


    private void okBtn_Click(object sender, EventArgs e)
    {
      var queryParams = HttpUtility.ParseQueryString("");
      var now = DateTime.UtcNow;

      queryParams["SatSubmit"] = "yes";
      queryParams["Confirm"] = "yes";
      queryParams["SatName"] = comboBox1.Text;
      queryParams["SatYear"] = $"{now.Year}";
      queryParams["SatMonth"] = $"{now.Month:D2}";
      queryParams["SatDay"] = $"{now.Day:D2}";
      queryParams["SatHour"] = $"{now.Hour}";
      queryParams["SatPeriod"] = $"{now.Minute / 15}";
      queryParams["SatCall"] = $"{ctx.Settings.User.Call.ToUpper()}";
      queryParams["SatReport"] = comboBox2.Text;
      queryParams["SatGridSquare"] = $"{ctx.Settings.User.Square.ToUpper()}";
      queryParams["App"] = "OrbiNom";

      string urlString = $"https://www.amsat.org/status/submit.php?{queryParams}";
      HttpClient client = new();
      var response = client.GetAsync(urlString).Result;
      var content = response.Content.ReadAsStringAsync().Result;
      content = Utils.HtmlToText(content).Replace("\n", " ");
      content = Regex.Replace(content, @"\s+", " ");
      Log.Information($"AMSAT requesdt: {urlString}");
      Log.Information($"AMSAT reply: {content}");

      if (!response.IsSuccessStatusCode)
      {
        string errorMessage = $"Error {response.StatusCode} sending AMSAT report";
        Log.Error(errorMessage);
        MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      Close();
    }


    /*

POST
	https://www.amsat.org/status/submit.php
SatName=RS-44&SatReport=Heard&SatMonth=03&SatDay=19&SatYear=2025&SatHour=2&SatPeriod=2&SatCall=VE3NEA&SatGridSquare=FN03GW&SatSubmit=Submit+Data
    /status/submit.php?
SatSubmit=yes&Confirm=yes&SatName=RS-44&SatYear=2025&SatMonth=03&SatDay=19&SatHour=21&SatPeriod=0&SatCall=VE3NEA&SatReport=Heard&SatGridSquare=FN03gw

GET
https://www.amsat.org/status/submit.php?SatSubmit=yes&Confirm=yes&SatName=RS-44&SatYear=2025&SatMonth=03&SatDay=19&SatHour=2&SatPeriod=2&SatCall=VE3NEA&SatReport=Heard&SatGridSquare=FN03gw
*/
  }
}
