using System.Drawing;
using System.Drawing.Imaging;

namespace Manina.Windows.Forms.TabControlRenderers
{
    /// <summary>
    /// Represents the renderer of the <see cref="TabControl"/>. 
    /// This renderer has a dark color theme.
    /// </summary>
    public class NoirRenderer : TabControl.TabControlRenderer
    {
        #region Properties
        /// <summary>
        /// Gets or sets whether to use the ForeColor and BackColor properties of the control
        /// to paint tabs.
        /// </summary>
        public override bool UseTabColors => false;

        /// <summary>
        /// Gets or sets the background color of inactive tabs.
        /// </summary>
        public override Color InactiveTabBackColor => Color.FromArgb(27, 28, 31);
        /// <summary>
        /// Gets or sets the background color of active tabs.
        /// </summary>
        public override Color ActiveTabBackColor => Color.FromArgb(70, 71, 73);
        /// <summary>
        /// Gets or sets the background color of hot tabs.
        /// </summary>
        public override Color HotTabBackColor => Color.FromArgb(47, 48, 50);
        /// <summary>
        /// Gets or sets the background color of pressed tabs.
        /// </summary>
        public override Color PressedTabBackColor => Color.FromArgb(39, 40, 42);

        /// <summary>
        /// Gets or sets the background color of hot and active tabs.
        /// </summary>
        public override Color HotAndActiveTabBackColor => Color.FromArgb(88, 89, 90);

        /// <summary>
        /// Gets or sets the foreground color of inactive tabs.
        /// </summary>
        public override Color InactiveTabForeColor => Color.White;
        /// <summary>
        /// Gets or sets the foreground color of active tabs.
        /// </summary>
        public override Color ActiveTabForeColor => Color.White;
        /// <summary>
        /// Gets or sets the foreground color of hot tabs.
        /// </summary>
        public override Color HotTabForeColor => Color.White;
        /// <summary>
        /// Gets or sets the foreground color of pressed tabs.
        /// </summary>
        public override Color PressedTabForeColor => Color.White;

        /// <summary>
        /// Gets or sets the background color of inactive scroll buttons.
        /// </summary>
        public override Color InactiveButtonBackColor => Color.FromArgb(27, 28, 31);
        /// <summary>
        /// Gets or sets the background color of hot scroll buttons.
        /// </summary>
        public override Color HotButtonBackColor => Color.FromArgb(88, 89, 90);
        /// <summary>
        /// Gets or sets the background color of pressed scroll buttons.
        /// </summary>
        public override Color PressedButtonBackColor => Color.FromArgb(39, 40, 42);

        /// <summary>
        /// Gets or sets the color of selected tab marker.
        /// </summary>
        public Color MarkerColor => Color.FromArgb(131, 192, 239);
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="NoirRenderer"/> class.
        /// </summary>
        public NoirRenderer(TabControl parent) : base(parent)
        {
            ;
        }
        #endregion

        #region Render Methods
        /// <summary>
        /// Draws the background of the control.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        public override void DrawBackGround(Graphics g)
        {
            g.Clear(InactiveTabBackColor);
        }

        /// <summary>
        /// Draws the contents of a tab.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="param">The parameters required to draw the tab.</param>
        public override void DrawTabContents(Graphics g, DrawTabParams param)
        {
            base.DrawTabContents(g, param);

            // draw ribbon
            if (param.IsSelected)
            {
                using (var pen = new Pen(MarkerColor))
                {
                    if ((Parent.TabLocation & TabLocation.Top) != TabLocation.None)
                        g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Right, param.Bounds.Top);
                    else if ((Parent.TabLocation & TabLocation.Bottom) != TabLocation.None)
                        g.DrawLine(pen, param.Bounds.Left, param.Bounds.Bottom - 1, param.Bounds.Right, param.Bounds.Bottom - 1);
                    else if ((Parent.TabLocation & TabLocation.Left) != TabLocation.None)
                        g.DrawLine(pen, param.Bounds.Left, param.Bounds.Top, param.Bounds.Left, param.Bounds.Bottom);
                    else if ((Parent.TabLocation & TabLocation.Right) != TabLocation.None)
                        g.DrawLine(pen, param.Bounds.Right - 1, param.Bounds.Top, param.Bounds.Right - 1, param.Bounds.Bottom);
                }
            }
        }

        /// <summary>
        /// Draws the close button of a tab.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="param">The parameters required to draw the tab.</param>
        public override void DrawCloseButton(Graphics g, DrawTabParams param)
        {
            using (var icon = InvertBitmap(Parent.CloseTabImage))
            {
                if (Parent.TextDirection == TextDirection.Right)
                    g.DrawImage(icon, Parent.GetCloseButtonBounds(param.Tab));
                else if (Parent.TextDirection == TextDirection.Down)
                    g.DrawImageRotatedDown(icon, Parent.GetCloseButtonBounds(param.Tab));
                else
                    g.DrawImageRotatedUp(icon, Parent.GetCloseButtonBounds(param.Tab));
            }
        }

        /// <summary>
        /// Draws the separators between tabs.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="param">The parameters required to draw the tab.</param>
        public override void DrawSeparator(Graphics g, DrawTabParams param)
        {
            // do not draw separators
        }

        /// <summary>
        /// Draws a border around the control.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="borderBounds">Bounding rectangle of the display area of the control.</param>
        public override void DrawBorder(Graphics g, Rectangle borderBounds)
        {
            // no border
        }

        /// <summary>
        /// Draws the near scroll button.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="param">The parameters required to draw the button.</param>
        public override void DrawNearScrollButton(Graphics g, DrawButtonParams param)
        {
            if ((param.State & ItemState.Disabled) != ItemState.Inactive)
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }
            }
            else
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }

                using (var img = InvertBitmap(Parent.IsHorizontal ? Parent.LeftArrowImage : Parent.UpArrowImage))
                {
                    var rec = new Rectangle(Point.Empty, img.Size).GetCenteredInside(param.Bounds);
                    g.DrawImage(img, rec);
                }
            }
        }

        /// <summary>
        /// Draws the far scroll button.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="param">The parameters required to draw the button.</param>
        public override void DrawFarScrollButton(Graphics g, DrawButtonParams param)
        {
            if ((param.State & ItemState.Disabled) != ItemState.Inactive)
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }
            }
            else
            {
                var backColor = GetScrollButtonBackColor(param);
                using (var brush = new SolidBrush(backColor))
                {
                    g.FillRectangle(brush, param.Bounds);
                }

                using (var img = InvertBitmap(Parent.IsHorizontal ? Parent.RightArrowImage : Parent.DownArrowImage))
                {
                    var rec = new Rectangle(Point.Empty, img.Size).GetCenteredInside(param.Bounds);
                    g.DrawImage(img, rec);
                }
            }
        }
        #endregion

        #region Helper Methods
        private Image InvertBitmap(Image source)
        {
            var newBitmap = new Bitmap(source.Width, source.Height);

            using (var g = Graphics.FromImage(newBitmap))
            {
                var colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {-1, 0, 0, 0, 0},
                    new float[] {0, -1, 0, 0, 0},
                    new float[] {0, 0, -1, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {1, 1, 1, 0, 1}
                });

                using (var attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);

                    g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                                0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);

                    return newBitmap;
                }
            }
        }
        #endregion
    }
}
