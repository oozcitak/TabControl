﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        protected internal class TabLocationEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                var selector = new TabLocationSelector();
                selector.TabLocation = (TabLocation)value;
                selector.MouseClick += (sender, e) => editorService.CloseDropDown();

                editorService.DropDownControl(selector);

                return selector.TabLocation;
            }
        }
    }
}
