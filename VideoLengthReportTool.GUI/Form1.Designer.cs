namespace VideoLengthReportTool.GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.videoFilesBox = new System.Windows.Forms.ListBox();
            this.addItemButton = new System.Windows.Forms.Button();
            this.removeFileButton = new System.Windows.Forms.Button();
            this.exportButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // videoFilesBox
            // 
            this.videoFilesBox.AllowDrop = true;
            this.videoFilesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoFilesBox.FormattingEnabled = true;
            this.videoFilesBox.IntegralHeight = false;
            this.videoFilesBox.ItemHeight = 15;
            this.videoFilesBox.Location = new System.Drawing.Point(12, 12);
            this.videoFilesBox.Name = "videoFilesBox";
            this.videoFilesBox.Size = new System.Drawing.Size(404, 200);
            this.videoFilesBox.TabIndex = 0;
            this.videoFilesBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.videoFilesBox_DragDrop);
            this.videoFilesBox.DragOver += new System.Windows.Forms.DragEventHandler(this.videoFilesBox_DragOver);
            // 
            // addItemButton
            // 
            this.addItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addItemButton.Location = new System.Drawing.Point(12, 218);
            this.addItemButton.Name = "addItemButton";
            this.addItemButton.Size = new System.Drawing.Size(75, 23);
            this.addItemButton.TabIndex = 1;
            this.addItemButton.Text = "Add File(s)";
            this.addItemButton.UseVisualStyleBackColor = true;
            this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
            // 
            // removeFileButton
            // 
            this.removeFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeFileButton.Location = new System.Drawing.Point(93, 218);
            this.removeFileButton.Name = "removeFileButton";
            this.removeFileButton.Size = new System.Drawing.Size(75, 23);
            this.removeFileButton.TabIndex = 2;
            this.removeFileButton.Text = "Remove";
            this.removeFileButton.UseVisualStyleBackColor = true;
            this.removeFileButton.Click += new System.EventHandler(this.removeFileButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Location = new System.Drawing.Point(341, 218);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(75, 23);
            this.exportButton.TabIndex = 3;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 253);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.removeFileButton);
            this.Controls.Add(this.addItemButton);
            this.Controls.Add(this.videoFilesBox);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Video Length Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox videoFilesBox;
        private Button addItemButton;
        private Button removeFileButton;
        private Button exportButton;
    }
}