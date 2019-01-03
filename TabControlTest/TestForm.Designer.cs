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
            this.tab2 = new Manina.Windows.Forms.TabControl.Tab();
            this.tab4 = new Manina.Windows.Forms.TabControl.Tab();
            this.SuspendLayout();
            // 
            // tab2
            // 
            this.tab2.Icon = null;
            this.tab2.Location = new System.Drawing.Point(1, 23);
            this.tab2.Name = "tab2";
            this.tab2.Size = new System.Drawing.Size(798, 426);
            this.tab2.Text = "tab2";
            // 
            // tab4
            // 
            this.tab4.Icon = null;
            this.tab4.Location = new System.Drawing.Point(1, 23);
            this.tab4.Name = "tab4";
            this.tab4.Size = new System.Drawing.Size(798, 426);
            this.tab4.Text = "tab4";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "TestForm";
            this.Text = "TabControl Test Form";
            this.ResumeLayout(false);

        }


        #endregion

        private Manina.Windows.Forms.TabControl.Tab tab2;
        private Manina.Windows.Forms.TabControl.Tab tab4;
    }
}

