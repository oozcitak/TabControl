using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

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
            /// Gets or sets the TabLocation of the designed control.
            /// </summary>
            [Editor(typeof(TabLocationEditor), typeof(UITypeEditor))]
            public TabLocation TabLocation
            {
                get { return Control.TabLocation; }
                set { GetPropertyByName("TabLocation").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the TabSizing of the designed control.
            /// </summary>
            public TabSizing TabSizing
            {
                get { return Control.TabSizing; }
                set { GetPropertyByName("TabSizing").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the TextDirection of the designed control.
            /// </summary>
            [Editor(typeof(TextDirectionEditor), typeof(UITypeEditor))]
            public TextDirection TextDirection
            {
                get { return Control.TextDirection; }
                set { GetPropertyByName("TextDirection").SetValue(Control, value); }
            }
            /// <summary>
            /// Gets or sets the ContentAlignment of the designed control.
            /// </summary>
            public Alignment ContentAlignment
            {
                get { return Control.ContentAlignment; }
                set { GetPropertyByName("ContentAlignment").SetValue(Control, value); }
            }

            /// <summary>
            /// Adds a tab to the designed control.
            /// </summary>
            public void AddTab()
            {
                if (DesignerHost != null)
                {
                    Tab tab = (Tab)DesignerHost.CreateComponent(typeof(Tab));
                    PropertyDescriptor nameProp = TypeDescriptor.GetProperties(tab)["Name"];
                    string name = (string)nameProp.GetValue(tab);
                    PropertyDescriptor textProp = TypeDescriptor.GetProperties(tab)["Text"];
                    textProp.SetValue(tab, name);

                    Control.Tabs.Add(tab);
                    Control.SelectedTab = tab;
                }
            }

            /// <summary>
            /// Removes the current page of the designed control.
            /// </summary>
            protected void RemoveTab()
            {
                if (DesignerHost != null)
                {
                    if (Control.Tabs.Count > 1)
                    {
                        Tab tab = Control.SelectedTab;
                        if (tab != null)
                        {
                            int index = Control.SelectedIndex;

                            DesignerHost.DestroyComponent(tab);
                            if (index == Control.Tabs.Count)
                                index = Control.Tabs.Count - 1;
                            Control.SelectedIndex = index;
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

                    new DesignerActionPropertyItem("TabLocation", "TabLocation", "Appearance", "Set the location of tabs relative to the control"),
                    new DesignerActionPropertyItem("TabSizing", "TabSizing", "Appearance", "Set the sizing mode of tabs"),
                    new DesignerActionPropertyItem("ContentAlignment", "ContentAlignment", "Appearance", "Set the alignment of tab contents"),
                    new DesignerActionPropertyItem("TextDirection", "TextDirection", "Appearance", "Set the direction of tab text"),
                    new DesignerActionMethodItem(this, "AddTab", "Add Tab", "Data", "Add a new tab to the control"),
                    new DesignerActionMethodItem(this, "RemoveTab", "Remove Tab", "Data" ,"Remove the current tab")
                };
            }
            #endregion
        }
    }
}
