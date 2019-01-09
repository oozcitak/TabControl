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
            this.tab4 = new Manina.Windows.Forms.TabControl.Tab();
            this.tab5 = new Manina.Windows.Forms.TabControl.Tab();
            this.tab6 = new Manina.Windows.Forms.TabControl.Tab();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab1);
            this.tabControl1.Controls.Add(this.tab2);
            this.tabControl1.Controls.Add(this.tab3);
            this.tabControl1.Controls.Add(this.tab4);
            this.tabControl1.Controls.Add(this.tab5);
            this.tabControl1.Controls.Add(this.tab6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(584, 261);
            this.tabControl1.TabIndex = 0;
            // 
            // tab1
            // 
            this.tab1.Icon = global::TabControlTest.Properties.Resources.brick;
            this.tab1.Location = new System.Drawing.Point(1, 24);
            this.tab1.Name = "tab1";
            this.tab1.Size = new System.Drawing.Size(582, 236);
            this.tab1.Text = "tab1";
            // 
            // tab2
            // 
            this.tab2.ForeColor = System.Drawing.Color.Red;
            this.tab2.Icon = global::TabControlTest.Properties.Resources.cake;
            this.tab2.Location = new System.Drawing.Point(1, 21);
            this.tab2.Name = "tab2";
            this.tab2.Size = new System.Drawing.Size(582, 239);
            this.tab2.Text = "tab2";
            // 
            // tab3
            // 
            this.tab3.Icon = global::TabControlTest.Properties.Resources.cd;
            this.tab3.Location = new System.Drawing.Point(1, 21);
            this.tab3.Name = "tab3";
            this.tab3.Size = new System.Drawing.Size(582, 239);
            this.tab3.Text = "tab3";
            // 
            // tab4
            // 
            this.tab4.Icon = global::TabControlTest.Properties.Resources.clock;
            this.tab4.Location = new System.Drawing.Point(1, 24);
            this.tab4.Name = "tab4";
            this.tab4.Size = new System.Drawing.Size(582, 236);
            this.tab4.Text = "tab4";
            // 
            // tab5
            // 
            this.tab5.Icon = global::TabControlTest.Properties.Resources.camera;
            this.tab5.Location = new System.Drawing.Point(1, 24);
            this.tab5.Name = "tab5";
            this.tab5.Size = new System.Drawing.Size(582, 236);
            this.tab5.Text = "tab5";
            // 
            // tab6
            // 
            this.tab6.Icon = global::TabControlTest.Properties.Resources.calendar;
            this.tab6.Location = new System.Drawing.Point(1, 24);
            this.tab6.Name = "tab6";
            this.tab6.Size = new System.Drawing.Size(582, 236);
            this.tab6.Text = "tab6";
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
        private Manina.Windows.Forms.TabControl.Tab tab4;
        private Manina.Windows.Forms.TabControl.Tab tab5;
        private Manina.Windows.Forms.TabControl.Tab tab6;
    }
}

