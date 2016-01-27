namespace RapidLib.Forms
{
    partial class ScreenShotPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenShotPreview));
            this.screenshotbox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenshotbox)).BeginInit();
            this.SuspendLayout();
            // 
            // screenshotbox
            // 
            this.screenshotbox.Location = new System.Drawing.Point(0, 0);
            this.screenshotbox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.screenshotbox.MaximumSize = new System.Drawing.Size(350, 250);
            this.screenshotbox.Name = "screenshotbox";
            this.screenshotbox.Size = new System.Drawing.Size(100, 0);
            this.screenshotbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.screenshotbox.TabIndex = 0;
            this.screenshotbox.TabStop = false;
            // 
            // ScreenShotPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(334, 0);
            this.Controls.Add(this.screenshotbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 250);
            this.MinimizeBox = false;
            this.Name = "ScreenShotPreview";
            this.ShowIcon = false;
            this.Text = "Screen Shot Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScreenShotPreview_FormClosing);
            this.Move += new System.EventHandler(this.ScreenShotPreview_Move);
            ((System.ComponentModel.ISupportInitialize)(this.screenshotbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox screenshotbox;
    }
}