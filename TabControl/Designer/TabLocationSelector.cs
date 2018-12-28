using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected internal class TabLocationSelector : Control
        {
            public TabLocationSelector()
            {
                Size = new Size(180, 120);
            }

            public TabLocation TabLocation { get; set; }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                var bounds = ClientRectangle;

                int w = bounds.Width / 5;
                int h = bounds.Height / 5;
                int x = bounds.Left;
                int y = bounds.Top;

                TabLocation newLoc = TabLocation.None;

                if (e.Y < h && e.X > w && e.X < 4 * w)
                    newLoc = TabLocation.Top;
                else if (e.Y > 4 * h && e.X > w && e.X < 4 * w)
                    newLoc = TabLocation.Bottom;
                else if (e.X < w && e.Y > h && e.Y < 4 * h)
                    newLoc = TabLocation.Left;
                else if (e.X > 4 * w && e.Y > h && e.Y < 4 * h)
                    newLoc = TabLocation.Right;

                if (newLoc != TabLocation.None)
                {
                    if (newLoc == TabLocation.Top || newLoc == TabLocation.Bottom)
                    {
                        if (e.X < 2 * w)
                            newLoc |= TabLocation.Near;
                        else if (e.X < 3 * w)
                            newLoc |= TabLocation.Center;
                        else
                            newLoc |= TabLocation.Far;
                    }
                    else if (newLoc == TabLocation.Left || newLoc == TabLocation.Right)
                    {
                        if (e.Y < 2 * h)
                            newLoc |= TabLocation.Near;
                        else if (e.Y < 3 * h)
                            newLoc |= TabLocation.Center;
                        else
                            newLoc |= TabLocation.Far;
                    }

                    TabLocation = newLoc;
                }

                base.OnMouseClick(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var bounds = ClientRectangle;

                e.Graphics.Clear(SystemColors.Window);

                DrawTab(e.Graphics, TabLocation.LeftTop, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.LeftCenter, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.LeftBottom, bounds, SystemBrushes.Control, SystemPens.ControlDark);

                DrawTab(e.Graphics, TabLocation.RightTop, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.RightCenter, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.RightBottom, bounds, SystemBrushes.Control, SystemPens.ControlDark);

                DrawTab(e.Graphics, TabLocation.TopLeft, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.TopCenter, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.TopRight, bounds, SystemBrushes.Control, SystemPens.ControlDark);

                DrawTab(e.Graphics, TabLocation.BottomLeft, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.BottomCenter, bounds, SystemBrushes.Control, SystemPens.ControlDark);
                DrawTab(e.Graphics, TabLocation.BottomRight, bounds, SystemBrushes.Control, SystemPens.ControlDark);

                if ((TabLocation & TabLocation.Left) != TabLocation.None || (TabLocation & TabLocation.Right) != TabLocation.None ||
                    (TabLocation & TabLocation.Top) != TabLocation.None || (TabLocation & TabLocation.Bottom) != TabLocation.None)
                    DrawTab(e.Graphics, TabLocation, bounds, SystemBrushes.Highlight, SystemPens.ControlLight);
            }

            private void DrawTab(Graphics g, TabLocation location, Rectangle bounds, Brush backBrush, Pen borderPen)
            {
                int w = bounds.Width / 5;
                int h = bounds.Height / 5;
                int x = 0;
                int y = 0;

                if ((location & TabLocation.Top) != TabLocation.None || (location & TabLocation.Bottom) != TabLocation.None)
                {
                    if ((location & TabLocation.Top) != TabLocation.None)
                        y = bounds.Top;
                    else
                        y = bounds.Top + 4 * h;

                    if ((location & TabLocation.Near) != TabLocation.None)
                        x = bounds.Left + w;
                    else if ((location & TabLocation.Center) != TabLocation.None)
                        x = bounds.Left + 2 * w;
                    else
                        x = bounds.Left + 3 * w;
                }
                else if ((location & TabLocation.Left) != TabLocation.None || (location & TabLocation.Right) != TabLocation.None)
                {
                    if ((location & TabLocation.Left) != TabLocation.None)
                        x = bounds.Left;
                    else
                        x = bounds.Left + 4 * w;

                    if ((location & TabLocation.Near) != TabLocation.None)
                        y = bounds.Top + h;
                    else if ((location & TabLocation.Center) != TabLocation.None)
                        y = bounds.Top + 2 * h;
                    else
                        y = bounds.Top + 3 * h;
                }

                g.FillRectangle(backBrush, x, y, w, h);
                g.DrawRectangle(borderPen, x, y, w, h);
            }
        }
    }
}
