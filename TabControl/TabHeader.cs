using System.Drawing;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a tab header associated wth a page.
        /// </summary>
        public class TabHeader
        {
            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            public TabControl Control { get; private set; }

            /// <summary>
            /// Gets the <see cref="Page"/> associated with this header.
            /// </summary>
            public Page Page { get; private set; }

            /// <summary>
            /// Gets the header text.
            /// </summary>
            public string Text => Page != null ? Page.Text : "";

            /// <summary>
            /// Gets the bounding rectangle of the tab header.
            /// </summary>
            public Rectangle Bounds { get; internal set; }

            /// <summary>
            /// Gets the visual state of a tab header.
            /// </summary>
            public TabHeaderState State
            {
                get
                {
                    if (Control == null) return TabHeaderState.Inactive;

                    TabHeaderState state = TabHeaderState.Inactive;

                    // active
                    if (ReferenceEquals(Control.SelectedPage, Page)) state |= TabHeaderState.Active;
                    // hot
                    if (ReferenceEquals(Control.hoveredTabHeader, this)) state |= TabHeaderState.Hot;
                    // pressed
                    if (ReferenceEquals(Control.mouseDownTabHeader, this)) state |= TabHeaderState.Pressed;
                    // focused
                    if (Control.Focused && (ReferenceEquals(Control.SelectedPage, Page))) state |= TabHeaderState.Focused;

                    return state;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of <see cref="TabHeader"/>.
            /// </summary>
            /// <param name="page">The associated page.</param>
            protected internal TabHeader(TabControl parent, Page page)
            {
                Control = parent;
                Page = page;
            }
            #endregion
        }
    }
}
