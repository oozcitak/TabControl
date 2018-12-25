using System;
using System.ComponentModel;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected class TabControlDesigner : PagedControl.PagedControlDesigner
        {
            #region Member Variables
            private Adorner tabButtonAdorner;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the parent control.
            /// </summary>
            public new TabControl Control => (TabControl)base.Control;
            #endregion

            #region Initialize/Dispose
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);

                ToolBar.Visible = false;

                tabButtonAdorner = new Adorner();
                BehaviorService.Adorners.Add(tabButtonAdorner);

                var tabGlyph = new TabGlyph(BehaviorService, this);
                tabButtonAdorner.Glyphs.Add(tabGlyph);

                Control.PageChanged += Control_PageChanged;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing)
                {
                    Control.PageChanged -= Control_PageChanged;
                }
            }
            #endregion

            #region Event Handlers
            private void Control_PageChanged(object sender, PageChangedEventArgs e)
            {
                SelectionService.SetSelectedComponents(new object[0]);
            }
            #endregion
        }
    }
}