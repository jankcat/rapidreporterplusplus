namespace RapidLib.Forms
{
    partial class ReporterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReporterForm));
            this.transparencySilder = new MB.Controls.ColorSlider();
            this.screenShotButton = new System.Windows.Forms.Button();
            this.plainTextNoteButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.progressBarBackground = new System.Windows.Forms.ProgressBar();
            this.noteInputBox = new System.Windows.Forms.TextBox();
            this.noteType = new RapidLib.TransparentLabel();
            this.previousTypeLabel = new RapidLib.TransparentLabel();
            this.nextTypeLabel = new RapidLib.TransparentLabel();
            this.SuspendLayout();
            // 
            // transparencySilder
            // 
            this.transparencySilder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.transparencySilder.BackColor = System.Drawing.Color.Transparent;
            this.transparencySilder.BarInnerColor = System.Drawing.SystemColors.ControlDark;
            this.transparencySilder.BarOuterColor = System.Drawing.SystemColors.ControlDark;
            this.transparencySilder.BarPenColor = System.Drawing.SystemColors.ControlDarkDark;
            this.transparencySilder.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.transparencySilder.DrawFocusRectangle = false;
            this.transparencySilder.ElapsedInnerColor = System.Drawing.SystemColors.ControlDark;
            this.transparencySilder.ElapsedOuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.transparencySilder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.transparencySilder.LargeChange = ((uint)(50u));
            this.transparencySilder.Location = new System.Drawing.Point(3, 5);
            this.transparencySilder.Minimum = -100;
            this.transparencySilder.Name = "transparencySilder";
            this.transparencySilder.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.transparencySilder.Size = new System.Drawing.Size(12, 45);
            this.transparencySilder.SmallChange = ((uint)(1u));
            this.transparencySilder.TabIndex = 1;
            this.transparencySilder.TabStop = false;
            this.transparencySilder.ThumbRoundRectSize = new System.Drawing.Size(10, 2);
            this.transparencySilder.ThumbSize = 6;
            this.transparencySilder.Value = -100;
            // 
            // screenShotButton
            // 
            this.screenShotButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.screenShotButton.BackgroundImage = global::RapidLib.Properties.Resources.iconshot_dis;
            this.screenShotButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.screenShotButton.Enabled = false;
            this.screenShotButton.Location = new System.Drawing.Point(21, 5);
            this.screenShotButton.Name = "screenShotButton";
            this.screenShotButton.Size = new System.Drawing.Size(27, 23);
            this.screenShotButton.TabIndex = 2;
            this.screenShotButton.TabStop = false;
            this.screenShotButton.UseVisualStyleBackColor = true;
            // 
            // plainTextNoteButton
            // 
            this.plainTextNoteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.plainTextNoteButton.BackgroundImage = global::RapidLib.Properties.Resources.iconnotes_dis;
            this.plainTextNoteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.plainTextNoteButton.Enabled = false;
            this.plainTextNoteButton.Location = new System.Drawing.Point(21, 27);
            this.plainTextNoteButton.Name = "plainTextNoteButton";
            this.plainTextNoteButton.Size = new System.Drawing.Size(27, 23);
            this.plainTextNoteButton.TabIndex = 3;
            this.plainTextNoteButton.TabStop = false;
            this.plainTextNoteButton.UseVisualStyleBackColor = true;
            // 
            // quitButton
            // 
            this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quitButton.BackgroundImage = global::RapidLib.Properties.Resources.exit32;
            this.quitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.quitButton.Location = new System.Drawing.Point(676, 5);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(25, 25);
            this.quitButton.TabIndex = 4;
            this.quitButton.TabStop = false;
            this.quitButton.UseVisualStyleBackColor = true;
            // 
            // progressBarBackground
            // 
            this.progressBarBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarBackground.Enabled = false;
            this.progressBarBackground.Location = new System.Drawing.Point(54, 5);
            this.progressBarBackground.Name = "progressBarBackground";
            this.progressBarBackground.Size = new System.Drawing.Size(618, 45);
            this.progressBarBackground.Step = 1;
            this.progressBarBackground.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarBackground.TabIndex = 5;
            // 
            // noteInputBox
            // 
            this.noteInputBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteInputBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.noteInputBox.Font = new System.Drawing.Font("Segoe UI", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.noteInputBox.Location = new System.Drawing.Point(209, 11);
            this.noteInputBox.Name = "noteInputBox";
            this.noteInputBox.Size = new System.Drawing.Size(461, 28);
            this.noteInputBox.TabIndex = 9;
            this.noteInputBox.WordWrap = false;
            // 
            // noteType
            // 
            this.noteType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.noteType.BackColor = System.Drawing.Color.Transparent;
            this.noteType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.noteType.Location = new System.Drawing.Point(54, 5);
            this.noteType.Name = "noteType";
            this.noteType.Size = new System.Drawing.Size(155, 45);
            this.noteType.TabIndex = 8;
            this.noteType.Text = "Environment:";
            this.noteType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // previousTypeLabel
            // 
            this.previousTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.previousTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.previousTypeLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.previousTypeLabel.Location = new System.Drawing.Point(54, 36);
            this.previousTypeLabel.Name = "previousTypeLabel";
            this.previousTypeLabel.Size = new System.Drawing.Size(120, 12);
            this.previousTypeLabel.TabIndex = 7;
            this.previousTypeLabel.Text = "Summary:";
            this.previousTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nextTypeLabel
            // 
            this.nextTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nextTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.nextTypeLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.nextTypeLabel.Location = new System.Drawing.Point(54, 5);
            this.nextTypeLabel.Name = "nextTypeLabel";
            this.nextTypeLabel.Size = new System.Drawing.Size(120, 12);
            this.nextTypeLabel.TabIndex = 6;
            this.nextTypeLabel.Text = "Summary:";
            // 
            // ReporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(104)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(705, 55);
            this.ControlBox = false;
            this.Controls.Add(this.noteInputBox);
            this.Controls.Add(this.noteType);
            this.Controls.Add(this.previousTypeLabel);
            this.Controls.Add(this.nextTypeLabel);
            this.Controls.Add(this.progressBarBackground);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.plainTextNoteButton);
            this.Controls.Add(this.screenShotButton);
            this.Controls.Add(this.transparencySilder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(4000, 55);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 55);
            this.Name = "ReporterForm";
            this.ShowIcon = false;
            this.Text = "RapidReporter++";
            this.TopMost = true;
            this.Move += new System.EventHandler(this.ReporterForm_Move);
            this.Resize += new System.EventHandler(this.ReporterForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MB.Controls.ColorSlider transparencySilder;
        private System.Windows.Forms.Button screenShotButton;
        private System.Windows.Forms.Button plainTextNoteButton;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.ProgressBar progressBarBackground;
        private TransparentLabel nextTypeLabel;
        private TransparentLabel previousTypeLabel;
        private TransparentLabel noteType;
        private System.Windows.Forms.TextBox noteInputBox;

    }
}