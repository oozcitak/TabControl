using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected internal class TextDirectionSelector : Control
        {
            #region Member Variables
            private int hoveredDirection = -1;
            #endregion

            #region Properties
            /// <summary>
            /// Gets or sets the selected text direction.
            /// </summary>
            public TextDirection TextDirection { get; set; } = TextDirection.Right;
            #endregion

            #region Constructor
            public TextDirectionSelector()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
                DoubleBuffered = true;

                Size = new Size(120, 120);

                Font = new Font(Font.FontFamily, 14, FontStyle.Italic);
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Determines the text direction display under the mouse cursor.
            /// </summary>
            private int HitTest(int x, int y)
            {
                var bounds = ClientRectangle;

                if (!bounds.Contains(x, y))
                    return -1;

                if (y < bounds.Height / 3)
                    return (int)TextDirection.Right;
                else if (x < bounds.Width / 2)
                    return (int)TextDirection.Down;
                else
                    return (int)TextDirection.Up;
            }

            /// <summary>
            /// Draws a text direction display.
            /// </summary>
            private void DrawDirection(Graphics g, TextDirection direction, Rectangle bounds, Color backColor, Color foreColor, Color borderColor)
            {
                if (direction == TextDirection.Right)
                    bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height / 3);
                else if (direction == TextDirection.Down)
                    bounds = new Rectangle(bounds.Left, bounds.Top + bounds.Height / 3, bounds.Width / 2, bounds.Height * 2 / 3);
                else
                    bounds = new Rectangle(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 3, bounds.Width / 2, bounds.Height * 2 / 3);

                string sampleText = "abc";
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

                using (var backBrush = new SolidBrush(backColor))
                using (var borderPen = new Pen(borderColor))
                {
                    g.FillRectangle(backBrush, bounds);
                    g.DrawRectangle(borderPen, bounds);
                }

                var textBounds = bounds.GetInflated(-4, -4);
                if (direction == TextDirection.Right)
                    TextRenderer.DrawText(g, sampleText, Font, textBounds, foreColor, backColor, flags);
                else if (direction == TextDirection.Down)
                    g.DrawVerticalTextDown(sampleText, Font, textBounds, foreColor, backColor, flags);
                else
                    g.DrawVerticalTextUp(sampleText, Font, textBounds, foreColor, backColor, flags);
            }
            #endregion

            #region Overridden Methods
            protected override void OnMouseMove(MouseEventArgs e)
            {
                var newHoveredDirection = HitTest(e.X, e.Y);
                if (hoveredDirection != newHoveredDirection)
                {
                    hoveredDirection = newHoveredDirection;
                    Invalidate();
                }

                base.OnMouseMove(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                if (hoveredDirection != -1)
                {
                    hoveredDirection = -1;
                    Invalidate();
                }

                base.OnMouseLeave(e);
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                var newDirection = HitTest(e.X, e.Y);

                if (newDirection != -1 && newDirection != (int)TextDirection)
                {
                    TextDirection = (TextDirection)newDirection;
                    Invalidate();
                }

                base.OnMouseClick(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var bounds = ClientRectangle;

                e.Graphics.Clear(SystemColors.Window);

                // Draw text direction displays
                DrawDirection(e.Graphics, TextDirection.Right, bounds, SystemColors.Control, SystemColors.ControlText, SystemColors.ControlDark);
                DrawDirection(e.Graphics, TextDirection.Down, bounds, SystemColors.Control, SystemColors.ControlText, SystemColors.ControlDark);
                DrawDirection(e.Graphics, TextDirection.Up, bounds, SystemColors.Control, SystemColors.ControlText, SystemColors.ControlDark);

                // Draw hovered text direction display
                if (hoveredDirection != -1)
                    DrawDirection(e.Graphics, (TextDirection)hoveredDirection, bounds, SystemColors.ControlLight, SystemColors.ControlText, SystemColors.ControlDark);

                // Draw selected text direction
                DrawDirection(e.Graphics, TextDirection, bounds, SystemColors.Highlight, SystemColors.HighlightText, SystemColors.ControlDark);
            }
            #endregion
        }
    }
}
