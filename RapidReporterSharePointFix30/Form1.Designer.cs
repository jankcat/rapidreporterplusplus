namespace RapidReporterSharePointFix30
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnChoosefile = new System.Windows.Forms.Button();
            this.FolderBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnChoosefile
            // 
            this.btnChoosefile.Location = new System.Drawing.Point(12, 12);
            this.btnChoosefile.Name = "btnChoosefile";
            this.btnChoosefile.Size = new System.Drawing.Size(154, 23);
            this.btnChoosefile.TabIndex = 0;
            this.btnChoosefile.Text = "Convert One Test Session";
            this.btnChoosefile.UseVisualStyleBackColor = true;
            this.btnChoosefile.Click += new System.EventHandler(this.btnChoosefile_Click);
            // 
            // FolderBtn
            // 
            this.FolderBtn.Location = new System.Drawing.Point(12, 41);
            this.FolderBtn.Name = "FolderBtn";
            this.FolderBtn.Size = new System.Drawing.Size(154, 23);
            this.FolderBtn.TabIndex = 1;
            this.FolderBtn.Text = "Convert Folder";
            this.FolderBtn.UseVisualStyleBackColor = true;
            this.FolderBtn.Click += new System.EventHandler(this.FolderBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 73);
            this.Controls.Add(this.FolderBtn);
            this.Controls.Add(this.btnChoosefile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SharePoint Issue #30";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnChoosefile;
        private System.Windows.Forms.Button FolderBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

