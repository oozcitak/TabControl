using System.Drawing;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a tab associated with a page.
        /// </summary>
        public class Tab
        {
            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            protected TabControl Control { get; private set; }

            /// <summary>
            /// Gets the <see cref="Page"/> associated with this tab.
            /// </summary>
            public Page Page { get; private set; }

            /// <summary>
            /// Gets the tab text.
            /// </summary>
            public string Text => Page != null ? Page.Text : "";

            /// <summary>
            /// Gets the bounding rectangle of the tab.
            /// </summary>
            public Rectangle Bounds { get; internal set; }

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
        }
    }
}
