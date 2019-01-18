using System.ComponentModel;
using System.Drawing;
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
                /// The tab to be drawn.
                /// </summary>
                public Tab Tab { get; private set; }
                /// <summary>
                /// Whether the tab is the selected tab of the control.
                /// </summary>
                public bool IsSelected { get; private set; }
                /// <summary>
                /// Visual state of the tab.
                /// </summary>
                public ItemState State { get; private set; }
                /// <summary>
                /// Bounding rectangle of the tab.
                /// </summary>
                public Rectangle Bounds { get; private set; }

                public DrawTabParams(int index, Tab tab, bool isSelected, ItemState state, Rectangle bounds)
                {
                    Index = index;
                    Tab = tab;
                    IsSelected = isSelected;
                    State = state;
                    Bounds = bounds;
                }
            }
            #endregion

            #region DrawButtonParams
            /// <summary>
            /// Represents the parameters required to draw a scroll button.
            /// </summary>
            public struct DrawButtonParams
            {
                /// <summary>
                /// Visual state of the button.
                /// </summary>
                public ItemState State { get; private set; }
                /// <summary>
                /// Bounding rectangle of the button.
                /// </summary>
                public Rectangle Bounds { get; private set; }

                public DrawButtonParams(ItemState state, Rectangle bounds)
                {
                    State = state;
                    Bounds = bounds;
                }
            }
            #endregion

            #region Properties
            /// <summary>
            /// Gets the parent control.
            /// </summary>
            [Browsable(false)]
            public TabControl Parent { get; protected set; }

            /// <summary>
            /// Gets or sets whether to use the ForeColor and BackColor properties of the control
            /// to paint tabs.
            /// </summary>
            public virtual bool UseTabColors { get; set; } = true;

            /// <summary>
            /// Gets or sets the color of control border.
            /// </summary>
            public virtual Color BorderColor { get; set; } = Color.Black;

            /// <summary>
            /// Gets or sets the background color of inactive tabs.
            /// </summary>
            public virtual Color InactiveTabBackColor { get; set; } = Color.FromArgb(225, 225, 225);
            /// <summary>
            /// Gets or sets the background color of active tabs.
            /// </summary>
            public virtual Color ActiveTabBackColor { get; set; } = Color.White;
            /// <summary>
            /// Gets or sets the background color of hot tabs.
            /// </summary>
            public virtual Color HotTabBackColor { get; set; } = Color.FromArgb(235, 235, 235);
            /// <summary>
            /// Gets or sets the background color of pressed tabs.
            /// </summary>
            public virtual Color PressedTabBackColor { get; set; } = Color.White;
            /// <summary>
            /// Gets or sets the background color of hot and active tabs.
            /// </summary>
            public virtual Color HotAndActiveTabBackColor { get; set; } = Color.White;

            /// <summary>
            /// Gets or sets the foreground color of inactive tabs.
            /// </summary>
            public virtual Color InactiveTabForeColor { get; set; } = Color.Black;
            /// <summary>
            /// Gets or sets the foreground color of active tabs.
            /// </summary>
            public virtual Color ActiveTabForeColor { get; set; } = Color.Black;
            /// <summary>
            /// Gets or sets the foreground color of hot tabs.
            /// </summary>
            public virtual Color HotTabForeColor { get; set; } = Color.Black;
            /// <summary>
            /// Gets or sets the foreground color of pressed tabs.
            /// </summary>
            public virtual Color PressedTabForeColor { get; set; } = Color.Black;

            /// <summary>
            /// Gets or sets the color of tab separators.
            /// </summary>
            public virtual Color SeparatorColor { get; set; } = Color.FromArgb(166, 166, 166);

            /// <summary>
            /// Gets or sets the background color of inactive scroll buttons.
            /// </summary>
            public virtual Color InactiveButtonBackColor { get; set; } = Color.FromArgb(225, 225, 225);
            /// <summary>
            /// Gets or sets the background color of hot scroll buttons.
            /// </summary>
            public virtual Color HotButtonBackColor { get; set; } = Color.FromArgb(235, 235, 235);
            /// <summary>
            /// Gets or sets the background color of pressed scroll buttons.
            /// </summary>
            public virtual Color PressedButtonBackColor { get; set; } = Color.White;
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="TabControlRenderer"/> class.
            /// </summary>
            public TabControlRenderer(TabControl parent)
            {
                Parent = parent;
            }
            #endregion

            #region Render Methods
            /// <summary>
            /// Draws the control.
            /// </summary>
            public virtual void Render(Graphics g)
            {
                // draw backgound
                DrawBackGround(g);

                // draw tabs
                int i = 0;
                foreach (var tab in Parent.Tabs)
                {
                    var param = new DrawTabParams(i, tab, ReferenceEquals(tab, Parent.SelectedTab), Parent.GetTabState(tab), Parent.GetTabBounds(tab));
                    DrawTabBackGround(g, param);
                    DrawTabContents(g, param);
                    DrawSeparator(g, param);
                    i++;
                }

                // draw border
                if (Parent.BorderStyle != BorderStyle.None)
                {
                    DrawBorder(g, Parent.DisplayRectangle.GetInflated(1, 1));
                }

                // draw scroll buttons
                if (Parent.ScrollButtons)
                {
                    DrawNearScrollButton(g, new DrawButtonParams(Parent.GetNearScrollButtonState(), Parent.NearScrollButtonBounds));
                    DrawFarScrollButton(g, new DrawButtonParams(Parent.GetFarScrollButtonState(), Parent.FarScrollButtonBounds));
                }
            }

            /// <summary>
            /// Draws the background of the control.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            public virtual void DrawBackGround(Graphics g)
            {
                g.Clear(Parent.BackColor);
            }

            /// <summary>
            /// Draws the background of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabBackGround(Graphics g, DrawTabParams param)
            {
                using (var brush = new SolidBrush(GetTabBackColor(param)))
                {
                    g.FillRectangle(brush, param.Bounds);
                }
            }

            /// <summary>
            /// Draws the contents of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabContents(Graphics g, DrawTabParams param)
            {
                // text
                if (!string.IsNullOrEmpty(param.Tab.Text))
                    DrawTabText(g, param);

                // icon
                if (param.Tab.Icon != null)
                    DrawTabIcon(g, param);

                // close button
                if (Parent.ShowCloseTabButtons && param.IsSelected)
                    DrawCloseButton(g, param);
            }

            /// <summary>
            /// Draws the icon of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabIcon(Graphics g, DrawTabParams param)
            {
                if (Parent.TextDirection == TextDirection.Right)
                    g.DrawImage(param.Tab.Icon, Parent.GetIconBounds(param.Tab));
                else if (Parent.TextDirection == TextDirection.Down)
                    g.DrawImageRotatedDown(param.Tab.Icon, Parent.GetIconBounds(param.Tab));
                else
                    g.DrawImageRotatedUp(param.Tab.Icon, Parent.GetIconBounds(param.Tab));
            }

            /// <summary>
            /// Draws the text of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawTabText(Graphics g, DrawTabParams param)
            {
                var backColor = GetTabBackColor(param);
                var foreColor = GetTabForeColor(param);

                TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;

                if (Parent.TextDirection == TextDirection.Right)
                    TextRenderer.DrawText(g, param.Tab.Text, param.Tab.Font, Parent.GetTextBounds(param.Tab), foreColor, backColor, flags);
                else if (Parent.TextDirection == TextDirection.Down)
                    g.DrawVerticalTextDown(param.Tab.Text, param.Tab.Font, Parent.GetTextBounds(param.Tab), foreColor, backColor, flags);
                else
                    g.DrawVerticalTextUp(param.Tab.Text, param.Tab.Font, Parent.GetTextBounds(param.Tab), foreColor, backColor, flags);
            }

            /// <summary>
            /// Draws the close button of a tab.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawCloseButton(Graphics g, DrawTabParams param)
            {
                Color buttonBackColor = GetCloseTabButtonBackColor(param);
                using (var buttonBrush = new SolidBrush(buttonBackColor))
                    g.FillRectangle(buttonBrush, Parent.GetCloseButtonBounds(param.Tab));

                if (Parent.TextDirection == TextDirection.Right)
                    g.DrawImage(Parent.CloseTabImage, Parent.GetCloseButtonBounds(param.Tab));
                else if (Parent.TextDirection == TextDirection.Down)
                    g.DrawImageRotatedDown(Parent.CloseTabImage, Parent.GetCloseButtonBounds(param.Tab));
                else
                    g.DrawImageRotatedUp(Parent.CloseTabImage, Parent.GetCloseButtonBounds(param.Tab));
            }

            /// <summary>
            /// Draws the separators between tabs.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the tab.</param>
            public virtual void DrawSeparator(Graphics g, DrawTabParams param)
            {
                if (!param.IsSelected)
                {
                    using (var pen = new Pen(SeparatorColor))
                    {
                        if (Parent.IsHorizontal)
                        {
                            if (param.Index != Parent.SelectedIndex + 1 && (param.Index != 0 || (param.Index == 0 && Parent.ScrollButtons)))
                                g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top + 4, param.Bounds.Left, param.Bounds.Bottom - 4);
                            if (param.Index == Parent.Tabs.Count - 1 && Parent.ScrollButtons)
                                g.DrawLine(pen, param.Bounds.Right - 1, param.Bounds.Top + 4, param.Bounds.Right - 1, param.Bounds.Bottom - 4);
                        }
                        else
                        {
                            if (param.Index != Parent.SelectedIndex + 1 && (param.Index != 0 || (param.Index == 0 && Parent.ScrollButtons)))
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Top, param.Bounds.Right - 4, param.Bounds.Top);
                            if (param.Index == Parent.Tabs.Count - 1 && Parent.ScrollButtons)
                                g.DrawLine(pen, param.Bounds.Left + 4, param.Bounds.Bottom - 1, param.Bounds.Right - 4, param.Bounds.Bottom - 1);
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
                if (Parent.SelectedIndex == -1)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        g.DrawRectangleFixed(pen, borderBounds);
                    }
                    return;
                }

                var tabBounds = Parent.GetTabBounds(Parent.SelectedTab);

                Point[] pt = new Point[8];
                if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = borderBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[2] = borderBounds.GetTopRight().GetOffset(-1, 0);
                    pt[3] = tabBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[4] = tabBounds.GetTopRight().GetOffset(-1, 0);
                    pt[5] = tabBounds.GetTopLeft();
                    pt[6] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[7] = borderBounds.GetTopLeft();
                }
                else if ((Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                {
                    pt[0] = borderBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[1] = tabBounds.GetTopLeft();
                    pt[2] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[3] = tabBounds.GetBottomRight().GetOffset(-1, -1);
                    pt[4] = tabBounds.GetTopRight().GetOffset(-1, 0);
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
                    pt[2] = tabBounds.GetBottomLeft().GetOffset(0, -1);
                    pt[3] = tabBounds.GetBottomRight().GetOffset(-1, -1);
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
            /// <param name="param">The parameters required to draw the button.</param>
            public virtual void DrawNearScrollButton(Graphics g, DrawButtonParams param)
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }

                Image img = Parent.IsHorizontal ? Parent.LeftArrowImage : Parent.UpArrowImage;
                var rec = new Rectangle(Point.Empty, img.Size).GetCenteredInside(param.Bounds);
                if ((param.State & ItemState.Disabled) != ItemState.Inactive)
                    ControlPaint.DrawImageDisabled(g, img, rec.X, rec.Y, backColor);
                else
                    g.DrawImage(img, rec);

                if (Parent.BorderStyle != BorderStyle.None)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Bottom - 1, param.Bounds.Right - 1, param.Bounds.Bottom - 1);
                        else if ((Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Right - 1, param.Bounds.Top);
                        else if ((Parent.TabLocation & TabLocation.Left) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Right - 1, param.Bounds.Top, param.Bounds.Right - 1, param.Bounds.Bottom - 1);
                        else
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Left, param.Bounds.Bottom - 1);
                    }
                }
            }

            /// <summary>
            /// Draws the far scroll button.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="param">The parameters required to draw the button.</param>
            public virtual void DrawFarScrollButton(Graphics g, DrawButtonParams param)
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }

                Image img = Parent.IsHorizontal ? Parent.RightArrowImage : Parent.DownArrowImage;
                var rec = new Rectangle(Point.Empty, img.Size).GetCenteredInside(param.Bounds);
                if ((param.State & ItemState.Disabled) != ItemState.Inactive)
                    ControlPaint.DrawImageDisabled(g, img, rec.X, rec.Y, backColor);
                else
                    g.DrawImage(img, rec);

                if (Parent.BorderStyle != BorderStyle.None)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Bottom - 1, param.Bounds.Right - 1, param.Bounds.Bottom - 1);
                        else if ((Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Right - 1, param.Bounds.Top);
                        else if ((Parent.TabLocation & TabLocation.Left) != TabLocation.None)
                            g.DrawLine(pen, param.Bounds.Right - 1, param.Bounds.Top, param.Bounds.Right - 1, param.Bounds.Bottom - 1);
                        else
                            g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Left, param.Bounds.Bottom - 1);
                    }
                }
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Returns tab backcolor for the given state.
            /// </summary>
            /// <param name="param">The state of the tab.</param>
            protected Color GetTabBackColor(DrawTabParams param)
            {
                if ((param.State & ItemState.Pressed) != ItemState.Inactive)
                    return UseTabColors ? param.Tab.BackColor.Lighten(0.1f) : PressedTabBackColor;
                else if ((param.State & ItemState.Hot) != ItemState.Inactive && param.IsSelected)
                    return UseTabColors ? param.Tab.BackColor.Darken(0.05f) : HotAndActiveTabBackColor;
                else if ((param.State & ItemState.Hot) != ItemState.Inactive && !param.IsSelected)
                    return UseTabColors ? param.Tab.BackColor.Darken(0.05f) : HotTabBackColor;
                else if (param.IsSelected)
                    return UseTabColors ? param.Tab.BackColor : ActiveTabBackColor;
                else
                    return UseTabColors ? param.Tab.BackColor.Darken(0.1f) : InactiveTabBackColor;
            }

            /// <summary>
            /// Returns tab forecolor for the given state.
            /// </summary>
            /// <param name="param">The state of the tab.</param>
            protected Color GetTabForeColor(DrawTabParams param)
            {
                if (UseTabColors) return param.Tab.ForeColor;

                if ((param.State & ItemState.Pressed) != ItemState.Inactive)
                    return PressedTabForeColor;
                else if ((param.State & ItemState.Hot) != ItemState.Inactive && !param.IsSelected)
                    return HotTabForeColor;
                else if (param.IsSelected)
                    return ActiveTabForeColor;
                else
                    return InactiveTabForeColor;
            }

            /// <summary>
            /// Returns scroll button backcolor for the given state.
            /// </summary>
            /// <param name="param">The state of the button.</param>
            protected Color GetScrollButtonBackColor(DrawButtonParams param)
            {
                if ((param.State & ItemState.Disabled) != ItemState.Inactive)
                    return InactiveButtonBackColor;
                else if ((param.State & ItemState.Pressed) != ItemState.Inactive)
                    return PressedButtonBackColor;
                else if ((param.State & ItemState.Hot) != ItemState.Inactive)
                    return HotButtonBackColor;
                else
                    return InactiveButtonBackColor;
            }

            /// <summary>
            /// Returns close tab button backcolor for the given state.
            /// </summary>
            /// <param name="param">The state of the tab.</param>
            protected Color GetCloseTabButtonBackColor(DrawTabParams param)
            {
                var state = Parent.GetTabCloseButtonState(param.Tab);
                if ((state & ItemState.Hot) != ItemState.Inactive)
                    return HotTabBackColor;
                else
                    return GetTabBackColor(param);
            }
            #endregion
        }
    }
}
