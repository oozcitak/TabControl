using System;
using System.Collections;
using System.Collections.Generic;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Represents a collection of tabs.
        /// </summary>
        public class TabCollection : IList<Tab>, IList
        {
            #region Properties
            /// <summary>
            /// Gets the owner control.
            /// </summary>
            protected TabControl Control { get; private set; }

            /// <summary>
            /// Gets the page collection of owner control.
            /// </summary>
            protected PageCollection Pages { get; private set; }

            /// <summary>
            /// Gets the tab at the given index.
            /// </summary>
            public Tab this[int index] { get => (Tab)Pages[index]; set => Pages[index] = value; }

            /// <summary>
            /// Gets the count of items.
            /// </summary>
            public int Count => Pages.Count;

            public bool IsReadOnly => false;

            bool IList.IsFixedSize => false;

            object ICollection.SyncRoot => throw new NotImplementedException();

            bool ICollection.IsSynchronized => false;

            object IList.this[int index] { get => this[index]; set => this[index] = (Tab)value; }
            #endregion

            #region Constructor
            /// <summary>
            /// Intializes a new instance of <see cref="TabCollection"/>.
            /// </summary>
            /// <param name="parent">The parent control.</param>
            protected internal TabCollection(TabControl parent)
            {
                Control = parent;
                Pages = parent.Pages;
            }
            #endregion

            #region IEnumerable
            public IEnumerator<Tab> GetEnumerator()
            {
                foreach (var page in Pages)
                    yield return (Tab)page;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Pages.GetEnumerator();
            }
            #endregion

            #region IList<>
            public void Add(Tab item) => Pages.Add(item);
            public void Clear() => Pages.Clear();
            public bool Contains(Tab item) => Pages.Contains(item);
            public void CopyTo(Tab[] array, int arrayIndex) => Pages.CopyTo(array, arrayIndex);
            public int IndexOf(Tab item) => Pages.IndexOf(item);
            public void Insert(int index, Tab item) => Pages.Insert(index, item);
            public bool Remove(Tab item) => Pages.Remove(item);
            public void RemoveAt(int index) => Pages.RemoveAt(index);
            #endregion

            #region IList
            int IList.Add(object value)
            {
                Add((Tab)value);
                return Count - 1;
            }

            bool IList.Contains(object value) => Contains((Tab)value);
            int IList.IndexOf(object value) => IndexOf((Tab)value);
            void IList.Insert(int index, object value) => Insert(index, (Tab)value);
            void IList.Remove(object value) => Remove((Tab)value);
            void ICollection.CopyTo(Array array, int arrayIndex) => CopyTo((Tab[])array, arrayIndex);
            #endregion
        }
    }
}
