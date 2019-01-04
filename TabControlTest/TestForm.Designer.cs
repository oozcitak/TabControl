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
            this.tab1 = new Manina.Windows.Forms.TabControl.Tab();
            this.tab2 = new Manina.Windows.Forms.TabControl.Tab();
            this.tab3 = new Manina.Windows.Forms.TabControl.Tab();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab1);
            this.tabControl1.Controls.Add(this.tab2);
            this.tabControl1.Controls.Add(this.tab3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(584, 261);
            this.tabControl1.TabIndex = 0;
            // 
            // tab1
            // 
            this.tab1.Icon = null;
            this.tab1.Location = new System.Drawing.Point(1, 21);
            this.tab1.Name = "tab1";
            this.tab1.Size = new System.Drawing.Size(582, 239);
            this.tab1.Text = "tab1";
            // 
            // tab2
            // 
            this.tab2.ForeColor = System.Drawing.Color.Red;
            this.tab2.Icon = null;
            this.tab2.Location = new System.Drawing.Point(1, 21);
            this.tab2.Name = "tab2";
            this.tab2.Size = new System.Drawing.Size(582, 239);
            this.tab2.Text = "tab2";
            // 
            // tab3
            // 
            this.tab3.Icon = null;
            this.tab3.Location = new System.Drawing.Point(1, 21);
            this.tab3.Name = "tab3";
            this.tab3.Size = new System.Drawing.Size(582, 239);
            this.tab3.Text = "tab3";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.tabControl1);
            this.Name = "TestForm";
            this.Text = "TabControl Test Form";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Manina.Windows.Forms.TabControl tabControl1;
        private Manina.Windows.Forms.TabControl.Tab tab1;
        private Manina.Windows.Forms.TabControl.Tab tab2;
        private Manina.Windows.Forms.TabControl.Tab tab3;
    }
}

