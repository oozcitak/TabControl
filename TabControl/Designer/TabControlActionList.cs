using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Defines smart tag entries for the control.
        /// </summary>
        internal class TabControlActionList : DesignerActionList
        {
            #region Properties
            /// <summary>
            /// Gets the associated <see cref="TabControl"/>.
            /// </summary>
            public TabControl Control { get; private set; }

            /// <summary>
            /// Gets the designer host.
            /// </summary>
            public IDesignerHost DesignerHost { get; private set; }

            /// <summary>
            /// Gets the selection service.
            /// </summary>
            public ISelectionService SelectionService { get; private set; }
            #endregion

            #region Actions
            /// <summary>
            /// Gets or sets the TabHeaderLocation of the designed control.
            /// </summary>
            public TabLocation TabHeaderLocation
            {
                get { return Control.TabHeaderLocation; }
                set { GetPropertyByName("TabHeaderLocation").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the TabHeaderAlignment of the designed control.
            /// </summary>
            public Alignment TabHeaderAlignment
            {
                get { return Control.TabHeaderAlignment; }
                set { GetPropertyByName("TabHeaderAlignment").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the TabHeaderSizing of the designed control.
            /// </summary>
            public TabSizing TabHeaderSizing
            {
                get { return Control.TabHeaderSizing; }
                set { GetPropertyByName("TabHeaderSizing").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the TextAlignment of the designed control.
            /// </summary>
            public Alignment TextAlignment
            {
                get { return Control.TextAlignment; }
                set { GetPropertyByName("TextAlignment").SetValue(Control, value); }
            }

            /// <summary>
            /// Adds a page to the designed control.
            /// </summary>
            public void AddPage()
            {
                if (DesignerHost != null)
                {
                    Page page = (Page)DesignerHost.CreateComponent(typeof(Page));
                    page.Text = string.Format("Page {0}", Control.Pages.Count + 1);
                    Control.Pages.Add(page);
                    Control.SelectedPage = page;

                    if (SelectionService != null)
                        SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
                }
            }

            /// <summary>
            /// Removes the current page of the designed control.
            /// </summary>
            protected void RemovePage()
            {
                if (DesignerHost != null)
                {
                    if (Control.Pages.Count > 1)
                    {
                        Page page = Control.SelectedPage;
                        if (page != null)
                        {
                            int index = Control.SelectedIndex;

                            DesignerHost.DestroyComponent(page);
                            if (index == Control.Pages.Count)
                                index = Control.Pages.Count - 1;
                            Control.SelectedIndex = index;

                            if (SelectionService != null)
                                SelectionService.SetSelectedComponents(new Component[] { Control.SelectedPage });
                        }
                    }
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="TabControlActionList"/> class.
            /// </summary>
            /// <param name="component">A component related to the DesignerActionList.</param>
            public TabControlActionList(IComponent component) : base(component)
            {
                Control = (TabControl)component;

                DesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
                SelectionService = (ISelectionService)GetService(typeof(ISelectionService));
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Helper method to retrieve control properties for undo support.
            /// </summary>
            /// <param name="propName">Property name.</param>
            private PropertyDescriptor GetPropertyByName(string propName)
            {
                PropertyDescriptor prop;
                prop = TypeDescriptor.GetProperties(Control)[propName];
                if (prop == null)
                    throw new ArgumentException("Unknown property.", propName);
                else
                    return prop;
            }
            #endregion

            #region Overrides
            // <summary>
            /// Returns the collection of <see cref="T:System.ComponentModel.Design.DesignerActionItem"/> objects contained in the list.
            /// </summary>
            public override DesignerActionItemCollection GetSortedActionItems()
            {
                return new DesignerActionItemCollection() {

                    new DesignerActionHeaderItem("Appearance"),
                    new DesignerActionHeaderItem("Data"),

                    new DesignerActionPropertyItem("TabHeaderLocation", "Tab Location", "Appearance", "Set the location of tabs  relative to the control"),
                    new DesignerActionPropertyItem("TabHeaderAlignment", "Tab Alignment", "Appearance", "Set the alignment of tabs relative to the control"),
                    new DesignerActionPropertyItem("TabHeaderSizing", "Tab Sizing", "Appearance", "Set the sizing mode of tabs"),
                    new DesignerActionPropertyItem("TextAlignment", "Text Alignment", "Appearance", "Set the alignment of tab text"),
                    new DesignerActionMethodItem(this, "AddPage", "Add Page", "Data", "Add a new page to the control"),
                    new DesignerActionMethodItem(this, "RemovePage", "Remove Page", "Data" ,"Remove the current page")
                };
            }
            #endregion
        }
    }
}
