using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected internal class SelectedTabEditor : ObjectSelectorEditor
        {
            protected override void FillTreeWithData(Selector selector, ITypeDescriptorContext context, IServiceProvider provider)
            {
                base.FillTreeWithData(selector, context, provider);

                var control = (PagedControl)context.Instance;

                foreach (var page in control.Pages)
                {
                    SelectorNode node = new SelectorNode(page.Name, page);
                    selector.Nodes.Add(node);

                    if (page == control.SelectedPage)
                        selector.SelectedNode = node;
                }
            }
        }
    }
}
