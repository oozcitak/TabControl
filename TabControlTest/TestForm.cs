using System;
using System.Drawing;
using System.Windows.Forms;

namespace TabControlTest
{
    public partial class TestForm : Form
    {
        public Rectangle dragBoxFromMouseDown;

        public TestForm()
        {
            InitializeComponent();

            button1.MouseDown += Button1_MouseDown;
            button1.MouseMove += Button1_MouseMove;

            button2.AllowDrop = true;
            button2.DragOver += Button2_DragOver;
            button2.DragDrop += Button2_DragDrop;

            tabControl1.AllowDrop = true;
            tabControl1.DragOver += TabControl1_DragOver;
            tabControl1.DragDrop += TabControl1_DragDrop;

            tempControl1.AllowDrop = true;
            tempControl1.DragOver += TabControl1_DragOver;
            tempControl1.DragDrop += TabControl1_DragDrop;

        }

        private void TabControl1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("Drop!");
        }

        private void TabControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            Console.WriteLine("Drag Over {0}", sender);
        }

        private void Button2_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("Drop!");
        }

        private void Button2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
            Console.WriteLine("Drag Over");
        }

        private void Button1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left || (e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    

                    var dd = new DataObject();
                    dd.SetText("Hello World");
                    
                    DragDropEffects dropEffect = button1.DoDragDrop(
                        dd,
                        DragDropEffects.Copy | DragDropEffects.Move);

                }
            }
        }

        private void Button1_MouseDown(object sender, MouseEventArgs e)
        {
            Size dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being
            // at the center of the rectangle.
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                            e.Y - (dragSize.Height / 2)),
                                dragSize);

        }

        private void TestForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
