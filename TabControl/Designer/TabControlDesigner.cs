using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected class TabControlDesigner : PageContainerDesigner
        {
            #region Member Variables
            private bool controlSelected = false;
            private DesignerActionListCollection actionLists = null;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the parent control.
            /// </summary>
            public new TabControl Control => (TabControl)base.Control;

            /// <summary>
            /// Gets the design-time action lists supported by the component associated with the designer.
            /// </summary>
            public override DesignerActionListCollection ActionLists
            {
                get
                {
                    if (actionLists == null)
                    {
                        actionLists = base.ActionLists;
                        actionLists.Add(new TabControlActionList(Control));
                    }
                    return actionLists;
                }
            }
            #endregion

            #region Initialize/Dispose
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);

                Control.PageChanged += Control_PageChanged;

                if (SelectionService != null)
                    SelectionService.SelectionChanged += SelectionService_SelectionChanged;
            }

            public override void InitializeNewComponent(IDictionary defaultValues)
            {
                // add two default tabs
                AddTabHandler(this, EventArgs.Empty);
                AddTabHandler(this, EventArgs.Empty);

                base.InitializeNewComponent(defaultValues);

                Control.SelectedIndex = 0;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Control.PageChanged -= Control_PageChanged;

                    if (SelectionService != null)
                        SelectionService.SelectionChanged -= SelectionService_SelectionChanged;
                }

                base.Dispose(disposing);
            }
            #endregion

            #region Verb Handlers
            /// <summary>
            /// Adds a new tab.
            /// </summary>
            protected void AddTabHandler(object sender, EventArgs e)
            {
                if (DesignerHost != null)
                {
                    MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];
                    RaiseComponentChanging(member);

                    Tab tab = (Tab)DesignerHost.CreateComponent(typeof(Tab));
                    PropertyDescriptor nameProp = TypeDescriptor.GetProperties(tab)["Name"];
                    string name = (string)nameProp.GetValue(tab);
                    PropertyDescriptor textProp = TypeDescriptor.GetProperties(tab)["Text"];
                    textProp.SetValue(tab, name);

                    Control.Tabs.Add(tab);
                    Control.SelectedTab = tab;

                    RaiseComponentChanged(member, null, null);
                }
            }
            #endregion

            #region Overridden Methods
            protected override bool GetHitTest(Point point)
            {
                if (controlSelected)
                {
                    point = Control.PointToClient(point);
                    foreach (var tab in Control.Tabs)
                    {
                        if (Control.GetTabBounds(tab).Contains(point))
                        {
                            return true;
                        }
                    }
                }
                return base.GetHitTest(point);
            }
            #endregion

            #region Event Handlers
            private void Control_PageChanged(object sender, PageChangedEventArgs e)
            {
                if (SelectionService != null)
                {
                    if (e.CurrentPage != null)
                        SelectionService.SetSelectedComponents(new object[] { e.CurrentPage });
                    else
                        SelectionService.SetSelectedComponents(new object[] { Control });
                }
            }

            private void SelectionService_SelectionChanged(object sender, EventArgs e)
            {
                controlSelected = false;
                foreach (var component in SelectionService.GetSelectedComponents())
                {
                    if (ReferenceEquals(component, Control) || (component is Tab tab && ReferenceEquals(tab.Parent, Control)))
                    {
                        controlSelected = true;
                        break;
                    }
                }
            }
            #endregion
        }
    }
}