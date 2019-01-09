using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    [ToolboxBitmap(typeof(TabControl))]
    [Designer(typeof(TabControlDesigner))]
    [Docking(DockingBehavior.Ask)]
    [DefaultEvent("PageChanged")]
    [DefaultProperty("SelectedTab")]
    public partial class TabControl : PagedControl
    {
        #region Events
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

        public delegate void TabMouseEventHandler(object sender, TabMouseEventArgs e);
        public delegate void CancelTabEventHandler(object sender, CancelTabEventArgs e);
        public delegate void MeasureTabEventHandler(object sender, MeasureTabEventArgs e);
        public delegate void MeasureEventHandler(object sender, MeasureEventArgs e);
        public delegate void LayoutTabsEventHandler(object sender, LayoutTabsEventArgs e);

        protected internal virtual void OnTabClick(TabMouseEventArgs e) { TabClick?.Invoke(this, e); }
        protected internal virtual void OnTabDoubleClick(TabMouseEventArgs e) { TabDoubleClick?.Invoke(this, e); }
        protected internal virtual void OnTabMouseDown(TabMouseEventArgs e) { TabMouseDown?.Invoke(this, e); }
        protected internal virtual void OnTabMouseUp(TabMouseEventArgs e) { TabMouseUp?.Invoke(this, e); }
        protected internal virtual void OnTabMouseMove(TabMouseEventArgs e) { TabMouseMove?.Invoke(this, e); }
        protected internal virtual void OnTabMouseWheel(TabMouseEventArgs e) { TabMouseWheel?.Invoke(this, e); }

        protected internal virtual void OnCloseTabButtonClick(CancelTabEventArgs e)
        {
            CloseTabButtonClick?.Invoke(this, e);
            if (!e.Cancel && SelectedPage != null)
                Pages.Remove(SelectedPage);
        }

        protected internal virtual void OnMeasureTab(MeasureTabEventArgs e)
        {
            int width = 0;
            int height = 0;

            if (e.Tab.HasIcon)
            {
                width = width + e.Tab.Icon.Width;
                height = Math.Max(height, e.Tab.Icon.Height);
            }

            if (e.Tab.HasText)
            {
                Size size = TextRenderer.MeasureText(e.Tab.Text, e.Tab.Font);
                width = width + size.Width;
                height = Math.Max(height, size.Height);
            }

            if (e.Tab.HasIcon && e.Tab.HasText)
                width += ContentSpacing;

            if (ShowCloseTabButtons)
            {
                if (width != 0)
                    width += ContentSpacing;

                width = width + CloseTabImage.Width;
                height = Math.Max(height, CloseTabImage.Height);
            }

            if (TextDirection == TextDirection.Right)
                e.Size = new Size(width, height) + TabPadding.Size;
            else
                e.Size = new Size(height, width) + TabPadding.Size;

            MeasureTab?.Invoke(this, e);
        }
        protected internal virtual void OnMeasureNearScrollButton(MeasureEventArgs e)
        {
            e.Size = new Size(16, 16) + TabPadding.Size;

            MeasureNearScrollButton?.Invoke(this, e);
        }
        protected internal virtual void OnMeasureFarScrollButton(MeasureEventArgs e)
        {
            e.Size = new Size(16, 16) + TabPadding.Size;

            MeasureFarScrollButton?.Invoke(this, e);
        }

        protected internal virtual void OnLayoutTabs(LayoutTabsEventArgs e) { LayoutTabs?.Invoke(this, e); }

        /// <summary>
        /// Occurs when a tab is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab is clicked.")]
        public event TabMouseEventHandler TabClick;

        /// <summary>
        /// Occurs when a tab is double clicked by the mouse.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab is double clicked by the mouse.")]
        public event TabMouseEventHandler TabDoubleClick;

        /// <summary>
        /// Occurs when a close tab button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a close tab button is clicked.")]
        public event CancelTabEventHandler CloseTabButtonClick;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and a mouse button is pressed.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and a mouse button is pressed.")]
        public event TabMouseEventHandler TabMouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and a mouse button is released.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and a mouse button is released.")]
        public event TabMouseEventHandler TabMouseUp;

        /// <summary>
        /// Occurs when the mouse pointer is moved over a tab.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is moved over a tab.")]
        public event TabMouseEventHandler TabMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and the mouse wheel is rotated.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and the mouse wheel is rotated.")]
        public event TabMouseEventHandler TabMouseWheel;

        /// <summary>
        /// Occurs when the size of a tab needs to be determined.
        /// </summary>
        [Category("Appearance"), Description("Occurs when the size of a tab needs to be determined.")]
        public event MeasureTabEventHandler MeasureTab;

        /// <summary>
        /// Occurs when the size of the near scroll button needs to be determined.
        /// </summary>
        [Category("Appearance"), Description("Occurs when the size of the near scroll button needs to be determined.")]
        public event MeasureEventHandler MeasureNearScrollButton;

        /// <summary>
        /// Occurs when the size of the far scroll button needs to be determined.
        /// </summary>
        [Category("Appearance"), Description("Occurs when the size of the far scroll button needs to be determined.")]
        public event MeasureEventHandler MeasureFarScrollButton;

        /// <summary>
        /// Occurs when the layout of all interface elements are calculated.
        /// </summary>
        [Category("Appearance"), Description("Occurs when the layout of all interface elements are calculated.")]
        public event LayoutTabsEventHandler LayoutTabs;
        #endregion

        #region Member Variables
        internal Tab hoveredTab = null;
        internal Tab mouseDownTab = null;
        internal bool hoveredButton = false;
        internal bool mouseDownButton = false;

        private Rectangle tabArea;
        private Rectangle displayArea;
        private Rectangle nearScrollButtonBounds;
        private Rectangle farScrollButtonBounds;

        private Size tabSize = new Size(75, 23);
        private TabLocation tabLocation = TabLocation.TopLeft;
        private Alignment contentAlignment = Alignment.Near;
        private TabSizing tabSizing = TabSizing.AutoFit;
        private Padding tabPadding = new Padding(4);
        private TextDirection textDirection = TextDirection.Right;
        private Image defaultCloseImage = null;
        private Image closeImage = null;
        private bool showCloseTabButtons = false;
        private int contentSpacing = 3;

        private bool scrollButtons;
        private int viewOffset = 0;
        private int maxViewOffset = 0;

        private TabControlRenderer renderer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the renderer associated with the control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabControlRenderer Renderer { get => renderer; set { renderer = value; Invalidate(); } }

        /// <summary>
        /// Gets or sets the size of tabs.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Size), "75, 23")]
        [Description("Gets or sets the size of tabs.")]
        public Size TabSize { get => tabSize; set { tabSize = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the padding between the contents a tab and its borders.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Padding), "4, 4, 4, 4")]
        [Description("Gets or sets the padding between the contents a tab and its borders.")]
        public Padding TabPadding { get => tabPadding; set { tabPadding = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the location of tabs.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabLocation.TopLeft)]
        [Description("Gets or sets the location of tabs.")]
        [Editor(typeof(TabLocationEditor), typeof(UITypeEditor))]
        public TabLocation TabLocation { get => tabLocation; set { tabLocation = value; viewOffset = 0; UpdateTabLayout(); UpdatePages(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the sizing mode of tabs.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabSizing.AutoFit)]
        [Description("Gets or sets the sizing mode of tabs.")]
        public TabSizing TabSizing { get => tabSizing; set { tabSizing = value; viewOffset = 0; UpdateTabLayout(); UpdatePages(); Invalidate(); } }
        /// <summary>
        /// Gets or sets the alignment of tab text.
        /// </summary>
        [Category("Appearance"), DefaultValue(Alignment.Near)]
        [Description("Gets or sets the alignment of tab text.")]
        public Alignment ContentAlignment { get => contentAlignment; set { contentAlignment = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }
        /// <summary>
        /// Gets or sets the direction of tab texts.
        /// </summary>
        [Category("Appearance"), DefaultValue(TextDirection.Right)]
        [Description("Gets or sets the direction of tab texts.")]
        [Editor(typeof(TextDirectionEditor), typeof(UITypeEditor))]
        public TextDirection TextDirection { get => textDirection; set { textDirection = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the image of close buttons.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of close buttons.")]
        public Image CloseTabImage { get => (closeImage ?? defaultCloseImage); set { closeImage = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets whether to show close tab buttons.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        [Description("Gets or sets whether to show close tab buttons.")]
        public bool ShowCloseTabButtons { get => showCloseTabButtons; set { showCloseTabButtons = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the spacing between tab contents (i.e. icon, text and close button).
        /// </summary>
        [Category("Appearance"), DefaultValue(3)]
        [Description("Gets or sets the spacing between tab contents (i.e. icon, text and close button).")]
        public int ContentSpacing { get => contentSpacing; set { contentSpacing = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets or sets the collection of pages.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override PageCollection Pages => base.Pages;

        /// <summary>
        /// Gets the collection of tabs.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets the collection of tabs.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabCollection Tabs { get; private set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Page SelectedPage { get => base.SelectedPage; set => base.SelectedPage = value; }

        /// <summary>
        /// Gets or sets the current tab.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the current tab.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor(typeof(SelectedTabEditor), typeof(UITypeEditor))]
        public Tab SelectedTab { get => (Tab)SelectedPage; set => SelectedPage = value; }

        /// <summary>
        /// Gets the rectangle that represents the client area of the control.
        /// </summary>
        [Browsable(false)]
        public new Rectangle ClientRectangle => new Rectangle(0, 0, Width, Height);

        /// <summary>
        /// Gets the client rectangle where pages are located.
        /// </summary>
        [Browsable(false)]
        public override Rectangle DisplayRectangle => displayArea;

        /// <summary>
        /// Gets the client rectangle where tabs are located.
        /// </summary>
        [Browsable(false)]
        public Rectangle TabArea => tabArea;

        /// <summary>
        /// Gets whether the scroll buttons are visible.
        /// </summary>
        [Browsable(false)]
        public bool ScrollButtons => scrollButtons;

        /// <summary>
        /// Gets or sets the font associated with the control.
        /// </summary>
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the font associated with the control.")]
        public override Font Font { get => base.Font; set { base.Font = value; UpdateTabLayout(); UpdatePages(); CheckViewOffset(); Invalidate(); } }

        /// <summary>
        /// Gets the bounds of the near scroll button. Although scroll button size is returned
        /// by the renderer, it may be resized along with tabs while layout logic is applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle NearScrollButtonBounds => nearScrollButtonBounds;

        /// <summary>
        /// Gets the bounds of the far scroll button. Although scroll button size is returned
        /// by the renderer, it may be resized along with tabs while layout logic is applied.
        /// </summary>
        [Browsable(false)]
        protected internal Rectangle FarScrollButtonBounds => farScrollButtonBounds;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
        {
            defaultCloseImage = Properties.Resources.CloseImage;

            Tabs = new TabCollection(this);
            renderer = new TabControlRenderer(this);
            UpdateTabLayout();
        }
        #endregion

        #region Property Defaults
        /// <summary>
        /// Determines if the close image should be serialized.
        /// </summary>
        /// <returns>true if the designer should serialize 
        /// the property; otherwise false.</returns>
        internal bool ShouldSerializeCloseTabImage()
        {
            return (closeImage != null);
        }
        /// <summary>
        /// Resets the close image to its default value.
        /// </summary>
        internal void ResetCloseTabImage()
        {
            closeImage = null;
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Performs a hit test with given coordinates and returns the
        /// <see cref="Tab"/> which contains the given point.
        /// </summary>
        /// <param name="pt">Hit test coordinates</param>
        /// <returns>The tab which contains the given point; or null
        /// if none of the tabs contains the given point.</returns>
        public Tab PerformHitTest(Point pt)
        {
            if (!TabArea.Contains(pt)) return null;

            foreach (var tab in Tabs)
            {
                if (tab.TabBounds.Contains(pt)) return tab;
            }
            return null;
        }
        #endregion

        #region Overriden Methods
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateTabLayout();
            UpdatePages();
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var tab = PerformHitTest(e.Location);
            if (tab != null && !ReferenceEquals(tab, SelectedPage))
                SelectedPage = tab;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var oldHoveredTab = hoveredTab;
            var oldHoveredButton = hoveredButton;
            hoveredTab = PerformHitTest(e.Location);
            hoveredButton = false;
            if (hoveredTab != null)
            {
                hoveredButton = hoveredTab.CloseButtonBounds.Contains(e.Location);
                OnTabMouseMove(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));
            }
            if (!ReferenceEquals(oldHoveredTab, hoveredTab) || oldHoveredButton != hoveredButton)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hoveredTab != null)
            {
                hoveredTab = null;
                hoveredButton = false;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (hoveredTab != null)
            {
                var oldmouseDownTab = mouseDownTab;
                mouseDownTab = hoveredTab;
                mouseDownButton = false;
                var oldMouseDownButton = mouseDownButton;
                if (ReferenceEquals(mouseDownTab, SelectedPage) && hoveredButton != false)
                {
                    mouseDownButton = mouseDownTab.CloseButtonBounds.Contains(e.Location);
                }
                OnTabMouseDown(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));
                if (oldmouseDownTab != mouseDownTab || oldMouseDownButton != mouseDownButton)
                    Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (mouseDownTab != null)
                OnTabClick(new TabMouseEventArgs(mouseDownTab, e.Button, e.Clicks, e.Delta, e.Location));

            if (hoveredTab != null)
                OnTabMouseUp(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));

            if (mouseDownButton != false)
                OnCloseTabButtonClick(new CancelTabEventArgs(mouseDownTab));

            var oldmouseDownTab = mouseDownTab;
            mouseDownTab = null;
            hoveredButton = false;
            mouseDownButton = false;
            if (!ReferenceEquals(oldmouseDownTab, mouseDownTab))
                Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (mouseDownTab != null)
                OnTabDoubleClick(new TabMouseEventArgs(mouseDownTab, e.Button, e.Clicks, e.Delta, e.Location));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (hoveredTab != null)
                OnTabMouseWheel(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            renderer.Render(e.Graphics);
        }

        protected override void OnPageAdded(PageEventArgs e)
        {
            e.Page.TextChanged += Page_TextChanged;

            base.OnPageAdded(e);

            UpdateTabLayout();
            Invalidate();
        }

        protected override void OnPageRemoved(PageEventArgs e)
        {
            e.Page.TextChanged -= Page_TextChanged;

            base.OnPageRemoved(e);

            UpdateTabLayout();
            Invalidate();
        }
        #endregion

        #region Helper Methods
        private void Page_TextChanged(object sender, EventArgs e)
        {
            UpdateTabLayout();
            Invalidate();
        }

        /// <summary>
        /// Makes sure that the view offset is within the scrollable width.
        /// </summary>
        private void CheckViewOffset()
        {
            if (ScrollButtons)
                viewOffset = Math.Max(0, Math.Min(viewOffset, maxViewOffset));
            else
                viewOffset = 0;
        }

        /// <summary>
        /// Updates the layout of all tabs.
        /// </summary>
        protected void UpdateTabLayout()
        {
            tabArea = Rectangle.Empty;
            displayArea = ClientRectangle;
            if (BorderStyle != BorderStyle.None)
                displayArea.Inflate(-1, -1);

            if (Pages.Count == 0) return;

            bool horizontal = (tabLocation & TabLocation.Top) != TabLocation.None || (tabLocation & TabLocation.Bottom) != TabLocation.None;
            var bounds = ClientRectangle;

            // measure tabs
            var tabSizes = Tabs.Select(t =>
            {
                if (TabSizing == TabSizing.Fixed)
                {
                    return TabSize;
                }
                else if (TabSizing == TabSizing.AutoFit)
                {
                    MeasureTabEventArgs e = new MeasureTabEventArgs(t, Size.Empty);
                    OnMeasureTab(e);
                    return e.Size;
                }
                else // if (TabSizing == TabSizing.Stretch)
                {
                    MeasureTabEventArgs e = new MeasureTabEventArgs(t, Size.Empty);
                    OnMeasureTab(e);
                    if (horizontal)
                    {
                        int stretchedWidth = Math.Max(10, (int)(bounds.Width / (float)Tabs.Count));
                        return new Size(stretchedWidth, e.Size.Height);
                    }
                    else
                    {
                        int stretchedHeight = Math.Max(10, (int)(bounds.Height / (float)Tabs.Count));
                        return new Size(e.Size.Width, stretchedHeight);
                    }
                }
            }).ToArray();

            // tolerate up-to a 2-pixel difference by sizing last tab
            int totalTabWidth = tabSizes.Sum(t => t.Width);
            int totalTabHeight = tabSizes.Sum(t => t.Height);
            if (horizontal)
            {
                int diff = totalTabWidth - bounds.Width;
                if (Math.Abs(diff) <= 2)
                {
                    Size size = tabSizes[tabSizes.Length - 1];
                    size.Width -= diff;
                    tabSizes[tabSizes.Length - 1] = size;
                    totalTabWidth -= diff;
                }
            }
            else
            {
                int diff = totalTabHeight - bounds.Height;
                if (Math.Abs(diff) <= 2)
                {
                    Size size = tabSizes[tabSizes.Length - 1];
                    size.Height -= diff;
                    tabSizes[tabSizes.Length - 1] = size;
                    totalTabHeight -= diff;
                }
            }

            // calculate maximum tab size
            Size maxSize = new Size(0, 0);
            foreach (var size in tabSizes)
            {
                maxSize.Width = Math.Max(maxSize.Width, size.Width);
                maxSize.Height = Math.Max(maxSize.Height, size.Height);
            }

            // make sure tabs have the same height (if horizontal) or width (if vertical)
            for (int i = 0; i < tabSizes.Length; i++)
            {
                Size size = tabSizes[i];
                if (horizontal)
                    size.Height = maxSize.Height;
                else
                    size.Width = maxSize.Width;
                tabSizes[i] = size;
            }

            // update tab area
            if ((tabLocation & TabLocation.Top) != TabLocation.None)
                tabArea = new Rectangle(bounds.Left, bounds.Top, bounds.Width, maxSize.Height);
            else if ((tabLocation & TabLocation.Bottom) != TabLocation.None)
                tabArea = new Rectangle(bounds.Left, bounds.Bottom - maxSize.Height, bounds.Width, maxSize.Height);
            else if ((tabLocation & TabLocation.Left) != TabLocation.None)
                tabArea = new Rectangle(bounds.Left, bounds.Top, maxSize.Width, bounds.Height);
            else
                tabArea = new Rectangle(bounds.Right - maxSize.Width, bounds.Top, maxSize.Width, bounds.Height);

            // do we need to show scroll buttons?
            maxViewOffset = Math.Max(0, horizontal ? totalTabWidth - tabArea.Width : totalTabHeight - tabArea.Height);
            scrollButtons = (maxViewOffset > 0);
            if (scrollButtons)
            {
                // update tab area to account for scroll buttons
                MeasureEventArgs eNear = new MeasureEventArgs(Size.Empty);
                MeasureEventArgs eFar = new MeasureEventArgs(Size.Empty);
                OnMeasureNearScrollButton(eNear);
                OnMeasureFarScrollButton(eFar);
                var nearScrollButtonSize = eNear.Size;
                var farScrollButtonSize = eFar.Size;

                if ((tabLocation & TabLocation.Top) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, nearScrollButtonSize.Width, maxSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Right - farScrollButtonSize.Width, bounds.Top, farScrollButtonSize.Width, maxSize.Height);

                    tabArea.X = bounds.Left + nearScrollButtonBounds.Width;
                    tabArea.Width = bounds.Width - nearScrollButtonBounds.Width - farScrollButtonBounds.Width;
                    maxViewOffset = totalTabWidth - tabArea.Width;
                }
                else if ((tabLocation & TabLocation.Bottom) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - nearScrollButtonSize.Height, nearScrollButtonSize.Width, maxSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Right - farScrollButtonSize.Width, bounds.Bottom - nearScrollButtonSize.Height, farScrollButtonSize.Width, maxSize.Height);

                    tabArea.X = bounds.Left + nearScrollButtonBounds.Width;
                    tabArea.Width = bounds.Width - nearScrollButtonBounds.Width - farScrollButtonBounds.Width;
                    maxViewOffset = totalTabWidth - tabArea.Width;
                }
                else if ((tabLocation & TabLocation.Left) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - nearScrollButtonSize.Height, maxSize.Width, nearScrollButtonSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, maxSize.Width, farScrollButtonSize.Height);

                    tabArea.Y = bounds.Top + nearScrollButtonBounds.Height;
                    tabArea.Height = bounds.Height - nearScrollButtonBounds.Height - farScrollButtonBounds.Height;
                    maxViewOffset = totalTabHeight - tabArea.Height;
                }
                else
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, maxSize.Width, nearScrollButtonSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - farScrollButtonSize.Height, maxSize.Width, farScrollButtonSize.Height);

                    tabArea.Y = bounds.Top + nearScrollButtonBounds.Height;
                    tabArea.Height = bounds.Height - nearScrollButtonBounds.Height - farScrollButtonBounds.Height;
                    maxViewOffset = totalTabHeight - tabArea.Height;
                }
            }
            else
            {
                nearScrollButtonBounds = Rectangle.Empty;
                farScrollButtonBounds = Rectangle.Empty;
            }
            CheckViewOffset();

            // update display area
            bounds = ClientRectangle;
            if (BorderStyle != BorderStyle.None)
                bounds.Inflate(-1, -1);

            if ((tabLocation & TabLocation.Top) != TabLocation.None)
                displayArea = new Rectangle(bounds.Left, tabArea.Bottom, bounds.Width, bounds.Height - tabArea.Height + 1);
            else if ((tabLocation & TabLocation.Bottom) != TabLocation.None)
                displayArea = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - tabArea.Height + 1);
            else if ((tabLocation & TabLocation.Left) != TabLocation.None)
                displayArea = new Rectangle(TabArea.Right, bounds.Top, bounds.Width - tabArea.Width + 1, bounds.Height);
            else
                displayArea = new Rectangle(bounds.Left, bounds.Top, bounds.Width - tabArea.Width + 1, bounds.Height);

            // raise the layout event
            LayoutTabsEventArgs eLayout = new LayoutTabsEventArgs(tabArea, displayArea, nearScrollButtonBounds, farScrollButtonBounds);
            OnLayoutTabs(eLayout);

            // recalculate view offset in case layout bounds were modified in the event
            tabArea = eLayout.TabAreaBounds;
            displayArea = eLayout.DisplayAreaBounds;
            nearScrollButtonBounds = eLayout.NearScrollButtonBounds;
            farScrollButtonBounds = eLayout.FarScrollButtonBounds;
            // do we need to show scroll buttons?
            maxViewOffset = Math.Max(0, horizontal ? totalTabWidth - tabArea.Width : totalTabHeight - tabArea.Height);
            scrollButtons = (maxViewOffset > 0);
            CheckViewOffset();

            // place tabs
            if (horizontal)
            {
                if (scrollButtons || (tabLocation & TabLocation.Near) != TabLocation.None)
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left + viewOffset, tabArea.Top, tabSizes[0].Width, tabSizes[0].Height);
                else if ((tabLocation & TabLocation.Far) != TabLocation.None)
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left + (tabArea.Width - totalTabWidth), tabArea.Top, tabSizes[0].Width, tabSizes[0].Height);
                else
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left + (tabArea.Width - totalTabWidth) / 2, tabArea.Top, tabSizes[0].Width, tabSizes[0].Height);

                for (int i = 1; i < Tabs.Count; i++)
                {
                    Point tabLocation = Tabs[i - 1].TabBounds.Location;
                    tabLocation.Offset(Tabs[i - 1].TabBounds.Width, 0);
                    Tabs[i].TabBounds = new Rectangle(tabLocation, tabSizes[i]);
                }
            }
            else
            {
                if (scrollButtons || (tabLocation & TabLocation.Near) != TabLocation.None)
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left, tabArea.Top - viewOffset, tabSizes[0].Width, tabSizes[0].Height);
                else if ((tabLocation & TabLocation.Far) != TabLocation.None)
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left, tabArea.Top + (tabArea.Height - totalTabHeight), tabSizes[0].Width, tabSizes[0].Height);
                else
                    Tabs[0].TabBounds = new Rectangle(tabArea.Left, tabArea.Top + (tabArea.Height - totalTabHeight) / 2, tabSizes[0].Width, tabSizes[0].Height);

                for (int i = 1; i < Tabs.Count; i++)
                {
                    Point tabLocation = Tabs[i - 1].TabBounds.Location;
                    tabLocation.Offset(0, Tabs[i - 1].TabBounds.Height);
                    Tabs[i].TabBounds = new Rectangle(tabLocation, tabSizes[i]);
                }
            }
        }
        #endregion
    }
}
