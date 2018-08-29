using System.Drawing;
using System.Windows.Forms;

namespace TopX.Crosshairs
{
    public class SimpleAquamarineCrosshair : CrosshairBase
    {
        public override void Draw(PaintEventArgs e)
        {
            var xhairWidth = 4;
            var xhairHeight = 4;
            var xPos = (Screen.PrimaryScreen.Bounds.Width / 2) - (xhairWidth / 2);
            var yPos = (Screen.PrimaryScreen.Bounds.Height / 2) - (xhairHeight / 2);
            var brush = new SolidBrush(Color.FromArgb(254, Color.Aquamarine));
            e.Graphics.FillRectangle(brush, xPos, yPos, xhairWidth, xhairHeight);
        }
    }
}
