using System.Collections;
using System.Collections.Generic;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a collection of tabs.
        /// </summary>
        public class TabCollection : IEnumerable<Tab>
        {
            #region Member Variables
            private readonly Dictionary<Page, Tab> pageLookup = new Dictionary<Page, Tab>();
            private readonly List<Tab> items = new List<Tab>();
            #endregion

            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            protected TabControl Control { get; private set; }

            /// <summary>
            /// Gets the header associated with a page.
            /// </summary>
            public Tab this[Page page] => pageLookup[page];
            /// <summary>
            /// Gets the header at the given index.
            /// </summary>
            public Tab this[int index] => items[index];

            /// <summary>
            /// Gets the count of items.
            /// </summary>
            public int Count => items.Count;
            #endregion

            #region Constructor
            /// <summary>
            /// Intializes a new instance of <see cref="TabCollection"/>.
            /// </summary>
            /// <param name="parent">The parent control.</param>
            protected internal TabCollection(TabControl parent)
            {
                Control = parent;
            }
            #endregion

            #region Page Handlers
            /// <summary>
            /// Adds a tab header whenever a new page is added to the owner control.
            /// </summary>
            internal void AddPage(Page page)
            {
                var header = new Tab(Control, page);
                pageLookup.Add(page, header);
                items.Add(header);
            }

            /// <summary>
            /// Removes the associated tab header whenever a page is removed from the owner control.
            /// </summary>
            internal void RemovePage(Page page)
            {
                var header = pageLookup[page];
                pageLookup.Remove(page);
                items.Remove(header);
            }
            #endregion

            #region IEnumerable
            public IEnumerator<Tab> GetEnumerator()
            {
                return items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return items.GetEnumerator();
            }
            #endregion
        }
    }
}
