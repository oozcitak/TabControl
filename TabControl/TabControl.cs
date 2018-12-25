using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(TabControl))]
    [Designer(typeof(TabControlDesigner))]
    [Docking(DockingBehavior.Ask)]
    [DefaultEvent("PageChanged")]
    [DefaultProperty("SelectedPage")]
    public partial class TabControl : PagedControl
    {
        #region Enums
        /// <summary>
        /// Represents the visual state of a tab header.
        /// </summary>
        [Flags]
        public enum TabHeaderState
        {
            /// <summary>
            /// The tab header is inactive.
            /// </summary>
            Inactive = 0,
            /// <summary>
            /// The tab header is the <see cref="PagedControl.SelectedPage"/> of the control.
            /// </summary>
            Active = 1,
            /// <summary>
            /// Mouse cursor is over the tab header.
            /// </summary>
            Hot = 2,
            /// <summary>
            /// The left mouse button is clicked over the tab header.
            /// </summary>
            Pressed = 4,
            /// <summary>
            /// The tab header has input focus.
            /// </summary>
            Focused = 8,
        }

        /// <summary>
        /// Represents the location of the tab header area within the control.
        /// </summary>
        public enum TabLocation
        {
            /// <summary>
            /// The top header area is at the top of the control.
            /// </summary>
            Top,
            /// <summary>
            /// The top header area is at the bottom of the control.
            /// </summary>
            Bottom,
            /// <summary>
            /// The top header area is at the left of the control. Tab header texts
            /// are drawn vertically.
            /// </summary>
            Left,
            /// <summary>
            /// The top header area is at the right of the control. Tab header texts
            /// are drawn vertically.
            /// </summary>
            Right
        }

        /// <summary>
        /// Represents the alignment of tab headers relative to the control.
        /// </summary>
        public enum Alignment
        {
            /// <summary>
            /// Tab headers are aligned to the left or top of the control.
            /// </summary>
            Near,
            /// <summary>
            /// Tab headers are aligned to center of the control.
            /// </summary>
            Center,
            /// <summary>
            /// Tab headers are aligned to the right or bottom of the control.
            /// </summary>
            Far
        }

        /// <summary>
        /// Represents the sizing behavior  of tabs.
        /// </summary>
        public enum TabSizing
        {
            /// <summary>
            /// Tabs are automatically sized to fit their texts.
            /// </summary>
            AutoFit,
            /// <summary>
            /// Tabs are fixed size which is defined by the <see cref="TabControl.TabHeaderSize"/>.
            /// </summary>
            Fixed,
            /// <summary>
            /// Tabs are stretched to fit the entire tab header area.
            /// </summary>
            Stretch
        }
        #endregion

        #region Events
        /// <summary>
        /// Contains event data for events related to a single tab header.
        /// </summary>
        public class TabHeaderEventArgs : EventArgs
        {
            /// <summary>
            /// Gets the index of the tab header.
            /// </summary>
            public int Index { get; private set; }

            public TabHeaderEventArgs(int index)
            {
                Index = index;
            }
        }

        /// <summary>
        /// Contains event data for mouse events related to a single tab header.
        /// </summary>
        public class TabHeaderMouseEventArgs : TabHeaderEventArgs
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

            public TabHeaderMouseEventArgs(int index, MouseButtons button, int clicks, int delta, Point location) : base(index)
            {
                Button = button;
                Clicks = clicks;
                Delta = delta;
                Location = location;
            }
        }

        public delegate void TabHeaderMouseEventHandler(object sender, TabHeaderMouseEventArgs e);

        protected internal virtual void OnTabHeaderClick(TabHeaderMouseEventArgs e) { TabHeaderClick?.Invoke(this, e); }
        protected internal virtual void OnTabHeaderDoubleClick(TabHeaderMouseEventArgs e) { TabHeaderDoubleClick?.Invoke(this, e); }
        protected internal virtual void OnTabHeaderMouseDown(TabHeaderMouseEventArgs e) { TabHeaderMouseDown?.Invoke(this, e); }
        protected internal virtual void OnTabHeaderMouseUp(TabHeaderMouseEventArgs e) { TabHeaderMouseUp?.Invoke(this, e); }
        protected internal virtual void OnTabHeaderMouseMove(TabHeaderMouseEventArgs e) { TabHeaderMouseMove?.Invoke(this, e); }
        protected internal virtual void OnTabHeaderMouseWheel(TabHeaderMouseEventArgs e) { TabHeaderMouseWheel?.Invoke(this, e); }

        /// <summary>
        /// Occurs when a tab header is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab header is clicked.")]
        public event TabHeaderMouseEventHandler TabHeaderClick;

        /// <summary>
        /// Occurs when a tab header is double clicked by the mouse.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab header is double clicked by the mouse.")]
        public event TabHeaderMouseEventHandler TabHeaderDoubleClick;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab header and a mouse button is pressed.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab header and a mouse button is pressed.")]
        public event TabHeaderMouseEventHandler TabHeaderMouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab header and a mouse button is released.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab header and a mouse button is released.")]
        public event TabHeaderMouseEventHandler TabHeaderMouseUp;

        /// <summary>
        /// Occurs when the mouse pointer is moved over a tab header.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is moved over a tab header.")]
        public event TabHeaderMouseEventHandler TabHeaderMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab header and the mouse wheel is rotated.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab header and the mouse wheel is rotated.")]
        public event TabHeaderMouseEventHandler TabHeaderMouseWheel;
        #endregion

        #region Member Variables
        private int hoveredTabHeader = -1;
        private int mouseDownTabHeader = -1;
        private Rectangle[] tabHeaderBounds = new Rectangle[0];

        private Size tabHeaderSize = new Size(75, 23);
        private TabLocation tabHeaderLocation = TabLocation.Top;
        private Alignment tabHeaderAlignment = Alignment.Near;
        private Alignment textAlignment = Alignment.Near;
        private TabSizing tabHeaderSizing = TabSizing.AutoFit;

        private TabControlRenderer renderer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the size of a tab header.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Size), "75, 23")]
        [Description("Gets or sets the size of a tab header.")]
        public Size TabHeaderSize { get => tabHeaderSize; set { tabHeaderSize = value; UpdateTabHeaderLayout(); UpdatePages(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the location of tab headers.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabLocation.Top)]
        [Description("Gets or sets the location of tab headers.")]
        public TabLocation TabHeaderLocation { get => tabHeaderLocation; set { tabHeaderLocation = value; UpdateTabHeaderLayout(); UpdatePages(); Invalidate(); } }
        /// <summary>
        /// Gets or sets the alignment of tab headers.
        /// </summary>
        [Category("Appearance"), DefaultValue(Alignment.Near)]
        [Description("Gets or sets the alignment of tab headers.")]
        public Alignment TabHeaderAlignment { get => tabHeaderAlignment; set { tabHeaderAlignment = value; UpdateTabHeaderLayout(); Invalidate(); } }
        /// <summary>
        /// Gets or sets the sizing mode of tab headers.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabSizing.AutoFit)]
        [Description("Gets or sets the sizing mode of tab headers.")]
        public TabSizing TabHeaderSizing { get => tabHeaderSizing; set { tabHeaderSizing = value; UpdateTabHeaderLayout(); Invalidate(); } }
        /// <summary>
        /// Gets or sets the alignment of tab text.
        /// </summary>
        [Category("Appearance"), DefaultValue(Alignment.Near)]
        [Description("Gets or sets the alignment of tab text.")]
        public Alignment TextAlignment { get => textAlignment; set { textAlignment = value; Invalidate(); } }

        /// <summary>
        /// Gets the rectangle that represents the client area of the control.
        /// </summary>
        [Browsable(false)]
        public new Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(0, 0, Width, Height);
            }
        }

        /// <summary>
        /// Gets the client rectangle where pages are located.
        /// </summary>
        [Browsable(false)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                var bounds = ClientRectangle;
                if (BorderStyle != BorderStyle.None)
                    bounds.Inflate(-1, -1);

                if (Pages.Count == 0)
                    return bounds;

                if (tabHeaderLocation == TabLocation.Top)
                    return new Rectangle(bounds.Left, bounds.Top + TabHeaderSize.Height, bounds.Width, bounds.Height - TabHeaderSize.Height);
                else if (tabHeaderLocation == TabLocation.Bottom)
                    return new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - TabHeaderSize.Height);
                else if (tabHeaderLocation == TabLocation.Left)
                    return new Rectangle(bounds.Left + TabHeaderSize.Height, bounds.Top, bounds.Width - TabHeaderSize.Height, bounds.Height);
                else // if (tabHeaderLocation == TabLocation.Right)
                    return new Rectangle(bounds.Left, bounds.Top, bounds.Width - TabHeaderSize.Height, bounds.Height);
            }
        }

        /// <summary>
        /// Gets the client rectangle where tab headers are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle TabHeaderArea
        {
            get
            {
                var bounds = ClientRectangle;
                if (tabHeaderLocation == TabLocation.Top)
                    return new Rectangle(bounds.Left, bounds.Top, bounds.Width, TabHeaderSize.Height + 1);
                else if (tabHeaderLocation == TabLocation.Bottom)
                    return new Rectangle(bounds.Left, bounds.Bottom - TabHeaderSize.Height - 1, bounds.Width, TabHeaderSize.Height + 1);
                else if (tabHeaderLocation == TabLocation.Left)
                    return new Rectangle(bounds.Left, bounds.Top, TabHeaderSize.Height + 1, bounds.Height);
                else // if (tabHeaderLocation == TabLocation.Right)
                    return new Rectangle(bounds.Right - TabHeaderSize.Height - 1, bounds.Top, TabHeaderSize.Height + 1, bounds.Height);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
        {
            SetRenderer(new TabControlRenderer(this));
            UpdateTabHeaderLayout();
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Performs a hit test with at given coordinates and returns the
        /// zero based index of the tab header which contains the given point.
        /// </summary>
        /// <param name="pt">hit test coordinates</param>
        /// <returns>The zero based index of the tab header which contains the given point; or -1
        /// if none of the tab headers contains the given point.</returns>
        public int PerformHitTest(Point pt)
        {
            if (!TabHeaderArea.Contains(pt)) return -1;

            for (int i = 0; i < Pages.Count; i++)
            {
                var bounds = GetTabHeaderBounds(i);
                if (bounds.Contains(pt)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Calculates the bounding rectangle of a tab header.
        /// </summary>
        /// <param name="index">The zero based index of the tab header.</param>
        /// <returns>A <see cref="Rectangle"/> which defines the bounds of the 
        /// given tab header.</returns>
        public Rectangle GetTabHeaderBounds(int index)
        {
            if (index == -1)
                return Rectangle.Empty;

            return tabHeaderBounds[index];
        }

        /// <summary>
        /// Changes the renderer associated with the control.
        /// </summary>
        /// <param name="renderer">The new renderer</param>
        public void SetRenderer(TabControlRenderer renderer)
        {
            this.renderer = renderer;
        }

        /// <summary>
        /// Returns the visual state of a tab header.
        /// </summary>
        /// <param name="index">The zero based index of the tab header.</param>
        public TabHeaderState GetTabState(int index)
        {
            TabHeaderState state = TabHeaderState.Inactive;

            // active
            if (SelectedIndex == index) state |= TabHeaderState.Active;
            // hot
            if (hoveredTabHeader == index) state |= TabHeaderState.Hot;
            // pressed
            if (mouseDownTabHeader == index) state |= TabHeaderState.Pressed;
            // focused
            if (Focused && (SelectedIndex == index)) state |= TabHeaderState.Focused;

            return state;
        }
        #endregion

        #region Overriden Methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateTabHeaderLayout();
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int index = PerformHitTest(e.Location);
            if (index != -1)
                SelectedIndex = index;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int oldHoveredTabHeader = hoveredTabHeader;
            hoveredTabHeader = PerformHitTest(e.Location);
            if (hoveredTabHeader != -1)
                OnTabHeaderMouseMove(new TabHeaderMouseEventArgs(hoveredTabHeader, e.Button, e.Clicks, e.Delta, e.Location));
            if (oldHoveredTabHeader != hoveredTabHeader)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hoveredTabHeader != -1)
            {
                hoveredTabHeader = -1;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (hoveredTabHeader != -1)
            {
                int oldmouseDownTabHeader = mouseDownTabHeader;
                mouseDownTabHeader = hoveredTabHeader;
                OnTabHeaderMouseDown(new TabHeaderMouseEventArgs(hoveredTabHeader, e.Button, e.Clicks, e.Delta, e.Location));
                if (oldmouseDownTabHeader != mouseDownTabHeader)
                    Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mouseDownTabHeader != -1)
                OnTabHeaderClick(new TabHeaderMouseEventArgs(mouseDownTabHeader, e.Button, e.Clicks, e.Delta, e.Location));

            if (hoveredTabHeader != -1)
                OnTabHeaderMouseUp(new TabHeaderMouseEventArgs(hoveredTabHeader, e.Button, e.Clicks, e.Delta, e.Location));

            int oldmouseDownTabHeader = mouseDownTabHeader;
            mouseDownTabHeader = -1;
            if (oldmouseDownTabHeader != mouseDownTabHeader)
                Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (mouseDownTabHeader != -1)
                OnTabHeaderDoubleClick(new TabHeaderMouseEventArgs(mouseDownTabHeader, e.Button, e.Clicks, e.Delta, e.Location));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (hoveredTabHeader != -1)
                OnTabHeaderMouseWheel(new TabHeaderMouseEventArgs(hoveredTabHeader, e.Button, e.Clicks, e.Delta, e.Location));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!OwnerDraw)
            {
                renderer.Render(e.Graphics);
            }
        }

        protected override void OnPageAdded(PageEventArgs e)
        {
            UpdateTabHeaderLayout();
            e.Page.TextChanged += Page_TextChanged;
            base.OnPageAdded(e);
        }

        protected override void OnPageRemoved(PageEventArgs e)
        {
            UpdateTabHeaderLayout();
            e.Page.TextChanged -= Page_TextChanged;
            base.OnPageRemoved(e);
        }
        #endregion

        #region Helper Methods
        private void Page_TextChanged(object sender, EventArgs e)
        {
            UpdateTabHeaderLayout();
            Invalidate();
        }
        /// <summary>
        /// Updates the layout of all tab headers.
        /// </summary>
        protected void UpdateTabHeaderLayout()
        {
            tabHeaderBounds = new Rectangle[Pages.Count];
            if (Pages.Count == 0) return;

            var bounds = TabHeaderArea;
            int[] tabHeaderWidths = new int[Pages.Count];
            int[] tabHeaderLocations = new int[Pages.Count];
            int width = (TabHeaderLocation == TabLocation.Top || TabHeaderLocation == TabLocation.Bottom) ? bounds.Width : bounds.Height;

            for (int index = 0; index < Pages.Count; index++)
            {
                int tabWidth = 0;
                if (TabHeaderSizing == TabSizing.Fixed)
                    tabWidth = TabHeaderSize.Width;
                else if (TabHeaderSizing == TabSizing.AutoFit)
                    tabWidth = TextRenderer.MeasureText(Pages[index].Text, Font, Size.Empty).Width + 8;
                else // if (TabHeaderSizing == TabSizing.Stretch)
                    tabWidth = (int)(width / (float)Pages.Count);

                tabHeaderWidths[index] = tabWidth;
            }

            // check if auto sized headers exceed control bounds
            if (TabHeaderSizing == TabSizing.AutoFit)
            {
                int averageHeaderWidth = (int)(width / (float)Pages.Count);
                int countOverAverage = 0;
                int totalWidth = 0;
                for (int index = 0; index < Pages.Count; index++)
                {
                    totalWidth += tabHeaderWidths[index];
                    if (tabHeaderWidths[index] > averageHeaderWidth)
                        countOverAverage++;
                }
                int toSubtract = (int)((totalWidth - width) / (float)countOverAverage);
                for (int index = 0; index < Pages.Count; index++)
                {
                    if (tabHeaderWidths[index] > averageHeaderWidth)
                        tabHeaderWidths[index] = tabHeaderWidths[index] - toSubtract;
                }
            }

            // calculate total width
            int totalTabWidth = 0;
            for (int index = 0; index < Pages.Count; index++)
            {
                totalTabWidth += tabHeaderWidths[index];
            }

            // place headers
            if (TabHeaderLocation == TabLocation.Left)
            {
                tabHeaderLocations[0] = (TabHeaderAlignment == Alignment.Near ? width : TabHeaderAlignment == Alignment.Far ? totalTabWidth : width - (width - totalTabWidth) / 2);
                for (int index = 1; index < Pages.Count; index++)
                {
                    tabHeaderLocations[index] = tabHeaderLocations[index - 1] - tabHeaderWidths[index - 1];
                }

                // make sure the last header does not exceed bounds
                if (tabHeaderLocations[Pages.Count - 1] - tabHeaderWidths[Pages.Count - 1] < 0)
                    tabHeaderWidths[Pages.Count - 1] = tabHeaderLocations[Pages.Count - 1];
            }
            else
            {
                tabHeaderLocations[0] = (TabHeaderAlignment == Alignment.Near ? 0 : TabHeaderAlignment == Alignment.Far ? width - totalTabWidth : (width - totalTabWidth) / 2);
                for (int index = 1; index < Pages.Count; index++)
                {
                    tabHeaderLocations[index] = tabHeaderLocations[index - 1] + tabHeaderWidths[index - 1];
                }

                // make sure the last header does not exceed bounds
                if (tabHeaderLocations[Pages.Count - 1] + tabHeaderWidths[Pages.Count - 1] > width)
                    tabHeaderWidths[Pages.Count - 1] = width - tabHeaderLocations[Pages.Count - 1];
            }

            // create header bounds
            for (int index = 0; index < Pages.Count; index++)
            {

                if (TabHeaderLocation == TabLocation.Top)
                    tabHeaderBounds[index] = new Rectangle(bounds.Left + tabHeaderLocations[index], bounds.Top, tabHeaderWidths[index], bounds.Height);
                else if (TabHeaderLocation == TabLocation.Bottom)
                    tabHeaderBounds[index] = new Rectangle(bounds.Left + tabHeaderLocations[index], bounds.Top, tabHeaderWidths[index], bounds.Height);
                else if (TabHeaderLocation == TabLocation.Left)
                    tabHeaderBounds[index] = new Rectangle(bounds.Left, bounds.Top + tabHeaderLocations[index] - tabHeaderWidths[index], bounds.Width, tabHeaderWidths[index]);
                else
                    tabHeaderBounds[index] = new Rectangle(bounds.Left, bounds.Top + tabHeaderLocations[index], bounds.Width, tabHeaderWidths[index]);
            }
        }
        #endregion
    }
}
