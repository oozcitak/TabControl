namespace Manina.Windows.Forms
{
    public partial class TabControl
    {
        /// <summary>
        /// Contains the result of a hit test on the tab control.
        /// </summary>
        public struct HitResult
        {
            /// <summary>
            /// Gets whether a tab was hit.
            /// </summary>
            public bool Tab { get; internal set; }
            /// <summary>
            /// Gets whether a close button was hit.
            /// </summary>
            public bool CloseButton { get; internal set; }
            /// <summary>
            /// Gets whether the near scroll button was hit.
            /// </summary>
            public bool NearScrollButton { get; internal set; }
            /// <summary>
            /// Gets whether the far scroll button was hit.
            /// </summary>
            public bool FarScrollButton { get; internal set; }
            /// <summary>
            /// Gets the tab under the mouse cursor.
            /// </summary>
            public Tab HitTab { get; internal set; }

            public static HitResult Empty => new HitResult();
        }
    }
}
