using System.Drawing;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Contains extension methods used in the assembly.
    /// </summary>
    internal static class ExtensionMethods
    {
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

        public static Point GetOffset(this Point pt, int dx, int dy)
        {
            Point opt = new Point(pt.X, pt.Y);
            opt.Offset(dx, dy);
            return opt;
        }

        public static void DrawRectangleFixed(this Graphics g, Pen pen, int x, int y, int width, int height)
        {
            g.DrawRectangle(pen, x, y, width - 1, height - 1);
        }

        public static void DrawRectangleFixed(this Graphics g, Pen pen, Rectangle rec)
        {
            g.DrawRectangleFixed(pen, rec.X, rec.Y, rec.Width, rec.Height);
        }
    }
}
