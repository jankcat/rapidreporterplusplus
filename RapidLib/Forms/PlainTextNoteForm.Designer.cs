namespace RapidLib.Forms
{
    partial class PlainTextNoteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlainTextNoteForm));
            this.clearNoteButton = new System.Windows.Forms.Button();
            this.saveAndCloseButton = new System.Windows.Forms.Button();
            this.noteTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // clearNoteButton
            // 
            this.clearNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearNoteButton.Location = new System.Drawing.Point(524, 135);
            this.clearNoteButton.Name = "clearNoteButton";
            this.clearNoteButton.Size = new System.Drawing.Size(75, 23);
            this.clearNoteButton.TabIndex = 0;
            this.clearNoteButton.Text = "Clear Note";
            this.clearNoteButton.UseVisualStyleBackColor = true;
            this.clearNoteButton.Click += new System.EventHandler(this.clearNoteButton_Click);
            // 
            // saveAndCloseButton
            // 
            this.saveAndCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAndCloseButton.Location = new System.Drawing.Point(605, 135);
            this.saveAndCloseButton.Name = "saveAndCloseButton";
            this.saveAndCloseButton.Size = new System.Drawing.Size(75, 23);
            this.saveAndCloseButton.TabIndex = 1;
            this.saveAndCloseButton.Text = "Save Note";
            this.saveAndCloseButton.UseVisualStyleBackColor = true;
            this.saveAndCloseButton.Click += new System.EventHandler(this.saveAndCloseButton_Click);
            // 
            // noteTextBox
            // 
            this.noteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteTextBox.Location = new System.Drawing.Point(3, 3);
            this.noteTextBox.Multiline = true;
            this.noteTextBox.Name = "noteTextBox";
            this.noteTextBox.Size = new System.Drawing.Size(678, 126);
            this.noteTextBox.TabIndex = 2;
            // 
            // PlainTextNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 162);
            this.Controls.Add(this.noteTextBox);
            this.Controls.Add(this.saveAndCloseButton);
            this.Controls.Add(this.clearNoteButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlainTextNoteForm";
            this.ShowInTaskbar = false;
            this.Text = "Plain Text Note";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlainTextNoteForm_FormClosing);
            this.Load += new System.EventHandler(this.PlainTextNoteForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clearNoteButton;
        private System.Windows.Forms.Button saveAndCloseButton;
        private System.Windows.Forms.TextBox noteTextBox;
    }
}