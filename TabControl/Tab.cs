using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a tab associated with a page.
        /// </summary>
        public class Tab
        {
            #region Member Variables
            private Rectangle bounds;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            protected internal TabControl Control { get; private set; }

            /// <summary>
            /// Gets the <see cref="Page"/> associated with this tab.
            /// </summary>
            public Page Page { get; private set; }

            /// <summary>
            /// Gets or sets the tab text.
            /// </summary>
            public string Text
            {
                get
                {
                    return Page != null ? Page.Text : "";
                }
                set
                {
                    if (Page != null) Page.Text = value;
                }
            }

            /// <summary>
            /// Gets or sets the tab icon.
            /// </summary>
            public Image Icon { get; set; } = null;

            /// <summary>
            /// Gets the bounding rectangle of the tab.
            /// </summary>
            public Rectangle Bounds { get => bounds; internal set { bounds = value; UpdateContentBounds(); } }

            /// <summary>
            /// Gets the visual state of a tab.
            /// </summary>
            public TabState State
            {
                get
                {
                    if (Control == null) return TabState.Inactive;

                    TabState state = TabState.Inactive;

                    // active
                    if (ReferenceEquals(Control.SelectedPage, Page)) state |= TabState.Active;
                    // hot
                    if (ReferenceEquals(Control.hoveredTab, this)) state |= TabState.Hot;
                    // pressed
                    if (ReferenceEquals(Control.mouseDownTab, this)) state |= TabState.Pressed;
                    // focused
                    if (Control.Focused && (ReferenceEquals(Control.SelectedPage, Page))) state |= TabState.Focused;

                    return state;
                }
            }

            /// <summary>
            /// Gets the bounding rectangle of tab icon in control's client coordinates.
            /// </summary>
            public Rectangle IconBounds { get; private set; }
            /// <summary>
            /// Gets the bounding rectangle of tab text in control's client coordinates.
            /// </summary>
            public Rectangle TextBounds { get; private set; }
            /// <summary>
            /// Gets the bounding rectangle of close tab button in control's client coordinates.
            /// </summary>
            public Rectangle CloseButtonBounds { get; private set; }

            internal bool HasIcon => Icon != null;
            internal bool HasText => !string.IsNullOrEmpty(Text);
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of <see cref="Tab"/>.
            /// </summary>
            /// <param name="page">The associated page.</param>
            protected internal Tab(TabControl parent, Page page)
            {
                Control = parent;
                Page = page;
            }
            #endregion

            #region Instance Methods
            /// <summary>
            /// Updates the bounding rectangles of tab contents.
            /// </summary>
            internal void UpdateContentBounds()
            {
                IconBounds = Rectangle.Empty;
                CloseButtonBounds = Rectangle.Empty;
                TextBounds = Rectangle.Empty;

                var tabBounds = Bounds.GetDeflated(Control.TabPadding);
                int width = (Control.TextDirection == TextDirection.Right ? tabBounds.Width : tabBounds.Height);
                int height = (Control.TextDirection == TextDirection.Right ? tabBounds.Height : tabBounds.Width);

                Size iconSize = HasIcon ? Icon.Size : Size.Empty;
                Size textSize = HasText ? TextRenderer.MeasureText(Text, Control.Font) : Size.Empty;
                Size buttonSize = Control.ShowCloseTabButtons ? Control.CloseTabImage.Size : Size.Empty;

                int availableIconAndTextWidth = width - (Control.ShowCloseTabButtons ? buttonSize.Width + Control.ContentSpacing : 0);
                int availableTextWidth = availableIconAndTextWidth - (HasIcon ? iconSize.Width + Control.ContentSpacing : 0);

                if (HasIcon)
                    IconBounds = new Rectangle(0, (height - iconSize.Height) / 2, iconSize.Width, iconSize.Height);

                if (Control.ShowCloseTabButtons)
                    CloseButtonBounds = new Rectangle(width - buttonSize.Width, (height - buttonSize.Height) / 2, buttonSize.Width, buttonSize.Height);

                if (HasText)
                    TextBounds = new Rectangle(0, (height - textSize.Height) / 2,
                        Math.Min(availableTextWidth, textSize.Width), textSize.Height);

                int actualIconAndTextWidth = (HasIcon ? IconBounds.Width + Control.ContentSpacing : 0) + TextBounds.Width;

                if (Control.ContentAlignment == Alignment.Near)
                {
                    if (HasText && HasIcon)
                        TextBounds = TextBounds.GetOffset(iconSize.Width + Control.ContentSpacing, 0);
                }
                else if (Control.ContentAlignment == Alignment.Center)
                {
                    if (HasIcon)
                        IconBounds = IconBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2, 0);

                    if (HasText)
                        TextBounds = TextBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2 + (HasIcon ? iconSize.Width + Control.ContentSpacing : 0), 0);
                }
                else
                {
                    if (HasIcon)
                        IconBounds = IconBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth, 0);

                    if (HasText)
                        TextBounds = TextBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth + (HasIcon ? iconSize.Width + Control.ContentSpacing : 0), 0);
                }

                IconBounds = IconBounds.GetRotated(tabBounds, Control.TextDirection);
                CloseButtonBounds = CloseButtonBounds.GetRotated(tabBounds, Control.TextDirection);
                TextBounds = TextBounds.GetRotated(tabBounds, Control.TextDirection);

                IconBounds.Offset(Control.TabPadding.Left, Control.TabPadding.Top);
                CloseButtonBounds.Offset(Control.TabPadding.Left, Control.TabPadding.Top);
                TextBounds.Offset(Control.TabPadding.Left, Control.TabPadding.Top);
            }
            #endregion
        }
    }
}
