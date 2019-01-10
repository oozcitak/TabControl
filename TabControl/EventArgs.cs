using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains event data for events related to a single tab.
    /// </summary>
    public class TabEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the tab related to the event.
        /// </summary>
        public Tab Tab { get; private set; }

        public TabEventArgs(Tab tab)
        {
            Tab = tab;
        }
    }

    /// <summary>
    /// Contains cancellable event data for events related to a single tab.
    /// </summary>
    public class CancelTabEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the tab related to the event.
        /// </summary>
        public Tab Tab { get; private set; }

        public CancelTabEventArgs(Tab tab)
        {
            Tab = tab;
        }
    }

    /// <summary>
    /// Contains event data for mouse events related to a single tab.
    /// </summary>
    public class TabMouseEventArgs : TabEventArgs
    {
        /// <summary>
        /// Gets which mouse button was pressed.
        /// </summary>
        public MouseButtons Button { get; private set; }
        /// <summary>
        /// Gets the number of times the mouse button was pressed and released.
        /// </summary>
        public int Clicks { get; private set; }
        /// <summary>
        /// Gets the x-coordinate of the mouse during the generating mouse event.
        /// </summary>
        public int X => Location.X;
        /// <summary>
        /// Gets the y-coordinate of the mouse during the generating mouse event.
        /// </summary>
        public int Y => Location.Y;
        /// <summary>
        /// Gets a signed count of the number of detents the mouse wheel has rotated, 
        /// multiplied by the WHEEL_DELTA constant. A detent is one notch of the mouse wheel.
        /// </summary>
        public int Delta { get; }
        /// <summary>
        /// Gets the location of the mouse during the generating mouse event.
        /// </summary>
        public Point Location { get; }

        public TabMouseEventArgs(Tab tab, MouseButtons button, int clicks, int delta, Point location) : base(tab)
        {
            Button = button;
            Clicks = clicks;
            Delta = delta;
            Location = location;
        }
    }

    /// <summary>
    /// Contains event data for measurement related events.
    /// </summary>
    public class MeasureEventArgs : EventArgs
    {
        /// <summary>
        /// The size of the item to be measured.
        /// </summary>
        public Size Size { get; set; }

        public MeasureEventArgs(Size size)
        {
            Size = size;
        }
    }

    /// <summary>
    /// Contains event data for tab measurement related events.
    /// </summary>
    public class MeasureTabEventArgs : TabEventArgs
    {
        /// <summary>
        /// The size of the item to be measured.
        /// </summary>
        public Size Size { get; set; }

        public MeasureTabEventArgs(Tab tab, Size size) : base(tab)
        {
            Size = size;
        }
    }

    /// <summary>
    /// Contains event data for layout events.
    /// </summary>
    public class LayoutTabsEventArgs : EventArgs
    {
        /// <summary>
        /// The bounding rectangle where tabs are located.
        /// </summary>
        public Rectangle TabAreaBounds { get; set; }
        /// <summary>
        /// The bounding rectangle where pages are located.
        /// </summary>
        public Rectangle DisplayAreaBounds { get; set; }
        /// <summary>
        /// The bounding rectangle of the near scroll button.
        /// </summary>
        public Rectangle NearScrollButtonBounds { get; set; }
        /// <summary>
        /// The bounding rectangle of the far scroll button.
        /// </summary>
        public Rectangle FarScrollButtonBounds { get; set; }

        public LayoutTabsEventArgs(Rectangle tabArea, Rectangle displayArea, Rectangle nearButton, Rectangle farButton)
        {
            TabAreaBounds = tabArea;
            DisplayAreaBounds = displayArea;
            NearScrollButtonBounds = nearButton;
            FarScrollButtonBounds = farButton;
        }
    }
}
