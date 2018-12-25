using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Allows tab headers to be clicked in the designer.
        /// This is an invisible glyph since tab headers are already drawn
        /// by the control.
        /// </summary>
        internal class TabGlyph : Glyph
        {
            #region Properties
            /// <summary>
            /// Gets the behavior service.
            /// </summary>
            public BehaviorService BehaviorService { get; private set; }

            /// <summary>
            /// Gets the designed control.
            /// </summary>
            public TabControl Control { get; private set; }
            #endregion

            #region Constructor
            public TabGlyph(BehaviorService behaviorService, ControlDesigner designer) : base(new TabGlyphBehavior())
            {
                this.BehaviorService = behaviorService;
                this.Control = (TabControl)designer.Component;
            }
            #endregion

            #region Behavior
            /// <summary>
            /// Represents the behaviour associated with toolbars.
            /// The behaviour raises a click event when a tab is clicked.
            /// </summary>
            internal class TabGlyphBehavior : Behavior
            {
                public override bool OnMouseDown(Glyph g, MouseButtons mouseButton, Point mouseLoc)
                {
                    if (mouseButton == MouseButtons.Left)
                    {
                        TabGlyph tabGlyph = (TabGlyph)g;
                        Point pt = tabGlyph.BehaviorService.ControlToAdornerWindow(tabGlyph.Control);

                        int index = tabGlyph.Control.PerformHitTest(mouseLoc.GetOffset(-pt.X, -pt.Y));
                        if (index != -1)
                            tabGlyph.Control.SelectedIndex = index;
                        tabGlyph.Control.Invalidate();
                    }

                    return false;
                }
            }
            #endregion

            #region Overriden Methods
            /// <summary>
            /// Determines whether the given point is over a toolbar button.
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public override Cursor GetHitTest(Point p)
            {
                Point ptControl = BehaviorService.ControlToAdornerWindow(Control);
                return (Control.PerformHitTest(p.GetOffset(-ptControl.X, -ptControl.Y)) != -1 ? Cursors.Default : null);
            }

            /// <summary>
            /// Paints the adorner.
            /// </summary>
            /// <param name="pe">Paint event arguments.</param>
            public override void Paint(PaintEventArgs pe)
            {
                // this adorner is invisible
            }
            #endregion
        }
    }
}
