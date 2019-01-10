using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents a tab associated with a page.
    /// </summary>
    public class Tab : Page
    {
        #region Member Variables
        private Rectangle tabBounds;
        private Image icon = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the tab icon.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the tab icon."), DefaultValue(null)]
        public Image Icon
        {
            get => icon;
            set
            {
                icon = value;
                if (Parent != null)
                    Parent.UpdateAll();
            }
        }

        /// <summary>
        /// Gets or sets the font associated with the control.
        /// </summary>
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the font associated with the control.")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (Parent != null)
                    Parent.UpdateAll();
            }
        }

        /// <summary>
        /// Gets the owner control.
        /// </summary>
        [Browsable(false)]
        public new TabControl Parent => (TabControl)base.Parent;

        /// <summary>
        /// Gets the bounding rectangle of the tab.
        /// </summary>
        [Browsable(false)]
        public Rectangle TabBounds { get => tabBounds; internal set { tabBounds = value; UpdateContentBounds(); } }

        /// <summary>
        /// Gets the visual state of a tab.
        /// </summary>
        [Browsable(false)]
        public ItemState State
        {
            get
            {
                if (Parent == null) return ItemState.Inactive;

                ItemState state = ItemState.Inactive;

                // hot
                if (ReferenceEquals(Parent.hoveredTab, this)) state |= ItemState.Hot;
                // pressed
                if (ReferenceEquals(Parent.mouseDownTab, this)) state |= ItemState.Pressed;
                // focused
                if (Parent.Focused && (ReferenceEquals(Parent.SelectedPage, this))) state |= ItemState.Focused;

                return state;
            }
        }

        /// <summary>
        /// Gets the visual state of the close tab button.
        /// </summary>
        [Browsable(false)]
        public ItemState CloseButtonState
        {
            get
            {
                if (Parent == null) return ItemState.Inactive;

                ItemState state = ItemState.Inactive;

                // hot
                if (ReferenceEquals(Parent.hoveredTab, this) && Parent.hoveredButton) state |= ItemState.Hot;
                // pressed
                if (ReferenceEquals(Parent.mouseDownTab, this) && Parent.mouseDownButton) state |= ItemState.Pressed;
                // focused
                if (Parent.Focused && (ReferenceEquals(Parent.SelectedPage, this))) state |= ItemState.Focused;

                return state;
            }
        }

        /// <summary>
        /// Gets the bounding rectangle of tab icon in control's client coordinates.
        /// </summary>
        [Browsable(false)]
        public Rectangle IconBounds { get; private set; }
        /// <summary>
        /// Gets the bounding rectangle of tab text in control's client coordinates.
        /// </summary>
        [Browsable(false)]
        public Rectangle TextBounds { get; private set; }
        /// <summary>
        /// Gets the bounding rectangle of close tab button in control's client coordinates.
        /// </summary>
        [Browsable(false)]
        public Rectangle CloseButtonBounds { get; private set; }

        /// <summary>
        /// Determines whether the tab has an icon.
        /// </summary>
        internal bool HasIcon => Icon != null;
        /// <summary>
        /// Determines whether the tab has text.
        /// </summary>
        internal bool HasText => !string.IsNullOrEmpty(Text);
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="Tab"/>.
        /// </summary>
        public Tab()
        {

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

            if (Parent == null)
                return;

            var tabBounds = TabBounds.GetDeflated(Parent.TabPadding);
            int width = (Parent.TextDirection == TextDirection.Right ? tabBounds.Width : tabBounds.Height);
            int height = (Parent.TextDirection == TextDirection.Right ? tabBounds.Height : tabBounds.Width);

            Size iconSize = HasIcon ? Icon.Size : Size.Empty;
            Size textSize = HasText ? TextRenderer.MeasureText(Text, Font) : Size.Empty;
            Size buttonSize = Parent.ShowCloseTabButtons ? Parent.CloseTabImage.Size : Size.Empty;

            int availableIconAndTextWidth = width - (Parent.ShowCloseTabButtons ? buttonSize.Width + Parent.ContentSpacing : 0);
            int availableTextWidth = availableIconAndTextWidth - (HasIcon ? iconSize.Width + Parent.ContentSpacing : 0);

            if (HasIcon)
                IconBounds = new Rectangle(0, (height - iconSize.Height) / 2, iconSize.Width, iconSize.Height);

            if (Parent.ShowCloseTabButtons)
                CloseButtonBounds = new Rectangle(width - buttonSize.Width, (height - buttonSize.Height) / 2, buttonSize.Width, buttonSize.Height);

            if (HasText)
                TextBounds = new Rectangle(0, (height - textSize.Height) / 2,
                    Math.Min(availableTextWidth, textSize.Width), textSize.Height);

            int actualIconAndTextWidth = (HasIcon ? IconBounds.Width + Parent.ContentSpacing : 0) + TextBounds.Width;

            if (Parent.ContentAlignment == Alignment.Near)
            {
                if (HasText && HasIcon)
                    TextBounds = TextBounds.GetOffset(iconSize.Width + Parent.ContentSpacing, 0);
            }
            else if (Parent.ContentAlignment == Alignment.Center)
            {
                if (HasIcon)
                    IconBounds = IconBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2, 0);

                if (HasText)
                    TextBounds = TextBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2 + (HasIcon ? iconSize.Width + Parent.ContentSpacing : 0), 0);
            }
            else
            {
                if (HasIcon)
                    IconBounds = IconBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth, 0);

                if (HasText)
                    TextBounds = TextBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth + (HasIcon ? iconSize.Width + Parent.ContentSpacing : 0), 0);
            }

            IconBounds = IconBounds.GetRotated(tabBounds, Parent.TextDirection);
            CloseButtonBounds = CloseButtonBounds.GetRotated(tabBounds, Parent.TextDirection);
            TextBounds = TextBounds.GetRotated(tabBounds, Parent.TextDirection);

            IconBounds.Offset(Parent.TabPadding.Left, Parent.TabPadding.Top);
            CloseButtonBounds.Offset(Parent.TabPadding.Left, Parent.TabPadding.Top);
            TextBounds.Offset(Parent.TabPadding.Left, Parent.TabPadding.Top);
        }
        #endregion
    }
}
