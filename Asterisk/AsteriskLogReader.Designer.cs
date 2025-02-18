namespace Asterisk
{
    partial class AsteriskLogReader
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonReadLogFile = new System.Windows.Forms.Button();
            this.TextBoxInput = new System.Windows.Forms.TextBox();
            this.ButtonCreateImage = new System.Windows.Forms.Button();
            this.Buttons = new System.Windows.Forms.Panel();
            this.ButtonSaveBitmap = new System.Windows.Forms.Button();
            this.PictureBoxBitmap = new System.Windows.Forms.PictureBox();
            this.Buttons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxBitmap)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonReadLogFile
            // 
            this.ButtonReadLogFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonReadLogFile.Location = new System.Drawing.Point(0, 0);
            this.ButtonReadLogFile.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonReadLogFile.Name = "ButtonReadLogFile";
            this.ButtonReadLogFile.Size = new System.Drawing.Size(128, 40);
            this.ButtonReadLogFile.TabIndex = 0;
            this.ButtonReadLogFile.Text = "Log Datei Einlesen";
            this.ButtonReadLogFile.UseVisualStyleBackColor = true;
            this.ButtonReadLogFile.Click += new System.EventHandler(this.ButtonReadFromFile_Click);
            // 
            // TextBoxInput
            // 
            this.TextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxInput.Location = new System.Drawing.Point(0, 0);
            this.TextBoxInput.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxInput.Multiline = true;
            this.TextBoxInput.Name = "TextBoxInput";
            this.TextBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxInput.Size = new System.Drawing.Size(980, 565);
            this.TextBoxInput.TabIndex = 2;
            this.TextBoxInput.Text = "Log einfügen oder aus Datei laden.";
            this.TextBoxInput.WordWrap = false;
            this.TextBoxInput.TextChanged += new System.EventHandler(this.TextBoxInput_TextChanged);
            // 
            // ButtonCreateImage
            // 
            this.ButtonCreateImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonCreateImage.Location = new System.Drawing.Point(128, 0);
            this.ButtonCreateImage.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCreateImage.Name = "ButtonCreateImage";
            this.ButtonCreateImage.Size = new System.Drawing.Size(128, 40);
            this.ButtonCreateImage.TabIndex = 3;
            this.ButtonCreateImage.Text = "Bitmap Erstellen";
            this.ButtonCreateImage.UseVisualStyleBackColor = true;
            this.ButtonCreateImage.Click += new System.EventHandler(this.ButtonReadFromText_Click);
            // 
            // Buttons
            // 
            this.Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Buttons.Controls.Add(this.ButtonSaveBitmap);
            this.Buttons.Controls.Add(this.ButtonCreateImage);
            this.Buttons.Controls.Add(this.ButtonReadLogFile);
            this.Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Buttons.Location = new System.Drawing.Point(0, 565);
            this.Buttons.Margin = new System.Windows.Forms.Padding(2);
            this.Buttons.Name = "Buttons";
            this.Buttons.Size = new System.Drawing.Size(980, 40);
            this.Buttons.TabIndex = 4;
            // 
            // ButtonSaveBitmap
            // 
            this.ButtonSaveBitmap.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonSaveBitmap.Location = new System.Drawing.Point(852, 0);
            this.ButtonSaveBitmap.Name = "ButtonSaveBitmap";
            this.ButtonSaveBitmap.Size = new System.Drawing.Size(128, 40);
            this.ButtonSaveBitmap.TabIndex = 4;
            this.ButtonSaveBitmap.Text = "Bitmap Speichern";
            this.ButtonSaveBitmap.UseVisualStyleBackColor = true;
            this.ButtonSaveBitmap.Click += new System.EventHandler(this.ButtonSaveBitmap_Click);
            // 
            // PictureBoxBitmap
            // 
            this.PictureBoxBitmap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PictureBoxBitmap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxBitmap.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxBitmap.Margin = new System.Windows.Forms.Padding(2);
            this.PictureBoxBitmap.Name = "PictureBoxBitmap";
            this.PictureBoxBitmap.Size = new System.Drawing.Size(980, 565);
            this.PictureBoxBitmap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxBitmap.TabIndex = 5;
            this.PictureBoxBitmap.TabStop = false;
            // 
            // AsteriskLogReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(980, 605);
            this.Controls.Add(this.TextBoxInput);
            this.Controls.Add(this.PictureBoxBitmap);
            this.Controls.Add(this.Buttons);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AsteriskLogReader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asterisk Log Reader";
            this.Load += new System.EventHandler(this.AsteriskLogReader_Load);
            this.Buttons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxBitmap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonReadLogFile;
        private System.Windows.Forms.TextBox TextBoxInput;
        private System.Windows.Forms.Button ButtonCreateImage;
        private System.Windows.Forms.Panel Buttons;
        private System.Windows.Forms.PictureBox PictureBoxBitmap;
        private System.Windows.Forms.Button ButtonSaveBitmap;
    }
}

