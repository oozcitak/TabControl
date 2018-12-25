using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
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
            }
            #endregion
        }
    }
}