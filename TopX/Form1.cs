using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopX
{
    public partial class Form1 : Form
    {

        private Margins marg;

        //this is used to specify the boundaries of the transparent area
        internal struct Margins
        {
            public int Left, Right, Top, Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]

        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]

        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]

        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        public const int GWL_EXSTYLE = -20;

        public const int WS_EX_LAYERED = 0x80000;

        public const int WS_EX_TRANSPARENT = 0x20;

        public const int LWA_ALPHA = 0x2;

        public const int LWA_COLORKEY = 0x1;

        [DllImport("dwmapi.dll")]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMargins);

        private Device device = null;

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            ShowIcon = true;
            ShowInTaskbar = true;
            TopMost = true;
            Visible = true;
            WindowState = FormWindowState.Maximized;
            //BackColor = Color.Black;

            //Set the Alpha on the Whole Window to 255 (solid)
            SetLayeredWindowAttributes(this.Handle, 0, 255, LWA_ALPHA);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Set the form click-through
                cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Create a margin (the whole form)
            marg.Left = 0;
            marg.Top = 0;
            marg.Right = this.Width;
            marg.Bottom = this.Height;

            // draw what you want
            
            var xhairWidth = 4;
            var xhairHeight = 4;
            var xPos = (Screen.PrimaryScreen.Bounds.Width / 2) - (xhairWidth / 2);
            var yPos = (Screen.PrimaryScreen.Bounds.Height / 2) - (xhairHeight / 2);
            var mybrush = new SolidBrush(Color.FromArgb(254, Color.Aquamarine));
            e.Graphics.FillRectangle(mybrush, xPos, yPos, xhairWidth, xhairHeight);
            //e.Graphics.FillEllipse(Brushes.Blue, xPos, yPos, xhairWidth, xhairHeight);

            //Expand the Aero Glass Effect Border to the WHOLE form.
            // since we have already had the border invisible we now
            // have a completely invisible window - apart from the DirectX
            // renders NOT in black.
            DwmExtendFrameIntoClientArea(this.Handle, ref marg);
            
        }
    }

}
