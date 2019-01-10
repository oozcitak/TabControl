using System;
using System.ComponentModel;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the visual state of an item.
    /// </summary>
    [Flags]
    public enum ItemState
    {
        /// <summary>
        /// The item is inactive.
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// Mouse cursor is over the item.
        /// </summary>
        Hot = 1,
        /// <summary>
        /// The left mouse button is clicked over the item.
        /// </summary>
        Pressed = 2,
        /// <summary>
        /// The item has input focus.
        /// </summary>
        Focused = 4,
        /// <summary>
        /// The item is disabled.
        /// </summary>
        Disabled = 8,
    }

    /// <summary>
    /// Represents the location of the tab area within the control.
    /// </summary>
    [Flags]
    public enum TabLocation
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        None = 0,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Near = 1,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Center = 2,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Far = 4,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Top = 8,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Bottom = 16,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Left = 32,
        [EditorBrowsable(EditorBrowsableState.Never)]
        Right = 64,
        /// <summary>
        /// The tab area is at the top of the control.
        /// Tab buttons are aligned to the left of the tab area.
        /// </summary>
        TopLeft = Top | Near,
        /// <summary>
        /// The tab area is at the top of the control.
        /// Tab buttons are aligned to the center of the tab area.
        /// </summary>
        TopCenter = Top | Center,
        /// <summary>
        /// The tab area is at the top of the control.
        /// Tab buttons are aligned to the right of the tab area.
        /// </summary>
        TopRight = Top | Far,
        /// <summary>
        /// The tab area is at the bottom of the control.
        /// Tab buttons are aligned to the left of the tab area.
        /// </summary>
        BottomLeft = Bottom | Near,
        /// <summary>
        /// The tab area is at the bottom of the control.
        /// Tab buttons are aligned to the center of the tab area.
        /// </summary>
        BottomCenter = Bottom | Center,
        /// <summary>
        /// The tab area is at the bottom of the control.
        /// Tab buttons are aligned to the right of the tab area.
        /// </summary>
        BottomRight = Bottom | Far,
        /// <summary>
        /// The tab area is at the left of the control.
        /// Tab buttons are aligned to the top of the tab area.
        /// </summary>
        LeftTop = Left | Near,
        /// <summary>
        /// The tab area is at the left of the control.
        /// Tab buttons are aligned to the center of the tab area.
        /// </summary>
        LeftCenter = Left | Center,
        /// <summary>
        /// The tab area is at the left of the control.
        /// Tab buttons are aligned to the bottom of the tab area.
        /// </summary>
        LeftBottom = Left | Far,
        /// <summary>
        /// The tab area is at the right of the control.
        /// Tab buttons are aligned to the top of the tab area.
        /// </summary>
        RightTop = Right | Near,
        /// <summary>
        /// The tab area is at the right of the control.
        /// Tab buttons are aligned to the center of the tab area.
        /// </summary>
        RightCenter = Right | Center,
        /// <summary>
        /// The tab area is at the right of the control.
        /// Tab buttons are aligned to the bottom of the tab area.
        /// </summary>
        RightBottom = Right | Far,
    }

    /// <summary>
    /// Represents the alignment of tab contents relative to the tab.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Content is aligned to the left or top of the tab.
        /// </summary>
        Near,
        /// <summary>
        /// Content is aligned to center of the tab.
        /// </summary>
        Center,
        /// <summary>
        /// Content is aligned to the right or bottom of the tab.
        /// </summary>
        Far
    }

    /// <summary>
    /// Represents the sizing behavior of tabs.
    /// </summary>
    public enum TabSizing
    {
        /// <summary>
        /// Tabs are automatically sized to fit their texts.
        /// </summary>
        AutoFit,
        /// <summary>
        /// Tabs are fixed size which is defined by the <see cref="TabControl.TabSize"/>.
        /// </summary>
        Fixed,
        /// <summary>
        /// Tabs are stretched to fit the entire tab area.
        /// </summary>
        Stretch
    }

    public enum TextDirection
    {
        /// <summary>
        /// Text is drawn left-to-right.
        /// </summary>
        Right,
        /// <summary>
        /// Text is rotated 90 degrees pointing down.
        /// </summary>
        Down,
        /// <summary>
        /// Text is rotated 90 degrees pointing up.
        /// </summary>
        Up
    }

    /// <summary>
    /// Represents a scroll button.
    /// </summary>
    internal enum ScrollButton
    {
        None,
        /// <summary>
        /// The near scroll button.
        /// </summary>
        Near,
        /// <summary>
        /// The far scroll button.
        /// </summary>
        Far
    }
}
