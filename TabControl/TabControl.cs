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
        #region Virtual Functions for Events
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

        protected internal virtual void OnNearScrollButtonClick(EventArgs e)
        {
            bool horizontal = IsHorizontal;

            for (int i = 0; i < Tabs.Count; i++)
            {
                var tab = Tabs[i];
                var tabBounds = GetTabBounds(tab);
                // find the first invisible tab
                if (horizontal && tabBounds.Left >= TabArea.Left)
                {
                    // scroll the view to show it
                    ViewOffset += tabBounds.Width;
                    break;
                }
                else if (!horizontal && tabBounds.Top >= TabArea.Top)
                {
                    // scroll the view to show it
                    ViewOffset += tabBounds.Height;
                    break;
                }
            }

            Invalidate();

            NearScrollButtonClick?.Invoke(this, e);
        }
        protected internal virtual void OnFarScrollButtonClick(EventArgs e)
        {
            bool horizontal = IsHorizontal;

            for (int i = 0; i < Tabs.Count; i++)
            {
                var tab = Tabs[i];
                var tabBounds = GetTabBounds(tab);
                // find the first visible tab
                if (horizontal && tabBounds.Left >= TabArea.Left)
                {
                    // scroll the view to hide it
                    ViewOffset -= tabBounds.Width;
                    break;
                }
                else if (!horizontal && tabBounds.Top >= TabArea.Top)
                {
                    // scroll the view to hide it
                    ViewOffset -= tabBounds.Height;
                    break;
                }
            }

            Invalidate();

            FarScrollButtonClick?.Invoke(this, e);
        }

        protected internal virtual void OnMeasureTab(MeasureTabEventArgs e)
        {
            int width = 0;
            int height = 0;
            var hasIcon = e.Tab.Icon != null;
            var hasText = !string.IsNullOrEmpty(e.Tab.Text);

            if (hasIcon)
            {
                width = width + e.Tab.Icon.Width;
                height = Math.Max(height, e.Tab.Icon.Height);
            }

            if (hasText)
            {
                Size size = TextRenderer.MeasureText(e.Tab.Text, e.Tab.Font);
                width = width + size.Width;
                height = Math.Max(height, size.Height);
            }

            if (hasIcon && hasText)
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
        protected internal virtual void OnMeasureScrollButton(MeasureEventArgs e)
        {
            if (IsHorizontal)
                e.Size = LeftArrowImage.Size.Max(RightArrowImage.Size) + TabPadding.Size;
            else
                e.Size = DownArrowImage.Size.Max(UpArrowImage.Size) + TabPadding.Size;

            MeasureScrollButton?.Invoke(this, e);
        }
        protected internal virtual void OnLayoutTabs(LayoutTabsEventArgs e) { LayoutTabs?.Invoke(this, e); }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a tab is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab is clicked.")]
        public event EventHandler<TabMouseEventArgs> TabClick;

        /// <summary>
        /// Occurs when a tab is double clicked by the mouse.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a tab is double clicked by the mouse.")]
        public event EventHandler<TabMouseEventArgs> TabDoubleClick;

        /// <summary>
        /// Occurs when a close tab button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when a close tab button is clicked.")]
        public event EventHandler<CancelTabEventArgs> CloseTabButtonClick;

        /// <summary>
        /// Occurs when the near scroll button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the near scroll button is clicked.")]
        public event EventHandler<EventArgs> NearScrollButtonClick;

        /// <summary>
        /// Occurs when the far scroll button is clicked.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the far scroll button is clicked.")]
        public event EventHandler<EventArgs> FarScrollButtonClick;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and a mouse button is pressed.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and a mouse button is pressed.")]
        public event EventHandler<TabMouseEventArgs> TabMouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and a mouse button is released.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and a mouse button is released.")]
        public event EventHandler<TabMouseEventArgs> TabMouseUp;

        /// <summary>
        /// Occurs when the mouse pointer is moved over a tab.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is moved over a tab.")]
        public event EventHandler<TabMouseEventArgs> TabMouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over a tab and the mouse wheel is rotated.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the mouse pointer is over a tab and the mouse wheel is rotated.")]
        public event EventHandler<TabMouseEventArgs> TabMouseWheel;

        /// <summary>
        /// Occurs when the size of a tab needs to be determined.
        /// </summary>
        [Category("Layout"), Description("Occurs when the size of a tab needs to be determined.")]
        public event EventHandler<MeasureTabEventArgs> MeasureTab;

        /// <summary>
        /// Occurs when the size of the scroll buttons needs to be determined.
        /// </summary>
        [Category("Layout"), Description("Occurs when the size of the scroll buttons needs to be determined.")]
        public event EventHandler<MeasureEventArgs> MeasureScrollButton;

        /// <summary>
        /// Occurs when the layout of all interface elements are calculated.
        /// </summary>
        [Category("Layout"), Description("Occurs when the layout of all interface elements are calculated.")]
        public event EventHandler<LayoutTabsEventArgs> LayoutTabs;
        #endregion

        #region Member Variables
        internal Tab hoveredTab = null;
        internal Tab mouseDownTab = null;
        internal bool hoveredButton = false;
        internal bool mouseDownButton = false;
        internal ScrollButton hoveredScrollButton = ScrollButton.None;
        internal ScrollButton mouseDownScrollButton = ScrollButton.None;

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
        private bool showCloseTabButtons = false;
        private int contentSpacing = 3;

        private int viewOffset = 0;
        private int minViewOffset = 0;

        private TabControlRenderer renderer;

        private Image defaultCloseImage = null;
        private Image closeImage = null;
        private Image defaultLeftArrowImage = null;
        private Image leftArrowImage = null;
        private Image defaultRightArrowImage = null;
        private Image rightArrowImage = null;
        private Image defaultUpArrowImage = null;
        private Image upArrowImage = null;
        private Image defaultDownArrowImage = null;
        private Image downArrowImage = null;
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
        public Size TabSize { get => tabSize; set { tabSize = value; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets the padding between the contents a tab and its borders.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(Padding), "4, 4, 4, 4")]
        [Description("Gets or sets the padding between the contents a tab and its borders.")]
        public Padding TabPadding { get => tabPadding; set { tabPadding = value; UpdateTabLayout(); } }

        /// <summary>
        /// Gets or sets the location of tabs.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabLocation.TopLeft)]
        [Description("Gets or sets the location of tabs.")]
        [Editor(typeof(TabLocationEditor), typeof(UITypeEditor))]
        public TabLocation TabLocation { get => tabLocation; set { tabLocation = value; ViewOffset = 0; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets the sizing mode of tabs.
        /// </summary>
        [Category("Appearance"), DefaultValue(TabSizing.AutoFit)]
        [Description("Gets or sets the sizing mode of tabs.")]
        public TabSizing TabSizing { get => tabSizing; set { tabSizing = value; ViewOffset = 0; UpdateTabLayout(); UpdatePages(); } }
        /// <summary>
        /// Gets or sets the alignment of tab text.
        /// </summary>
        [Category("Appearance"), DefaultValue(Alignment.Near)]
        [Description("Gets or sets the alignment of tab text.")]
        public Alignment ContentAlignment { get => contentAlignment; set { contentAlignment = value; UpdateTabLayout(); } }
        /// <summary>
        /// Gets or sets the direction of tab texts.
        /// </summary>
        [Category("Appearance"), DefaultValue(TextDirection.Right)]
        [Description("Gets or sets the direction of tab texts.")]
        [Editor(typeof(TextDirectionEditor), typeof(UITypeEditor))]
        public TextDirection TextDirection { get => textDirection; set { textDirection = value; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets the image of close buttons.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of close buttons.")]
        public Image CloseTabImage { get => (closeImage ?? defaultCloseImage); set { closeImage = value; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets whether to show close tab buttons.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        [Description("Gets or sets whether to show close tab buttons.")]
        public bool ShowCloseTabButtons { get => showCloseTabButtons; set { showCloseTabButtons = value; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets the spacing between tab contents (i.e. icon, text and close button).
        /// </summary>
        [Category("Appearance"), DefaultValue(3)]
        [Description("Gets or sets the spacing between tab contents (i.e. icon, text and close button).")]
        public int ContentSpacing { get => contentSpacing; set { contentSpacing = value; UpdateTabLayout(); UpdatePages(); } }

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
        /// Gets or sets the zero-based index of the selected tab.
        /// </summary>
        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the zero-based index of the selected tab.")]
        public override int SelectedIndex { get => base.SelectedIndex; set => base.SelectedIndex = value; }

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
        /// Gets or sets the font associated with the control.
        /// </summary>
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the font associated with the control.")]
        public override Font Font { get => base.Font; set { base.Font = value; UpdateTabLayout(); UpdatePages(); } }

        /// <summary>
        /// Gets or sets the image of a scroll button pointing left.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of a scroll button pointing left.")]
        public Image LeftArrowImage { get => (leftArrowImage ?? defaultLeftArrowImage); set { leftArrowImage = value; UpdateTabLayout(); UpdatePages(); } }
        /// <summary>
        /// Gets or sets the image of a scroll button pointing right.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of a scroll button pointing right.")]
        public Image RightArrowImage { get => (rightArrowImage ?? defaultRightArrowImage); set { rightArrowImage = value; UpdateTabLayout(); UpdatePages(); } }
        /// <summary>
        /// Gets or sets the image of a scroll button pointing up.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of a scroll button pointing up.")]
        public Image UpArrowImage { get => (upArrowImage ?? defaultUpArrowImage); set { upArrowImage = value; UpdateTabLayout(); UpdatePages(); } }
        /// <summary>
        /// Gets or sets the image of a scroll button pointing down.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the image of a scroll button pointing down.")]
        public Image DownArrowImage { get => (downArrowImage ?? defaultDownArrowImage); set { downArrowImage = value; UpdateTabLayout(); UpdatePages(); } }

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

        /// <summary>
        /// Determines whether the toolbar is horizontal (at top or bottom) or vertical (left or right).
        /// </summary>
        [Browsable(false)]
        protected internal bool IsHorizontal => (TabLocation & TabLocation.Top) != TabLocation.None || (TabLocation & TabLocation.Bottom) != TabLocation.None;

        /// <summary>
        /// Gets the value of the scrollable view offset.
        /// </summary>
        [Browsable(false)]
        protected internal int ViewOffset
        {
            get => viewOffset;
            set
            {
                viewOffset = value;
                /// make sure that the view offset is within the scrollable range
                if (ScrollButtons)
                    viewOffset = Math.Max(MinViewOffset, Math.Min(0, viewOffset));
                else
                    viewOffset = 0;
            }
        }

        /// <summary>
        /// Gets the mininum value of the scrollable view offset.
        /// </summary>
        [Browsable(false)]
        protected internal int MinViewOffset
        {
            get => minViewOffset;
            private set
            {
                minViewOffset = Math.Min(0, value);
                // force an update of the ViewOffset
                ViewOffset = viewOffset;
            }
        }

        /// <summary>
        /// Gets whether the scroll buttons are visible.
        /// </summary>
        [Browsable(false)]
        public bool ScrollButtons => (MinViewOffset < 0);
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControl"/> class.
        /// </summary>
        public TabControl()
        {
            defaultCloseImage = Properties.Resources.CloseImage;

            defaultLeftArrowImage = Properties.Resources.LeftArrow;
            defaultRightArrowImage = Properties.Resources.RightArrow;
            defaultUpArrowImage = Properties.Resources.UpArrow;
            defaultDownArrowImage = Properties.Resources.DownArrow;

            Tabs = new TabCollection(this);
            renderer = new TabControlRenderer(this);
            UpdateTabLayout();
        }
        #endregion

        #region Property Defaults
        internal bool ShouldSerializeCloseTabImage() => (closeImage != null);
        internal void ResetCloseTabImage() => closeImage = null;

        internal bool ShouldSerializeLeftArrowImage() => (leftArrowImage != null);
        internal void ResetLeftArrowTabImage() => leftArrowImage = null;

        internal bool ShouldSerializeRightArrowImage() => (rightArrowImage != null);
        internal void ResetRightArrowTabImage() => rightArrowImage = null;

        internal bool ShouldSerializeUpArrowImage() => (upArrowImage != null);
        internal void ResetUpArrowTabImage() => upArrowImage = null;

        internal bool ShouldSerializeDownArrowImage() => (downArrowImage != null);
        internal void ResetDownArrowTabImage() => downArrowImage = null;
        #endregion

        #region Instance Methods
        /// <summary>
        /// Performs a hit test with given coordinates and returns the
        /// tab which contains the given point.
        /// </summary>
        /// <param name="pt">Hit test coordinates</param>
        /// <returns>The tab which contains the given point; or null
        /// if none of the tabs contains the given point.</returns>
        public HitResult PerformHitTest(Point pt)
        {
            if (NearScrollButtonBounds.Contains(pt))
                return new HitResult() { NearScrollButton = true };
            else if (FarScrollButtonBounds.Contains(pt))
                return new HitResult() { FarScrollButton = true };

            foreach (var tab in Tabs)
            {
                if (GetTabBounds(tab).Contains(pt))
                {
                    var hit = new HitResult() { Tab = true, HitTab = tab };
                    if (GetCloseButtonBounds(tab).Contains(pt))
                        hit.CloseButton = true;
                    return hit;
                }
            }

            return HitResult.Empty;
        }

        /// <summary>
        /// Scrolls the control to make sure that the given tab is visible.
        /// </summary>
        /// <param name="tab">The tab to make visible.</param>
        public void EnsureVisible(Tab tab)
        {
            if (tab == null)
            {
                ViewOffset = 0;
                return;
            }

            var tabBounds = GetTabBounds(tab);

            if ((tabLocation & TabLocation.Top) != TabLocation.None || (tabLocation & TabLocation.Bottom) != TabLocation.None)
            {
                // horizontal
                if (tabBounds.Left < TabArea.Left)
                {
                    ViewOffset -= tabBounds.Left - TabArea.Left;
                }
                else if (tabBounds.Right > TabArea.Right)
                {
                    ViewOffset -= tabBounds.Right - TabArea.Right;
                }
            }
            else
            {
                // vertical
                if (tabBounds.Top < TabArea.Top)
                {
                    ViewOffset -= tabBounds.Top - TabArea.Top;
                }
                else if (tabBounds.Bottom > TabArea.Bottom)
                {
                    ViewOffset -= tabBounds.Bottom - TabArea.Bottom;
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Gets the bounding rectangle of the tab in client coordinates.
        /// </summary>
        /// <param name="tab">The tab to calculate bounds for.</param>
        public Rectangle GetTabBounds(Tab tab) => GetViewOffsetBounds(tab.TabBounds);

        /// <summary>
        /// Gets the bounding rectangle of the tab in client coordinates.
        /// </summary>
        /// <param name="tab">The tab to calculate bounds for.</param>
        public Rectangle GetIconBounds(Tab tab) => GetViewOffsetBounds(tab.IconBounds);

        /// <summary>
        /// Gets the bounding rectangle of the tab in client coordinates.
        /// </summary>
        /// <param name="tab">The tab to calculate bounds for.</param>
        public Rectangle GetCloseButtonBounds(Tab tab) => GetViewOffsetBounds(tab.CloseButtonBounds);

        /// <summary>
        /// Gets the bounding rectangle of the tab in client coordinates.
        /// </summary>
        /// <param name="tab">The tab to calculate bounds for.</param>
        public Rectangle GetTextBounds(Tab tab) => GetViewOffsetBounds(tab.TextBounds);
        #endregion

        #region Overriden Methods
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            UpdateTabLayout();
            UpdatePages();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateTabLayout();
            UpdatePages();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var oldHoveredTab = hoveredTab;
            var oldHoveredButton = hoveredButton;
            var oldHoveredScrollButton = hoveredScrollButton;

            var hit = PerformHitTest(e.Location);
            hoveredTab = hit.HitTab;
            hoveredButton = hit.CloseButton;
            hoveredScrollButton = hit.NearScrollButton ? ScrollButton.Near : hit.FarScrollButton ? ScrollButton.Far : ScrollButton.None;

            if (hoveredTab != null)
                OnTabMouseMove(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));

            if (!ReferenceEquals(oldHoveredTab, hoveredTab) || oldHoveredButton != hoveredButton || oldHoveredScrollButton != hoveredScrollButton)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hoveredTab != null || hoveredScrollButton != ScrollButton.None)
            {
                hoveredTab = null;
                hoveredButton = false;
                hoveredScrollButton = ScrollButton.None;
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
                    mouseDownButton = GetCloseButtonBounds(mouseDownTab).Contains(e.Location);
                }
                OnTabMouseDown(new TabMouseEventArgs(hoveredTab, e.Button, e.Clicks, e.Delta, e.Location));
                if (oldmouseDownTab != mouseDownTab || oldMouseDownButton != mouseDownButton)
                    Invalidate();
            }
            else if (hoveredScrollButton != ScrollButton.None)
            {
                var oldmouseDownScrollButton = mouseDownScrollButton;
                mouseDownScrollButton = hoveredScrollButton;
                if (oldmouseDownScrollButton != mouseDownScrollButton)
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

            if (mouseDownTab != null && !ReferenceEquals(mouseDownTab, SelectedPage) && e.Button == MouseButtons.Left)
                SelectedPage = mouseDownTab;

            if (mouseDownButton != false && e.Button == MouseButtons.Left)
                OnCloseTabButtonClick(new CancelTabEventArgs(mouseDownTab));

            if (mouseDownScrollButton == ScrollButton.Near && (GetNearScrollButtonState() & ItemState.Disabled) == ItemState.Inactive && e.Button == MouseButtons.Left)
                OnNearScrollButtonClick(EventArgs.Empty);
            else if (mouseDownScrollButton == ScrollButton.Far && (GetFarScrollButtonState() & ItemState.Disabled) == ItemState.Inactive && e.Button == MouseButtons.Left)
                OnFarScrollButtonClick(EventArgs.Empty);

            var oldmouseDownTab = mouseDownTab;
            mouseDownTab = null;
            hoveredButton = false;
            mouseDownButton = false;
            var oldmouseDownScrollButton = mouseDownScrollButton;
            mouseDownScrollButton = ScrollButton.None;
            if (oldmouseDownTab != null || oldmouseDownScrollButton != ScrollButton.None)
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
            ((Tab)e.Page).ContentsChanged += Tab_ContentsChanged;

            base.OnPageAdded(e);

            UpdateTabLayout();
        }

        protected override void OnPageRemoved(PageEventArgs e)
        {
            ((Tab)e.Page).ContentsChanged -= Tab_ContentsChanged;

            base.OnPageRemoved(e);

            UpdateTabLayout();
        }

        protected override void OnPageChanged(PageChangedEventArgs e)
        {
            base.OnPageChanged(e);

            EnsureVisible(SelectedTab);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets the visual state of a tab.
        /// </summary>
        /// <param name="tab">The tab to check.</param>
        protected internal ItemState GetTabState(Tab tab)
        {
            var state = ItemState.Inactive;

            // hot
            if (ReferenceEquals(hoveredTab, tab)) state |= ItemState.Hot;
            // pressed
            if (ReferenceEquals(mouseDownTab, tab)) state |= ItemState.Pressed;
            // focused
            if (Parent.Focused && (ReferenceEquals(SelectedPage, tab))) state |= ItemState.Focused;

            return state;
        }

        /// <summary>
        /// Gets the visual state of the close tab button.
        /// </summary>
        /// <param name="tab">The tab to check.</param>
        protected internal ItemState GetTabCloseButtonState(Tab tab)
        {
            var state = ItemState.Inactive;

            // hot
            if (ReferenceEquals(hoveredTab, tab) && hoveredButton) state |= ItemState.Hot;
            // pressed
            if (ReferenceEquals(mouseDownTab, tab) && mouseDownButton) state |= ItemState.Pressed;
            // focused
            if (Parent.Focused && (ReferenceEquals(SelectedPage, tab))) state |= ItemState.Focused;

            return state;
        }

        /// <summary>
        /// Gets the visual state of the near scroll button.
        /// </summary>
        protected internal ItemState GetNearScrollButtonState()
        {
            var state = ItemState.Inactive;

            if (hoveredScrollButton == ScrollButton.Near)
                state |= ItemState.Hot;
            if (mouseDownScrollButton == ScrollButton.Near)
                state |= ItemState.Pressed;
            if (ViewOffset == 0)
                state |= ItemState.Disabled;

            return state;
        }

        /// <summary>
        /// Gets the visual state of the far scroll button.
        /// </summary>
        protected internal ItemState GetFarScrollButtonState()
        {
            var state = ItemState.Inactive;

            if (hoveredScrollButton == ScrollButton.Far)
                state |= ItemState.Hot;
            if (mouseDownScrollButton == ScrollButton.Far)
                state |= ItemState.Pressed;
            if (ViewOffset == MinViewOffset)
                state |= ItemState.Disabled;

            return state;
        }

        private void Tab_ContentsChanged(object sender, EventArgs e)
        {
            UpdateTabLayout();
        }

        /// <summary>
        /// Updates the layout of all tabs.
        /// </summary>
        protected internal void UpdateTabLayout()
        {
            if (!IsHandleCreated)
                return;

            tabArea = Rectangle.Empty;
            displayArea = ClientRectangle;
            if (BorderStyle != BorderStyle.None)
                displayArea.Inflate(-1, -1);

            if (Pages.Count == 0) return;

            bool horizontal = IsHorizontal;
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

            // total tab size
            int totalTabWidth = tabSizes.Sum(t => t.Width);
            int totalTabHeight = tabSizes.Sum(t => t.Height);

            // maximum tab size
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
            int scrollDist = Math.Max(0, horizontal ? totalTabWidth - tabArea.Width : totalTabHeight - tabArea.Height);
            if (scrollDist > 0)
            {
                // update tab area to account for scroll buttons
                MeasureEventArgs eScroll = new MeasureEventArgs(Size.Empty);
                OnMeasureScrollButton(eScroll);
                var scrollButtonSize = eScroll.Size;

                if ((tabLocation & TabLocation.Top) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, scrollButtonSize.Width, maxSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Right - scrollButtonSize.Width, bounds.Top, scrollButtonSize.Width, maxSize.Height);

                    tabArea.X = bounds.Left + nearScrollButtonBounds.Width;
                    tabArea.Width = bounds.Width - nearScrollButtonBounds.Width - farScrollButtonBounds.Width;
                }
                else if ((tabLocation & TabLocation.Bottom) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - scrollButtonSize.Height, scrollButtonSize.Width, maxSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Right - scrollButtonSize.Width, bounds.Bottom - scrollButtonSize.Height, scrollButtonSize.Width, maxSize.Height);

                    tabArea.X = bounds.Left + nearScrollButtonBounds.Width;
                    tabArea.Width = bounds.Width - nearScrollButtonBounds.Width - farScrollButtonBounds.Width;
                }
                else if ((tabLocation & TabLocation.Left) != TabLocation.None)
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, maxSize.Width, scrollButtonSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - scrollButtonSize.Height, maxSize.Width, scrollButtonSize.Height);

                    tabArea.Y = bounds.Top + nearScrollButtonBounds.Height;
                    tabArea.Height = bounds.Height - nearScrollButtonBounds.Height - farScrollButtonBounds.Height;
                }
                else
                {
                    nearScrollButtonBounds = new Rectangle(bounds.Left, bounds.Bottom - scrollButtonSize.Height, maxSize.Width, scrollButtonSize.Height);
                    farScrollButtonBounds = new Rectangle(bounds.Left, bounds.Top, maxSize.Width, scrollButtonSize.Height);

                    tabArea.Y = bounds.Top + nearScrollButtonBounds.Height;
                    tabArea.Height = bounds.Height - nearScrollButtonBounds.Height - farScrollButtonBounds.Height;
                }
            }
            else
            {
                nearScrollButtonBounds = Rectangle.Empty;
                farScrollButtonBounds = Rectangle.Empty;
            }

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

            // get bounds from event argument in case layout bounds were modified in the event
            tabArea = eLayout.TabAreaBounds;
            displayArea = eLayout.DisplayAreaBounds;
            nearScrollButtonBounds = eLayout.NearScrollButtonBounds;
            farScrollButtonBounds = eLayout.FarScrollButtonBounds;

            // update tab sizes in stretch mode in case layout bounds are modified
            if (TabSizing == TabSizing.Stretch)
            {
                for (int i = 0; i < Tabs.Count; i++)
                {
                    if (horizontal)
                        tabSizes[i].Width = Math.Max(10, (int)(bounds.Width / (float)Tabs.Count));
                    else
                        tabSizes[i].Height = Math.Max(10, (int)(bounds.Height / (float)Tabs.Count));
                }

                // total tab size
                totalTabWidth = tabSizes.Sum(t => t.Width);
                totalTabHeight = tabSizes.Sum(t => t.Height);

                // fit last tab to bounds
                if (horizontal)
                    tabSizes[tabSizes.Length - 1].Width = tabArea.Width - (totalTabWidth - tabSizes[tabSizes.Length - 1].Width);
                else
                    tabSizes[tabSizes.Length - 1].Height = tabArea.Height - (totalTabHeight - tabSizes[tabSizes.Length - 1].Height);
            }

            // total tab size
            totalTabWidth = tabSizes.Sum(t => t.Width);
            totalTabHeight = tabSizes.Sum(t => t.Height);

            // the minimum view offset that we can scroll to
            MinViewOffset = -Math.Max(0, horizontal ? totalTabWidth - tabArea.Width : totalTabHeight - tabArea.Height);

            // place tabs
            if (horizontal)
            {
                if (ScrollButtons || (tabLocation & TabLocation.Near) != TabLocation.None)
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left, tabArea.Top, tabSizes[0].Width, tabSizes[0].Height));
                else if ((tabLocation & TabLocation.Far) != TabLocation.None)
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left + (tabArea.Width - totalTabWidth), tabArea.Top, tabSizes[0].Width, tabSizes[0].Height));
                else
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left + (tabArea.Width - totalTabWidth) / 2, tabArea.Top, tabSizes[0].Width, tabSizes[0].Height));

                for (int i = 1; i < Tabs.Count; i++)
                {
                    Point tabLocation = Tabs[i - 1].TabBounds.Location;
                    tabLocation.Offset(Tabs[i - 1].TabBounds.Width, 0);
                    UpdateContentBounds(Tabs[i], new Rectangle(tabLocation, tabSizes[i]));
                }
            }
            else
            {
                if (ScrollButtons || (tabLocation & TabLocation.Near) != TabLocation.None)
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left, tabArea.Top, tabSizes[0].Width, tabSizes[0].Height));
                else if ((tabLocation & TabLocation.Far) != TabLocation.None)
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left, tabArea.Top + (tabArea.Height - totalTabHeight), tabSizes[0].Width, tabSizes[0].Height));
                else
                    UpdateContentBounds(Tabs[0], new Rectangle(tabArea.Left, tabArea.Top + (tabArea.Height - totalTabHeight) / 2, tabSizes[0].Width, tabSizes[0].Height));

                for (int i = 1; i < Tabs.Count; i++)
                {
                    Point tabLocation = Tabs[i - 1].TabBounds.Location;
                    tabLocation.Offset(0, Tabs[i - 1].TabBounds.Height);
                    UpdateContentBounds(Tabs[i], new Rectangle(tabLocation, tabSizes[i]));
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Updates the bounding rectangles of tab contents.
        /// </summary>
        /// <param name="tab">The tab to update.</param>
        /// <param name="tabBounds">The bounding rectangle of the tab.</param>
        protected internal void UpdateContentBounds(Tab tab, Rectangle tabBounds)
        {
            var hasIcon = tab.Icon != null;
            var hasText = !string.IsNullOrEmpty(tab.Text);

            tab.TabBounds = tabBounds;
            var iconBounds = Rectangle.Empty;
            var closeButtonBounds = Rectangle.Empty;
            var textBounds = Rectangle.Empty;

            tabBounds = tabBounds.GetDeflated(TabPadding);
            int width = (TextDirection == TextDirection.Right ? tabBounds.Width : tabBounds.Height);
            int height = (TextDirection == TextDirection.Right ? tabBounds.Height : tabBounds.Width);

            Size iconSize = hasIcon ? tab.Icon.Size : Size.Empty;
            Size textSize = hasText ? TextRenderer.MeasureText(tab.Text, tab.Font) : Size.Empty;
            Size buttonSize = ShowCloseTabButtons ? CloseTabImage.Size : Size.Empty;

            int availableIconAndTextWidth = width - (ShowCloseTabButtons ? buttonSize.Width + ContentSpacing : 0);
            int availableTextWidth = availableIconAndTextWidth - (hasIcon ? iconSize.Width + ContentSpacing : 0);

            if (hasIcon)
                iconBounds = new Rectangle(0, (height - iconSize.Height) / 2, iconSize.Width, iconSize.Height);

            if (ShowCloseTabButtons)
                closeButtonBounds = new Rectangle(width - buttonSize.Width, (height - buttonSize.Height) / 2, buttonSize.Width, buttonSize.Height);

            if (hasText)
                textBounds = new Rectangle(0, (height - textSize.Height) / 2,
                    Math.Min(availableTextWidth, textSize.Width), textSize.Height);

            int actualIconAndTextWidth = (hasIcon ? iconBounds.Width + ContentSpacing : 0) + textBounds.Width;

            if (ContentAlignment == Alignment.Near)
            {
                if (hasText && hasIcon)
                    textBounds = textBounds.GetOffset(iconSize.Width + ContentSpacing, 0);
            }
            else if (ContentAlignment == Alignment.Center)
            {
                if (hasIcon)
                    iconBounds = iconBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2, 0);

                if (hasText)
                    textBounds = textBounds.GetOffset((availableIconAndTextWidth - actualIconAndTextWidth) / 2 + (hasIcon ? iconSize.Width + ContentSpacing : 0), 0);
            }
            else
            {
                if (hasIcon)
                    iconBounds = iconBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth, 0);

                if (hasText)
                    textBounds = textBounds.GetOffset(availableIconAndTextWidth - actualIconAndTextWidth + (hasIcon ? iconSize.Width + ContentSpacing : 0), 0);
            }

            tab.IconBounds = iconBounds.GetRotated(tabBounds, TextDirection).EnsureMinSize(1, 1);
            tab.CloseButtonBounds = closeButtonBounds.GetRotated(tabBounds, TextDirection).EnsureMinSize(1, 1);
            tab.TextBounds = textBounds.GetRotated(tabBounds, TextDirection).EnsureMinSize(1, 1);
        }

        /// <summary>
        /// Offsets the given bounding rectangle with the view offset.
        /// </summary>
        /// <param name="bounds">The bounds to offset.</param>
        protected internal Rectangle GetViewOffsetBounds(Rectangle bounds)
        {
            if (!ScrollButtons)
                return bounds;
            else if (IsHorizontal)
                return bounds.GetOffset(ViewOffset, 0);
            else
                return bounds.GetOffset(0, ViewOffset);
        }
        #endregion
    }
}
