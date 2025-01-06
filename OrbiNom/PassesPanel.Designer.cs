﻿namespace OrbiNom
{
  partial class PassesPanel
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      flowLayoutPanel1 = new FlowLayoutPanel();
      CurrentSatBtn = new RadioButton();
      GroupBtn = new RadioButton();
      AllBtn = new RadioButton();
      panel1 = new Panel();
      listViewEx1 = new VE3NEA.ListViewEx();
      columnHeader1 = new ColumnHeader();
      flowLayoutPanel1.SuspendLayout();
      panel1.SuspendLayout();
      SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      flowLayoutPanel1.Controls.Add(CurrentSatBtn);
      flowLayoutPanel1.Controls.Add(GroupBtn);
      flowLayoutPanel1.Controls.Add(AllBtn);
      flowLayoutPanel1.Dock = DockStyle.Top;
      flowLayoutPanel1.Location = new Point(0, 0);
      flowLayoutPanel1.Name = "flowLayoutPanel1";
      flowLayoutPanel1.Size = new Size(434, 27);
      flowLayoutPanel1.TabIndex = 0;
      // 
      // CurrentSatBtn
      // 
      CurrentSatBtn.AutoSize = true;
      CurrentSatBtn.Checked = true;
      CurrentSatBtn.Location = new Point(3, 3);
      CurrentSatBtn.Name = "CurrentSatBtn";
      CurrentSatBtn.Size = new Size(109, 19);
      CurrentSatBtn.TabIndex = 0;
      CurrentSatBtn.TabStop = true;
      CurrentSatBtn.Text = "Current Satellite";
      CurrentSatBtn.UseVisualStyleBackColor = true;
      CurrentSatBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // GroupBtn
      // 
      GroupBtn.AutoSize = true;
      GroupBtn.Location = new Point(118, 3);
      GroupBtn.Name = "GroupBtn";
      GroupBtn.Size = new Size(58, 19);
      GroupBtn.TabIndex = 1;
      GroupBtn.Text = "Group";
      GroupBtn.UseVisualStyleBackColor = true;
      GroupBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // AllBtn
      // 
      AllBtn.AutoSize = true;
      AllBtn.Location = new Point(182, 3);
      AllBtn.Name = "AllBtn";
      AllBtn.Size = new Size(92, 19);
      AllBtn.TabIndex = 2;
      AllBtn.Text = "All VHF/UHF";
      AllBtn.UseVisualStyleBackColor = true;
      AllBtn.CheckedChanged += radioButton_CheckedChanged;
      // 
      // panel1
      // 
      panel1.AutoScroll = true;
      panel1.Controls.Add(listViewEx1);
      panel1.Dock = DockStyle.Fill;
      panel1.Location = new Point(0, 27);
      panel1.Name = "panel1";
      panel1.Size = new Size(434, 582);
      panel1.TabIndex = 1;
      // 
      // listViewEx1
      // 
      listViewEx1.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
      listViewEx1.Dock = DockStyle.Fill;
      listViewEx1.FullRowSelect = true;
      listViewEx1.HeaderStyle = ColumnHeaderStyle.None;
      listViewEx1.Location = new Point(0, 0);
      listViewEx1.MultiSelect = false;
      listViewEx1.Name = "listViewEx1";
      listViewEx1.OwnerDraw = true;
      listViewEx1.ShowItemToolTips = true;
      listViewEx1.Size = new Size(434, 582);
      listViewEx1.TabIndex = 0;
      listViewEx1.UseCompatibleStateImageBehavior = false;
      listViewEx1.View = View.Details;
      listViewEx1.VirtualMode = true;
      listViewEx1.DrawSubItem += listViewEx1_DrawSubItem;
      listViewEx1.RetrieveVirtualItem += listViewEx1_RetrieveVirtualItem;
      listViewEx1.Resize += listViewEx1_Resize;
      // 
      // PassesPanel
      // 
      AutoScaleMode = AutoScaleMode.Inherit;
      ClientSize = new Size(434, 609);
      Controls.Add(panel1);
      Controls.Add(flowLayoutPanel1);
      Name = "PassesPanel";
      Text = "Satellite Passes";
      FormClosing += PassesPanel_FormClosing;
      flowLayoutPanel1.ResumeLayout(false);
      flowLayoutPanel1.PerformLayout();
      panel1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel flowLayoutPanel1;
    private RadioButton CurrentSatBtn;
    private RadioButton GroupBtn;
    private Panel panel1;
    private RadioButton AllBtn;
    private VE3NEA.ListViewEx listViewEx1;
    private ColumnHeader columnHeader1;
  }
}