using System.Collections;
using System.Collections.Generic;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a collection of tab headers.
        /// </summary>
        public class TabHeaderCollection : IEnumerable<TabHeader>
        {
            #region Member Variables
            private readonly Dictionary<Page, TabHeader> pageLookup = new Dictionary<Page, TabHeader>();
            private readonly List<TabHeader> items = new List<TabHeader>();
            #endregion

            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            public TabControl Control { get; private set; }

            /// <summary>
            /// Gets the header associated with a page.
            /// </summary>
            public TabHeader this[Page page] => pageLookup[page];
            /// <summary>
            /// Gets the header at the given index.
            /// </summary>
            public TabHeader this[int index] => items[index];

            /// <summary>
            /// Gets the count of items.
            /// </summary>
            public int Count => items.Count;
            #endregion

            #region Constructor
            /// <summary>
            /// Intializes a new instance of  <see cref="TabHeaderCollection"/>.
            /// </summary>
            /// <param name="parent">The parent control.</param>
            protected internal TabHeaderCollection(TabControl parent)
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
                var header = new TabHeader(Control, page);
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
            public IEnumerator<TabHeader> GetEnumerator()
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
