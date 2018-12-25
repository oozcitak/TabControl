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
            /// Gets the list of designer verbs.
            /// </summary>
            public override DesignerVerbCollection Verbs => verbs;

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

                CreateVerbs();

                Control.PageAdded += Control_PageAdded;
                Control.PageRemoved += Control_PageRemoved;

                if (SelectionService != null)
                    SelectionService.SelectionChanged += SelectionService_SelectionChanged;
            }

            public override void InitializeNewComponent(IDictionary defaultValues)
            {
                base.InitializeNewComponent(defaultValues);

                // add a default page
                AddPageHandler(this, EventArgs.Empty);

                MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];
                RaiseComponentChanging(member);
                RaiseComponentChanged(member, null, null);

                Control.SelectedIndex = 0;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Control.PageAdded -= Control_PageAdded;
                    Control.PageRemoved -= Control_PageRemoved;

                    if (BehaviorService != null)
                        BehaviorService.Adorners.Remove(tabButtonAdorner);

                    if (SelectionService != null)
                        SelectionService.SelectionChanged -= SelectionService_SelectionChanged;
                }
                base.Dispose(disposing);
            }
            #endregion

            #region Helper Methods
            /// <summary>
            /// Creates the designer verbs.
            /// </summary>
            private void CreateVerbs()
            {
                addPageVerb = new DesignerVerb("Add page", new EventHandler(AddPageHandler));
                removePageVerb = new DesignerVerb("Remove page", new EventHandler(RemovePageHandler));

                verbs = new DesignerVerbCollection();
                verbs.AddRange(new DesignerVerb[] {
                    addPageVerb, removePageVerb
            });
            }

            /// <summary>
            /// Updates the adorner when the selection is changed.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void SelectionService_SelectionChanged(object sender, EventArgs e)
            {
                bool showAdorner = false;

                if (SelectionService != null && SelectionService.PrimarySelection != null)
                {
                    if (SelectionService.PrimarySelection == Control)
                        showAdorner = true;
                    else if (SelectionService.PrimarySelection is Page page && page.Parent == Control)
                        showAdorner = true;
                }

                tabButtonAdorner.Enabled = showAdorner;

                UpdateTabButtons();
            }

            /// <summary>
            /// Updates verbs and toolbar buttons when a new page is added.
            /// </summary>
            private void Control_PageAdded(object sender, PagedControl.PageEventArgs e)
            {
                UpdateTabButtons();
            }

            /// <summary>
            /// Updates verbs and toolbar buttons when a page is removed.
            /// </summary>
            private void Control_PageRemoved(object sender, PagedControl.PageEventArgs e)
            {
                UpdateTabButtons();
            }

            /// <summary>
            /// Updates the visual states of the tab buttons.
            /// </summary>
            private void UpdateTabButtons()
            {
                removePageVerb.Enabled = (Control.Pages.Count > 1);
            }

            private void AddPageButton_Click(object sender, EventArgs e)
            {
                AddPageHandler(this, EventArgs.Empty);
            }

            private void RemovePageButton_Click(object sender, EventArgs e)
            {
                RemovePageHandler(this, EventArgs.Empty);
            }
            #endregion

            #region Parent/Child Relation
            public override bool CanParent(Control control)
            {
                return (control is Page);
            }

            public override bool CanParent(ControlDesigner controlDesigner)
            {
                return (controlDesigner != null && controlDesigner.Component is Page);
            }
            #endregion
        }
    }
}