using System;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the visual state of a tab.
    /// </summary>
    [Flags]
    public enum TabState
    {
        /// <summary>
        /// The tab is inactive.
        /// </summary>
        Inactive = 0,
        /// <summary>
        /// The tab is the <see cref="PagedControl.SelectedPage"/> of the control.
        /// </summary>
        Active = 1,
        /// <summary>
        /// Mouse cursor is over the tab.
        /// </summary>
        Hot = 2,
        /// <summary>
        /// The left mouse button is clicked over the tab.
        /// </summary>
        Pressed = 4,
        /// <summary>
        /// The tab has input focus.
        /// </summary>
        Focused = 8,
    }

    /// <summary>
    /// Represents the location of the tab area within the control.
    /// </summary>
    public enum TabLocation
    {
        /// <summary>
        /// The top area is at the top of the control.
        /// </summary>
        Top,
        /// <summary>
        /// The top area is at the bottom of the control.
        /// </summary>
        Bottom,
        /// <summary>
        /// The top area is at the left of the control. Tab texts
        /// are drawn vertically.
        /// </summary>
        Left,
        /// <summary>
        /// The top area is at the right of the control. Tab texts
        /// are drawn vertically.
        /// </summary>
        Right
    }

    /// <summary>
    /// Represents the alignment of tabs relative to the control.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Tabs are aligned to the left or top of the control.
        /// </summary>
        Near,
        /// <summary>
        /// Tabs are aligned to center of the control.
        /// </summary>
        Center,
        /// <summary>
        /// Tabs are aligned to the right or bottom of the control.
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
}
