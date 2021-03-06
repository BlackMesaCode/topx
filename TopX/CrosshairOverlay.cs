﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TopX.Crosshairs;

namespace TopX
{
    public partial class CrosshairOverlay : Form
    {
        public SystemTray SystemTray { get; }
        public ProcessWatcher ProcessWatcher { get; set; }
        public IEnumerable<string> ProcessesToWatch { get; set; }
        public CrosshairBase CurrentCrosshair { get; set; }

        public CrosshairOverlay(IEnumerable<string> processesToWatch)
        {
            InitializeComponent();

            // Setup ProcessWatcher
            ProcessesToWatch = processesToWatch;
            ProcessWatcher = new ProcessWatcher(ProcessesToWatch);
            ProcessWatcher.ProcessStarted += ProcessWatcher_ProcessStarted;
            ProcessWatcher.ProcessStopped += ProcessWatcher_ProcessStopped;

            // Setup Form
            FormBorderStyle = FormBorderStyle.None;
            ShowIcon = true;
            ShowInTaskbar = false;
            TopMost = true;
            WindowState = FormWindowState.Maximized;
            SystemTray = new SystemTray(this.NotifyIcon);
            SystemTray.ToggleMenuItem.Click += ToggleMenuItem_Click;
            Load += CrosshairOverlay_Load;
            FormClosed += CrosshairOverlay_FormClosed;

            // Setup Crosshair
            CurrentCrosshair = new GreenCrosshair();

            //Set Alpha to Solid
            Win32.SetLayeredWindowAttributes(this.Handle, 0, 255, Win32.LWA_ALPHA);
        }

        private void CrosshairOverlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            ProcessWatcher?.Dispose();
        }

        private IEnumerable<string> GetRunningProcesses()
        {
            return Process.GetProcesses().Select(process => process.ProcessName.ToLower());
        }

        private void CrosshairOverlay_Load(object sender, System.EventArgs e)
        {
            var runningProcesses = GetRunningProcesses();
            if (runningProcesses.Any(process => ProcessesToWatch.Contains(process)))
                Visible = true;
            else
                Visible = false;
        }

        private void ProcessWatcher_ProcessStopped(string obj)
        {
            if (Visible && !GetRunningProcesses().Intersect(ProcessesToWatch).Any())
            {
                Invoke((MethodInvoker)delegate ()
                {
                    Visible = false;
                });
            }
        }

        private void ProcessWatcher_ProcessStarted(string obj)
        {
            if (!Visible)
            {
                Invoke((MethodInvoker)delegate ()
                {
                    Visible = true;
                });
            }
        }

        private void ToggleMenuItem_Click(object sender, System.EventArgs e)
        {
            Visible = !Visible;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams _params = base.CreateParams;

                // Set the form click-through
                _params.ExStyle |= Win32.WS_EX_LAYERED | Win32.WS_EX_TRANSPARENT;
                return _params;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            CurrentCrosshair.Draw(e);

            var margins = new Margins
            {
                Left = 0,
                Top = 0,
                Right = this.Width,
                Bottom = this.Height
            };

            // DwmExtendFrameIntoClientArea is part of the "Aero effects library"
            // Negative margins would have special meaning (glass effect) to DwmExtendFrameIntoClientArea - but we dont use the glassy look
            // we just use the function to render everything transparent that is of the color black
            Win32.DwmExtendFrameIntoClientArea(this.Handle, ref margins);
            
        }

    }

}
