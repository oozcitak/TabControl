using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents the renderer of the <see cref="TabControl"/>.
        /// </summary>
        public class TabControlRenderer
        {
            #region DrawTabHeaderParams
            /// <summary>
            /// Represents the parameters required to draw a tab header.
            /// </summary>
            public struct DrawTabHeaderParams
            {
                /// <summary>
                /// Index of the tab in its container collection.
                /// </summary>
                public int Index { get; private set; }
                /// <summary>
                /// Tab text.
                /// </summary>
                public string Text { get; private set; }
                /// <summary>
                /// Visual state of the tab.
                /// </summary>
                public TabControl.TabHeaderState State { get; private set; }
                /// <summary>
                /// Bounding rectangle of the tab.
                /// </summary>
                public Rectangle Bounds { get; private set; }

                public DrawTabHeaderParams(int index, string text, Rectangle bounds, TabControl.TabHeaderState state)
                {
                    Index = index;
                    Text = text;
                    Bounds = bounds;
                    State = state;
                }
            }
            #endregion

            #region DrawTabHeaderParamsComparer
            /// <summary>
            /// Compares items so that they are sorted as: Inactive > Hot > Active > Pressed
            /// </summary>
            private class DrawTabHeaderParamsComparer : IComparer<DrawTabHeaderParams>
            {
                /// <summary>
                /// Compares tabs by draw order.
                /// </summary>
                public int Compare(DrawTabHeaderParams p1, DrawTabHeaderParams p2)
                {
                    if (ReferenceEquals(p1, p2) || p1.Index == p2.Index)
                        return 0;

                    if ((p1.State & TabControl.TabHeaderState.Pressed) != TabControl.TabHeaderState.Inactive)
                        return 1;
                    else if ((p2.State & TabControl.TabHeaderState.Pressed) != TabControl.TabHeaderState.Inactive)
                        return -1;
                    else if ((p1.State & TabControl.TabHeaderState.Active) != TabControl.TabHeaderState.Inactive)
                        return 1;
                    else if ((p2.State & TabControl.TabHeaderState.Active) != TabControl.TabHeaderState.Inactive)
                        return -1;
                    else if ((p1.State & TabControl.TabHeaderState.Hot) != TabControl.TabHeaderState.Inactive)
                        return 1;
                    else if ((p2.State & TabControl.TabHeaderState.Hot) != TabControl.TabHeaderState.Inactive)
                        return -1;

                    return p1.Index.CompareTo(p2.Index);
                }
            }
            #endregion

            #region Properties
            /// <summary>
            /// Gets the parent control.
            /// </summary>
            public TabControl Parent { get; protected set; }

            /// <summary>
            /// Gets the background color of tab header area.
            /// </summary>
            public virtual Color BackColor { get; protected set; } = SystemColors.Control;
            /// <summary>
            /// Gets the foreground color of tab header area.
            /// </summary>
            public virtual Color ForeColor { get; protected set; } = Color.Black;
            /// <summary>
            /// Gets the color of control border.
            /// </summary>
            public virtual Color BorderColor { get; protected set; } = Color.Black;

            /// <summary>
            /// Gets the font of tab headers.
            /// </summary>
            public virtual Font Font { get; protected set; }

            /// <summary>
            /// Gets the background color of inactive tabs.
            /// </summary>
            public virtual Color InactiveTabBackColor { get; protected set; } = Color.FromArgb(225, 225, 225);
            /// <summary>
            /// Gets the background color of active tabs.
            /// </summary>
            public virtual Color ActiveTabBackColor { get; protected set; } = Color.White;
            /// <summary>
            /// Gets the background color of hot tabs.
            /// </summary>
            public virtual Color HotTabBackColor { get; protected set; } = Color.FromArgb(235, 235, 235);
            /// <summary>
            /// Gets the background color of pressed tabs.
            /// </summary>
            public virtual Color PressedTabBackColor { get; protected set; } = Color.White;

            /// <summary>
            /// Gets the foreground color of inactive tabs.
            /// </summary>
            public virtual Color InactiveTabForeColor { get; protected set; } = Color.Black;
            /// <summary>
            /// Gets the foreground color of active tabs.
            /// </summary>
            public virtual Color ActiveTabForeColor { get; protected set; } = Color.Black;
            /// <summary>
            /// Gets the foreground color of hot tabs.
            /// </summary>
            public virtual Color HotTabForeColor { get; protected set; } = Color.Black;
            /// <summary>
            /// Gets the foreground color of pressed tabs.
            /// </summary>
            public virtual Color PressedTabForeColor { get; protected set; } = Color.Black;

            /// <summary>
            /// Gets the color of tab separators.
            /// </summary>
            public virtual Color SeparatorColor { get; protected set; } = Color.FromArgb(166, 166, 166);
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="TabControlRenderer"/> class.
            /// </summary>
            public TabControlRenderer(TabControl parent)
            {
                Parent = parent;

                Font = parent.Font;
            }
            #endregion

            #region Render Methods
            /// <summary>
            /// Draws the background of the tab header area.
            /// </summary>
            public virtual void Render(Graphics g)
            {
                // clear backgound
                g.Clear(BackColor);

                // sort headers
                var drawParams = new List<DrawTabHeaderParams>();
                for (int i = 0; i < Parent.Pages.Count; i++)
                {
                    var state = Parent.GetTabState(i);
                    var tabBounds = Parent.GetTabHeaderBounds(i);
                    var text = Parent.Pages[i].Text;
                    drawParams.Add(new DrawTabHeaderParams(i, text, tabBounds, state));
                }
                drawParams.Sort(new DrawTabHeaderParamsComparer());

                // draw headers
                foreach (var param in drawParams)
                {
                    DrawTabHeaderBackGround(g, param);
                    DrawTabHeaderText(g, param);
                    DrawSeparator(g, param);
                }

                // draw border
                if (Parent.BorderStyle != BorderStyle.None)
                {
                    var borderBounds = Parent.DisplayRectangle;
                    borderBounds.Inflate(1, 1);
                    DrawBorder(g, borderBounds);
                }
            }

            /// <summary>
            /// Draws the backgound of a tab header.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab header.</param>
            public virtual void DrawTabHeaderBackGround(Graphics g, DrawTabHeaderParams param)
            {
                using (var brush = new SolidBrush(GetTabBackColor(param.State)))
                {
                    g.FillRectangle(brush, param.Bounds);
                }
            }

            /// <summary>
            /// Draws the text of a tab header.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab header.</param>
            public virtual void DrawTabHeaderText(Graphics g, DrawTabHeaderParams param)
            {
                if (string.IsNullOrEmpty(param.Text)) return;

                var backColor = GetTabBackColor(param.State);
                var foreColor = GetTabForeColor(param.State);

                TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
                if (Parent.TextAlignment == TabControl.Alignment.Far)
                    flags |= TextFormatFlags.Right;
                else if (Parent.TextAlignment == TabControl.Alignment.Center)
                    flags |= TextFormatFlags.HorizontalCenter;

                var textBounds = param.Bounds;
                textBounds.Inflate(-4, -4);

                if (Parent.TabHeaderLocation == TabControl.TabLocation.Top || Parent.TabHeaderLocation == TabControl.TabLocation.Bottom)
                {
                    TextRenderer.DrawText(g, param.Text, Font, textBounds, foreColor, backColor, flags);
                }
                else
                {
                    var imageBounds = new Rectangle(0, 0, param.Bounds.Height, param.Bounds.Width);
                    textBounds = imageBounds;
                    textBounds.Inflate(-4, -4);

                    using (var image = new Bitmap(imageBounds.Width, imageBounds.Height, PixelFormat.Format32bppArgb))
                    using (Graphics imageGraphics = Graphics.FromImage(image))
                    {
                        TextRenderer.DrawText(imageGraphics, param.Text, Font, textBounds, foreColor, backColor, flags);
                        // Rotate, translate and draw the image
                        Point[] ptMap = new Point[3];
                        if (Parent.TabHeaderLocation == TabControl.TabLocation.Top || Parent.TabHeaderLocation == TabControl.TabLocation.Bottom)
                        {
                            ptMap[0] = param.Bounds.GetTopLeft();    // upper-left
                            ptMap[1] = param.Bounds.GetTopRight();   // upper-right
                            ptMap[2] = param.Bounds.GetBottomLeft(); // lower-left
                        }
                        else if (Parent.TabHeaderLocation == TabControl.TabLocation.Left)
                        {
                            ptMap[0] = param.Bounds.GetBottomLeft();  // upper-left
                            ptMap[1] = param.Bounds.GetTopLeft();     // upper-right
                            ptMap[2] = param.Bounds.GetBottomRight(); // lower-left
                        }
                        else // if (Parent.TabHeaderLocation == TabControl.TabLocation.Right)
                        {
                            ptMap[0] = param.Bounds.GetTopRight();    // upper-left
                            ptMap[1] = param.Bounds.GetBottomRight(); // upper-right
                            ptMap[2] = param.Bounds.GetTopLeft();     // lower-left
                        }
                        g.DrawImage(image, ptMap);
                    }
                }
            }

            /// <summary>
            /// Draws the separators between tabs.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab header.</param>
            public virtual void DrawSeparator(Graphics g, DrawTabHeaderParams param)
            {
                using (var pen = new Pen(SeparatorColor))
                {
                    if (param.Index != Parent.SelectedIndex)
                    {
                        if (Parent.TabHeaderLocation == TabControl.TabLocation.Top || Parent.TabHeaderLocation == TabControl.TabLocation.Bottom)
                        {
                            if (param.Index != 0 && param.Index != Parent.SelectedIndex + 1)
                                g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top + 4, param.Bounds.Left, param.Bounds.Bottom - 4);
                            if (param.Index != Parent.Pages.Count - 1 && param.Index != Parent.SelectedIndex - 1)
                                g.DrawLine(pen, param.Bounds.Right, param.Bounds.Top + 4, param.Bounds.Right, param.Bounds.Bottom - 4);
                        }
                        else if (Parent.TabHeaderLocation == TabControl.TabLocation.Left)
                        {
                            if (param.Index != 0 && param.Index != Parent.SelectedIndex + 1)
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Bottom, param.Bounds.Right - 4, param.Bounds.Bottom);
                            if (param.Index != Parent.Pages.Count - 1 && param.Index != Parent.SelectedIndex - 1)
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Top, param.Bounds.Right - 4, param.Bounds.Top);
                        }
                        else // if (Parent.TabHeaderLocation == TabControl.TabLocation.Right)
                        {
                            if (param.Index != 0 && param.Index != Parent.SelectedIndex + 1)
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Top, param.Bounds.Right - 4, param.Bounds.Top);
                            if (param.Index != Parent.Pages.Count - 1 && param.Index != Parent.SelectedIndex - 1)
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Bottom, param.Bounds.Right - 4, param.Bounds.Bottom);
                        }
                    }
                }
            }

            /// <summary>
            /// Draws a border around the control.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="borderBounds">Bounding rectangle of the display area of the control.</param>
            public virtual void DrawBorder(Graphics g, Rectangle borderBounds)
            {
                int index = Parent.SelectedIndex;
                if (index == -1)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        g.DrawRectangle(pen, borderBounds);
                    }
                    return;
                }

                var tabBounds = Parent.GetTabHeaderBounds(index);

                Point[] pt = new Point[8];
                if (Parent.TabHeaderLocation == TabControl.TabLocation.Top)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = borderBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[2] = borderBounds.GetTopRight().GetOffset(-1, 0);
                    pt[3] = tabBounds.GetBottomRight().GetOffset(0, -1);
                    pt[4] = tabBounds.GetTopRight();
                    pt[5] = tabBounds.GetTopLeft();
                    pt[6] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[7] = borderBounds.GetTopLeft();
                }
                else if (Parent.TabHeaderLocation == TabControl.TabLocation.Bottom)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = tabBounds.GetTopLeft();
                    pt[2] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[3] = tabBounds.GetBottomRight().GetOffset(0, -1);
                    pt[4] = tabBounds.GetTopRight();
                    pt[5] = borderBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[6] = borderBounds.GetTopRight().GetOffset(-1, 0);
                    pt[7] = borderBounds.GetTopLeft();
                }
                else if (Parent.TabHeaderLocation == TabControl.TabLocation.Left)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = borderBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[2] = borderBounds.GetTopRight().GetOffset(-1, 0);
                    pt[3] = borderBounds.GetTopLeft();
                    pt[4] = tabBounds.GetTopRight().GetOffset(-1, 0);
                    pt[5] = tabBounds.GetTopLeft();
                    pt[6] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[7] = tabBounds.GetBottomRight().GetOffset(-1, -1);
                }
                else // if (Parent.TabHeaderLocation == TabControl.TabLocation.Right)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = borderBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[2] = tabBounds.GetBottomLeft();
                    pt[3] = tabBounds.GetBottomRight().GetOffset(-1, 0);
                    pt[4] = tabBounds.GetTopRight().GetOffset(-1, 0);
                    pt[5] = tabBounds.GetTopLeft();
                    pt[6] = borderBounds.GetTopRight().GetOffset(-1, 0);
                    pt[7] = borderBounds.GetTopLeft();
                }

                using (Pen pen = new Pen(BorderColor))
                {
                    g.DrawPolygon(pen, pt);
                }
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Returns tab backcolor for the given state.
            /// </summary>
            /// <param name="state">The state of the tab header.</param>
            protected Color GetTabBackColor(TabControl.TabHeaderState state)
            {
                if ((state & TabControl.TabHeaderState.Pressed) != TabControl.TabHeaderState.Inactive)
                    return PressedTabBackColor;
                else if ((state & TabControl.TabHeaderState.Hot) != TabControl.TabHeaderState.Inactive &&
                    (state & TabControl.TabHeaderState.Active) == TabControl.TabHeaderState.Inactive)
                    return HotTabBackColor;
                else if ((state & TabControl.TabHeaderState.Active) != TabControl.TabHeaderState.Inactive)
                    return ActiveTabBackColor;
                else
                    return InactiveTabBackColor;
            }

            /// <summary>
            /// Returns tab forecolor for the given state.
            /// </summary>
            /// <param name="state">The state of the tab header.</param>
            protected Color GetTabForeColor(TabControl.TabHeaderState state)
            {
                if ((state & TabControl.TabHeaderState.Pressed) != TabControl.TabHeaderState.Inactive)
                    return PressedTabForeColor;
                else if ((state & TabControl.TabHeaderState.Hot) != TabControl.TabHeaderState.Inactive &&
                    (state & TabControl.TabHeaderState.Active) == TabControl.TabHeaderState.Inactive)
                    return HotTabForeColor;
                else if ((state & TabControl.TabHeaderState.Active) != TabControl.TabHeaderState.Inactive)
                    return ActiveTabForeColor;
                else
                    return InactiveTabForeColor;
            }
            #endregion
        }
    }
}
