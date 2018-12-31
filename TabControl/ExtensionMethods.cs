using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains extension methods used in the assembly.
    /// </summary>
    internal static class ExtensionMethods
    {
        #region Rectangle
        public static Point GetBottomLeft(this Rectangle r)
        {
            return new Point(r.Left, r.Bottom);
        }

        public static Point GetTopLeft(this Rectangle r)
        {
            return new Point(r.Left, r.Top);
        }

        public static Point GetBottomRight(this Rectangle r)
        {
            return new Point(r.Right, r.Bottom);
        }

        public static Point GetTopRight(this Rectangle r)
        {
            return new Point(r.Right, r.Top);
        }

        public static Rectangle GetInflated(this Rectangle rec, int dx, int dy)
        {
            return new Rectangle(rec.Left - dx, rec.Top - dy, rec.Width + 2 * dx, rec.Height + 2 * dy);
        }

        public static Rectangle GetInflated(this Rectangle rec, Size size)
        {
            return rec.GetInflated(size.Width, size.Height);
        }

        public static Rectangle GetInflated(this Rectangle rec, Padding padding)
        {
            return new Rectangle(rec.Left - padding.Left, rec.Top - padding.Top, rec.Width + padding.Horizontal, rec.Height + padding.Vertical);
        }

        public static Rectangle GetDeflated(this Rectangle rec, int dx, int dy)
        {
            return new Rectangle(rec.Left + dx, rec.Top + dy, rec.Width - 2 * dx, rec.Height - 2 * dy);
        }

        public static Rectangle GetDeflated(this Rectangle rec, Size size)
        {
            return rec.GetDeflated(size.Width, size.Height);
        }

        public static Rectangle GetDeflated(this Rectangle rec, Padding padding)
        {
            return new Rectangle(rec.Left + padding.Left, rec.Top + padding.Top, rec.Width - padding.Horizontal, rec.Height - padding.Vertical);
        }
        #endregion

        #region Point
        public static Point GetOffset(this Point pt, int dx, int dy)
        {
            return new Point(pt.X + dx, pt.Y + dy);
        }
        #endregion

        #region Graphics
        public static void DrawRectangleFixed(this Graphics g, Pen pen, int x, int y, int width, int height)
        {
            g.DrawRectangle(pen, x, y, width - 1, height - 1);
        }

        public static void DrawRectangleFixed(this Graphics g, Pen pen, Rectangle rec)
        {
            g.DrawRectangleFixed(pen, rec.X, rec.Y, rec.Width, rec.Height);
        }

        public static void DrawVerticalTextDown(this Graphics g, string text, Font font, Rectangle bounds, Color foreColor, Color backColor, TextFormatFlags flags)
        {
            var imageBounds = new Rectangle(0, 0, bounds.Height, bounds.Width);

            using (var image = new Bitmap(imageBounds.Width, imageBounds.Height, PixelFormat.Format32bppArgb))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                TextRenderer.DrawText(imageGraphics, text, font, imageBounds, foreColor, backColor, flags);
                // Rotate, translate and draw the image
                Point[] ptMap = new Point[] {
                    bounds.GetTopRight(),    // upper-left
                    bounds.GetBottomRight(), // upper-right
                    bounds.GetTopLeft(),     // lower-left
                };
                g.DrawImage(image, ptMap);
            }
        }

        public static void DrawVerticalTextUp(this Graphics g, string text, Font font, Rectangle bounds, Color foreColor, Color backColor, TextFormatFlags flags)
        {
            var imageBounds = new Rectangle(0, 0, bounds.Height, bounds.Width);

            using (var image = new Bitmap(imageBounds.Width, imageBounds.Height, PixelFormat.Format32bppArgb))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                TextRenderer.DrawText(imageGraphics, text, font, imageBounds, foreColor, backColor, flags);
                // Rotate, translate and draw the image
                Point[] ptMap = new Point[] {
                    bounds.GetBottomLeft(),  // upper-left
                    bounds.GetTopLeft(),     // upper-right
                    bounds.GetBottomRight(), // lower-left
                };
                g.DrawImage(image, ptMap);
            }
        }
        #endregion
    }
}
