﻿using System.Collections.Generic;
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
            #region DrawTabParams
            /// <summary>
            /// Represents the parameters required to draw a tab.
            /// </summary>
            public struct DrawTabParams
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
                public TabState State { get; private set; }
                /// <summary>
                /// Bounding rectangle of the tab.
                /// </summary>
                public Rectangle Bounds { get; private set; }

                public DrawTabParams(int index, string text, Rectangle bounds, TabState state)
                {
                    Index = index;
                    Text = text;
                    Bounds = bounds;
                    State = state;
                }
            }
            #endregion

            #region DrawTabParamsComparer
            /// <summary>
            /// Compares items so that they are sorted as: Inactive > Hot > Active > Pressed
            /// </summary>
            private class DrawTabParamsComparer : IComparer<DrawTabParams>
            {
                /// <summary>
                /// Compares tabs by draw order.
                /// </summary>
                public int Compare(DrawTabParams p1, DrawTabParams p2)
                {
                    if (ReferenceEquals(p1, p2) || p1.Index == p2.Index)
                        return 0;

                    if ((p1.State & TabState.Pressed) != TabState.Inactive)
                        return 1;
                    else if ((p2.State & TabState.Pressed) != TabState.Inactive)
                        return -1;
                    else if ((p1.State & TabState.Active) != TabState.Inactive)
                        return 1;
                    else if ((p2.State & TabState.Active) != TabState.Inactive)
                        return -1;
                    else if ((p1.State & TabState.Hot) != TabState.Inactive)
                        return 1;
                    else if ((p2.State & TabState.Hot) != TabState.Inactive)
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
            /// Gets the background color of the tab area.
            /// </summary>
            public virtual Color BackColor { get; protected set; } = SystemColors.Control;
            /// <summary>
            /// Gets the foreground color of the tab area.
            /// </summary>
            public virtual Color ForeColor { get; protected set; } = Color.Black;
            /// <summary>
            /// Gets the color of control border.
            /// </summary>
            public virtual Color BorderColor { get; protected set; } = Color.Black;

            /// <summary>
            /// Gets the font of tab texts.
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

            /// <summary>
            /// Gets the background color of scroll buttons.
            /// </summary>
            public virtual Color ButtonBackColor { get; protected set; } = SystemColors.Control;
            /// <summary>
            /// Gets the foreground color of scroll buttons.
            /// </summary>
            public virtual Color ButtonForeColor { get; protected set; } = Color.FromArgb(255, 244, 192);
            /// <summary>
            /// Gets the shape fill color of scroll buttons.
            /// </summary>
            public virtual Color ButtonFillColor { get; protected set; } = Color.FromArgb(92, 184, 92);
            /// <summary>
            /// Gets the shape border color of scroll buttons.
            /// </summary>
            public virtual Color ButtonBorderColor { get; protected set; } = Color.FromArgb(51, 51, 51);
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
            /// Draws the background of the tab area.
            /// </summary>
            public virtual void Render(Graphics g)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // clear backgound
                g.Clear(BackColor);

                // sort tabs
                var drawParams = new List<DrawTabParams>();
                for (int i = 0; i < Parent.Tabs.Count; i++)
                {
                    var tab = Parent.Tabs[i];
                    drawParams.Add(new DrawTabParams(i, tab.Text, tab.Bounds, tab.State));
                }
                drawParams.Sort(new DrawTabParamsComparer());

                // draw tabs
                foreach (var param in drawParams)
                {
                    DrawTabBackGround(g, param);
                    DrawTabText(g, param);
                    DrawSeparator(g, param);
                }

                // draw scroll buttons
                if (Parent.ScrollButtons)
                {
                    DrawNearScrollButton(g, Parent.NearScrollButtonBounds);
                    DrawFarScrollButton(g, Parent.FarScrollButtonBounds);
                }

                // draw border
                if (Parent.BorderStyle != BorderStyle.None)
                {
                    DrawBorder(g, Parent.DisplayRectangle.GetInflated(1, 1));
                }
            }

            /// <summary>
            /// Draws the backgound of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabBackGround(Graphics g, DrawTabParams param)
            {
                using (var brush = new SolidBrush(GetTabBackColor(param.State)))
                {
                    g.FillRectangle(brush, param.Bounds);
                }
            }

            /// <summary>
            /// Draws the text of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabText(Graphics g, DrawTabParams param)
            {
                if (string.IsNullOrEmpty(param.Text)) return;

                var backColor = GetTabBackColor(param.State);
                var foreColor = GetTabForeColor(param.State);

                TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
                if (Parent.TextAlignment == Alignment.Far)
                    flags |= TextFormatFlags.Right;
                else if (Parent.TextAlignment == Alignment.Center)
                    flags |= TextFormatFlags.HorizontalCenter;

                var textBounds = param.Bounds;
                textBounds.X += Parent.TabPadding.Left;
                textBounds.Y += Parent.TabPadding.Top;
                textBounds.Width -= Parent.TabPadding.Horizontal;
                textBounds.Height -= Parent.TabPadding.Vertical;

                if (Parent.TextDirection == TextDirection.Right)
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
                        if (Parent.TextDirection == TextDirection.Right)
                        {
                            ptMap[0] = param.Bounds.GetTopLeft();    // upper-left
                            ptMap[1] = param.Bounds.GetTopRight();   // upper-right
                            ptMap[2] = param.Bounds.GetBottomLeft(); // lower-left
                        }
                        else if (Parent.TextDirection == TextDirection.Up)
                        {
                            ptMap[0] = param.Bounds.GetBottomLeft();  // upper-left
                            ptMap[1] = param.Bounds.GetTopLeft();     // upper-right
                            ptMap[2] = param.Bounds.GetBottomRight(); // lower-left
                        }
                        else // if (Parent.TextDirection == TextDirection.Down)
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
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawSeparator(Graphics g, DrawTabParams param)
            {
                if (param.Index != Parent.SelectedIndex)
                {
                    using (var pen = new Pen(SeparatorColor))
                    {
                        if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None || (Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                        {
                            if (param.Index != 0 && param.Index != Parent.SelectedIndex + 1)
                                g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top + 4, param.Bounds.Left, param.Bounds.Bottom - 4);
                            if (param.Index != Parent.Pages.Count - 1 && param.Index != Parent.SelectedIndex - 1)
                                g.DrawLine(pen, param.Bounds.Right, param.Bounds.Top + 4, param.Bounds.Right, param.Bounds.Bottom - 4);
                        }
                        else
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
                        g.DrawRectangleFixed(pen, borderBounds);
                    }
                    return;
                }

                var tabBounds = Parent.Tabs[Parent.SelectedPage].Bounds;

                Point[] pt = new Point[8];
                if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None)
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
                else if ((Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
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
                else if ((Parent.TabLocation & TabLocation.Left) != TabLocation.None)
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
                else
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

            /// <summary>
            /// Draws the near scroll button.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="bounds">Button bounds.</param>
            public virtual void DrawNearScrollButton(Graphics g, Rectangle bounds)
            {
                using (Brush backBrush = new SolidBrush(ButtonBackColor))
                using (Brush brush = new SolidBrush(ButtonFillColor))
                using (Pen pen = new Pen(ButtonBorderColor))
                using (Pen textPen = new Pen(ButtonForeColor))
                {
                    g.FillRectangle(backBrush, bounds);

                    bounds = bounds.GetDeflated(Parent.TabPadding);
                    bounds = bounds.GetInflated(-4, -4);
                    Point[] points = new Point[3];

                    if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None || (Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                    {
                        points[0] = new Point(bounds.Right, bounds.Top);
                        points[1] = new Point(bounds.Left, (bounds.Top + bounds.Bottom) / 2);
                        points[2] = new Point(bounds.Right, bounds.Bottom);
                    }
                    else
                    {
                        points[0] = new Point(bounds.Left, bounds.Bottom);
                        points[1] = new Point((bounds.Left + bounds.Right) / 2, bounds.Top);
                        points[2] = new Point(bounds.Right, bounds.Bottom);
                    }

                    g.FillPolygon(brush, points);
                    g.DrawPolygon(pen, points);
                }
            }

            /// <summary>
            /// Draws the far scroll button.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="bounds">Button bounds.</param>
            public virtual void DrawFarScrollButton(Graphics g, Rectangle bounds)
            {
                using (Brush backBrush = new SolidBrush(ButtonBackColor))
                using (Brush brush = new SolidBrush(ButtonFillColor))
                using (Pen pen = new Pen(ButtonBorderColor))
                using (Pen textPen = new Pen(ButtonForeColor))
                {
                    g.FillRectangle(backBrush, bounds);

                    bounds = bounds.GetDeflated(Parent.TabPadding);
                    bounds = bounds.GetInflated(-4, -4);
                    Point[] points = new Point[3];

                    if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None || (Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                    {
                        points[0] = new Point(bounds.Left, bounds.Top);
                        points[1] = new Point(bounds.Right, (bounds.Top + bounds.Bottom) / 2);
                        points[2] = new Point(bounds.Left, bounds.Bottom);
                    }
                    else
                    {
                        points[0] = new Point(bounds.Left, bounds.Top);
                        points[1] = new Point((bounds.Left + bounds.Right) / 2, bounds.Bottom);
                        points[2] = new Point(bounds.Right, bounds.Top);
                    }

                    g.FillPolygon(brush, points);
                    g.DrawPolygon(pen, points);
                }
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Returns tab backcolor for the given state.
            /// </summary>
            /// <param name="state">The state of the tab.</param>
            protected Color GetTabBackColor(TabState state)
            {
                if ((state & TabState.Pressed) != TabState.Inactive)
                    return PressedTabBackColor;
                else if ((state & TabState.Hot) != TabState.Inactive &&
                    (state & TabState.Active) == TabState.Inactive)
                    return HotTabBackColor;
                else if ((state & TabState.Active) != TabState.Inactive)
                    return ActiveTabBackColor;
                else
                    return InactiveTabBackColor;
            }

            /// <summary>
            /// Returns tab forecolor for the given state.
            /// </summary>
            /// <param name="state">The state of the tab.</param>
            protected Color GetTabForeColor(TabState state)
            {
                if ((state & TabState.Pressed) != TabState.Inactive)
                    return PressedTabForeColor;
                else if ((state & TabState.Hot) != TabState.Inactive &&
                    (state & TabState.Active) == TabState.Inactive)
                    return HotTabForeColor;
                else if ((state & TabState.Active) != TabState.Inactive)
                    return ActiveTabForeColor;
                else
                    return InactiveTabForeColor;
            }
            #endregion
        }
    }
}