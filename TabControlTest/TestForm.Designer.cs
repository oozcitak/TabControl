namespace TabControlTest
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new Manina.Windows.Forms.TabControl();
            this.page1 = new Manina.Windows.Forms.Page();
            this.page2 = new Manina.Windows.Forms.Page();
            this.page3 = new Manina.Windows.Forms.Page();
            this.page4 = new Manina.Windows.Forms.Page();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.page1);
            this.tabControl1.Controls.Add(this.page2);
            this.tabControl1.Controls.Add(this.page3);
            this.tabControl1.Controls.Add(this.page4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // page1
            // 
            this.page1.Location = new System.Drawing.Point(1, 24);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(798, 425);
            this.page1.Text = "Page 1";
            // 
            // page2
            // 
            this.page2.Location = new System.Drawing.Point(1, 24);
            this.page2.Name = "page2";
            this.page2.Size = new System.Drawing.Size(393, 263);
            this.page2.Text = "Page 2";
            // 
            // page3
            // 
            this.page3.Location = new System.Drawing.Point(1, 24);
            this.page3.Name = "page3";
            this.page3.Size = new System.Drawing.Size(393, 263);
            this.page3.Text = "Page 3";
            // 
            // page4
            // 
            this.page4.Location = new System.Drawing.Point(1, 24);
            this.page4.Name = "page4";
            this.page4.Size = new System.Drawing.Size(798, 425);
            this.page4.Text = "Page 4";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "TestForm";
            this.Text = "TabControl Test Form";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Manina.Windows.Forms.TabControl tabControl1;
        private Manina.Windows.Forms.Page page1;
        private Manina.Windows.Forms.Page page2;
        private Manina.Windows.Forms.Page page3;
        private Manina.Windows.Forms.Page page4;
    }
}

