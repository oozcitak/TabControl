using System;
using System.ComponentModel;
using System.Drawing;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents a tab associated with a page.
    /// </summary>
    public class Tab : Page
    {
        #region Virtual Functions for Events
        protected internal virtual void OnContentsChanged(EventArgs e) { ContentsChanged?.Invoke(this, e); }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the contents of a tab are changed.
        /// </summary>
        [Category("Property Changed"), Description("Occurs when the contents of a tab are changed.")]
        public event EventHandler ContentsChanged;
        #endregion

        #region Member Variables
        private Image icon = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the tab icon.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the tab icon."), DefaultValue(null)]
        public Image Icon { get => icon; set { icon = value; OnContentsChanged(EventArgs.Empty); } }

        /// <summary>
        /// Gets or sets the font associated with the control.
        /// </summary>
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the font associated with the control.")]
        public override Font Font { get => base.Font; set { base.Font = value; OnContentsChanged(EventArgs.Empty); } }

        /// <summary>
        /// Gets the bounding rectangle of the tab in control's client coordinates 
        /// but without the view offset applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle TabBounds { get; set; }

        /// <summary>
        /// Gets the bounding rectangle of tab icon in control's client coordinates
        /// but without the view offset applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle IconBounds { get; set; }
        /// <summary>
        /// Gets the bounding rectangle of tab text in control's client coordinates
        /// but without the view offset applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle TextBounds { get; set; }
        /// <summary>
        /// Gets the bounding rectangle of close tab button in control's client coordinates
        /// but without the view offset applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle CloseButtonBounds { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="Tab"/>.
        /// </summary>
        public Tab()
        {

        }
        #endregion

        #region Overridden Functions
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            OnContentsChanged(EventArgs.Empty);
        }
        #endregion
    }
}
