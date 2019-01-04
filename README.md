[![License](http://img.shields.io/github/license/oozcitak/tabcontrol.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Nuget](https://img.shields.io/nuget/v/TabControl.svg?style=flat-square)](https://www.nuget.org/packages/TabControl)

TabControl is a .NET control with multiple tabs. The control is based on [PagedControl](https://github.com/oozcitak/PagedControl).

The control has full designer support for adding/removing tabs and dragging child controls. It is also possible to programmatically add/remove pages by using the `Tabs` property of the control.

![TabControl in Use](https://raw.githubusercontent.com/wiki/oozcitak/TabControl/TabControl.designer.png)

# Installation #

If you are using [NuGet](https://nuget.org/) you can install the assembly with:

`PM> Install-Package TabControl`

# Properties #

Following public properties are available.

|Name|Type|Description|
|----|----|-----------|
|SelectedTab       |Tab                           |Gets or sets the currently selected tab.|
|SelectedIndex     |int                           |Gets or sets the index of the currently selected tab.|
|Tab               |TabControl.TabCollection      |Gets the collection of tab.|
|CanGoBack         |bool                          |Gets whether the control can navigate to the previous tab.|
|CanGoNext         |bool                          |Gets whether the control can navigate to the next tab.|
|DisplayRectangle  |System.Drawing.Rectangle      |Gets the client rectangle where tabs are located. Deriving classes can override this property to modify tab bounds.|

# Methods #

Following public methods are available.

|Name|Description|
|----|-----------|
|GoBack()|Navigates to the previous tab if possible.|
|GoNext()|Navigates to the next tab if possible.|

# Events #

Following events are raised by the control:

|Name|Event Argument|Description|
|----|--------------|-----------|
|CurrentPageChanging|PagedControl.PageChangingEventArgs|Occurs before the selected page changes. The event arguments contains references to the currently selected page and the page to become selected. It is possible to make the control navigate to a different page by setting the `NewPage` property of the event arguments, or to cancel navigation entirely by setting `Cancel = true` while handling the event.|
|CurrentPageChanged |PagedControl.PageChangedEventArgs |Occurs after the selected page changes. The event arguments contains references to the currently selected page and the previous selected page.|
|PageAdded  |PagedControl.PageEventArgs|Occurs after a new page is added to the page collection. The event arguments contains a reference to the new page.|
|PageRemoved|PagedControl.PageEventArgs|Occurs after an existing page is removed from the page collection. The event arguments contains a reference to the removed page.|
|PageValidating|PagedControl.PageValidatingEventArgs|Occurs before the selected page changes and it needs to be validated. The event arguments contains a reference to the currently selected page. By setting `Cancel = true` while handling the event, the validation stops and the selected page is not changed.|
|PageValidated |PagedControl.PageEventArgs          |Occurs before the selected page changes and after it is successfully validated. The event arguments contains a reference to the currently selected page.|
|PageHidden    |PagedControl.PageEventArgs          |Occurs before the selected page changes and after the currently selected page is hidden. The event arguments contains a reference to the page.|
|PageShown     |PagedControl.PageEventArgs          |Occurs before the selected page changes and the page to become selected is shown. The event arguments contains a reference to the page.|
|PagePaint       |PagedControl.PagePaintEventArgs|Occurs when a page is needed to be painted. The control paints the background of the pages by default. |
