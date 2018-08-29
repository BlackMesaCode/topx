using System.Drawing;
using System.Windows.Forms;

namespace TopX.Crosshairs
{
    public class GreenCrosshair : CrosshairBase
    {
        public override void Draw(PaintEventArgs e)
        {
            // draw center dot
            var xhairWidth = 4;
            var xhairHeight = 4;
            var xPos = (Screen.PrimaryScreen.Bounds.Width / 2) - (xhairWidth / 2);
            var yPos = (Screen.PrimaryScreen.Bounds.Height / 2) - (xhairHeight / 2);
            var brush = new SolidBrush(Color.FromArgb(254, 0, 255, 0));
            e.Graphics.FillRectangle(brush, xPos, yPos, xhairWidth, xhairHeight);

            // draw helper lines
            var helperLineWidth = 20;
            var offset = 12;
            e.Graphics.FillRectangle(brush, xPos - offset - helperLineWidth, yPos, helperLineWidth, 1);
            e.Graphics.FillRectangle(brush, xPos + xhairWidth + offset, yPos, helperLineWidth, 1);


        }
    }
}
