using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected class TabControlDesigner : PageContainerDesigner
        {
            #region Member Variables
            private Adorner tabButtonAdorner;
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

                tabButtonAdorner = new Adorner();
                BehaviorService.Adorners.Add(tabButtonAdorner);

                var tabGlyph = new TabGlyph(BehaviorService, this);
                tabButtonAdorner.Glyphs.Add(tabGlyph);
                
                Control.PageChanged += Control_PageChanged;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Control.PageChanged -= Control_PageChanged;
                }

                base.Dispose(disposing);
            }
            #endregion

            #region Event Handlers
            private void Control_PageChanged(object sender, PageChangedEventArgs e)
            {
                SelectionService.SetSelectedComponents(new object[] { Control });
            }
            #endregion
        }
    }
}