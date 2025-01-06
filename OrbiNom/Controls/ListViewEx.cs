﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VE3NEA
{
  public class ListViewEx : ListView
  {
    //--------------------------------------------------------------------------------------------------------------
    //                 prevent flicker: https://stackoverflow.com/questions/2751686
    //--------------------------------------------------------------------------------------------------------------
    private const int WM_ERASEBKGND = 0x14;

    public ListViewEx()
    {
      SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.EnableNotifyMessage, true);
    }

    protected override void OnNotifyMessage(Message m)
    {
      if (m.Msg != WM_ERASEBKGND) base.OnNotifyMessage(m);
    }




    //--------------------------------------------------------------------------------------------------------------
    //           hide horizontal scroollbar: https://stackoverflow.com/questions/2488622
    //--------------------------------------------------------------------------------------------------------------
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);

    const int WM_NCCALCSIZE = 0x83;
    const int GWL_STYLE = -16;
    const int WS_HSCROLL = 0x00100000;

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_NCCALCSIZE)
      {
        int style = (int)GetWindowLongPtr(Handle, GWL_STYLE);
        if ((style & WS_HSCROLL) == WS_HSCROLL) 
          SetWindowLongPtr(Handle, GWL_STYLE, style & ~WS_HSCROLL);
      }

      base.WndProc(ref m);
    }




    //--------------------------------------------------------------------------------------------------------------
    //                     set row height: https://stackoverflow.com/questions/6563863
    //--------------------------------------------------------------------------------------------------------------
    public void SetRowHeight(int height)
    {
      SmallImageList = new ImageList();
      SmallImageList.ImageSize = new Size(1, height);
    }




    //--------------------------------------------------------------------------------------------------------------
    //                   set tooltip delay: https://stackoverflow.com/questions/4899687 
    //--------------------------------------------------------------------------------------------------------------
    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    const int LVM_GETTOOLTIPS = 0x104E;
    const int TTM_SETDELAYTIME = 0x403;
    const int TTDT_AUTOMATIC = 0;
    const int TTDT_AUTOPOP = 2;
    const int TTDT_INITIAL = 3;

    public void SetTooltipDelay(int delayMs)
    {
      var tooltip = SendMessage(Handle, LVM_GETTOOLTIPS, 0, 0);
      SendMessage(tooltip, TTM_SETDELAYTIME, TTDT_AUTOMATIC, delayMs);
    }
  }
}
