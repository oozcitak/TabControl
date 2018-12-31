using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected internal class TabLocationSelector : Control
        {
            #region Member Variables
            private TabLocation hoveredLocation = TabLocation.None;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the selected tab location display.
            /// </summary>
            public TabLocation TabLocation { get; set; } = TabLocation.TopLeft;
            #endregion

            #region Constructor
            public TabLocationSelector()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
                DoubleBuffered = true;

                Size = new Size(180, 120);
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Determines the tab location display under the mouse cursor.
            /// </summary>
            private TabLocation HitTest(int x, int y)
            {
                var bounds = ClientRectangle;

                int w = bounds.Width / 5;
                int h = bounds.Height / 5;

                TabLocation newLoc = TabLocation.None;

                if (y < h && x > w && x < 4 * w)
                    newLoc = TabLocation.Top;
                else if (y > 4 * h && x > w && x < 4 * w)
                    newLoc = TabLocation.Bottom;
                else if (x < w && y > h && y < 4 * h)
                    newLoc = TabLocation.Left;
                else if (x > 4 * w && y > h && y < 4 * h)
                    newLoc = TabLocation.Right;

                if (newLoc != TabLocation.None)
                {
                    if (newLoc == TabLocation.Top || newLoc == TabLocation.Bottom)
                    {
                        if (x < 2 * w)
                            newLoc |= TabLocation.Near;
                        else if (x < 3 * w)
                            newLoc |= TabLocation.Center;
                        else
                            newLoc |= TabLocation.Far;
                    }
                    else if (newLoc == TabLocation.Left || newLoc == TabLocation.Right)
                    {
                        if (y < 2 * h)
                            newLoc |= TabLocation.Near;
                        else if (y < 3 * h)
                            newLoc |= TabLocation.Center;
                        else
                            newLoc |= TabLocation.Far;
                    }
                }

                return newLoc;
            }

            /// <summary>
            /// Draws a tab location display.
            /// </summary>
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
            #endregion

            #region Overridden Methods
            protected override void OnMouseMove(MouseEventArgs e)
            {
                var newHoveredLocation = HitTest(e.X, e.Y);
                if (hoveredLocation != newHoveredLocation)
                {
                    hoveredLocation = newHoveredLocation;
                    Invalidate();
                }

                base.OnMouseMove(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                if (hoveredLocation != TabLocation.None)
                {
                    hoveredLocation = TabLocation.None;
                    Invalidate();
                }

                base.OnMouseLeave(e);
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                TabLocation newLoc = HitTest(e.X, e.Y);

                if (newLoc != TabLocation.None && newLoc != TabLocation)
                {
                    TabLocation = newLoc;
                    Invalidate();
                }

                base.OnMouseClick(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var bounds = ClientRectangle;

                e.Graphics.Clear(SystemColors.Window);

                // Draw tab location displays
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

                // Draw hovered tab location display
                if (hoveredLocation != TabLocation.None)
                    DrawTab(e.Graphics, hoveredLocation, bounds, SystemBrushes.ControlLight, SystemPens.ControlDark);

                // Draw selected tab location
                if ((TabLocation & TabLocation.Left) != TabLocation.None || (TabLocation & TabLocation.Right) != TabLocation.None ||
                    (TabLocation & TabLocation.Top) != TabLocation.None || (TabLocation & TabLocation.Bottom) != TabLocation.None)
                    DrawTab(e.Graphics, TabLocation, bounds, SystemBrushes.Highlight, SystemPens.ControlDark);

                // Draw a representation of the control
                var controlBounds = new Rectangle(bounds.Left + bounds.Width / 5, bounds.Top + bounds.Height / 5, bounds.Width * 3 / 5 + 1, bounds.Height * 3 / 5 + 1);
                controlBounds.Inflate(-4, -4);
                e.Graphics.DrawRectangleFixed(SystemPens.WindowText, controlBounds);
                controlBounds.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics, controlBounds, SystemColors.ControlText, SystemColors.Window);
            }
            #endregion
        }
    }
}
