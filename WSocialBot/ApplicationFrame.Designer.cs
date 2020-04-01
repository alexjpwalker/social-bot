namespace WSocialBot
{
    partial class ApplicationFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationFrame));
            this.ChatBox = new System.Windows.Forms.RichTextBox();
            this.ChatEntry = new System.Windows.Forms.RichTextBox();
            this.Submit = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.t_botsettings = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.renameBot = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.t_chatsettings = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.autoCaps = new System.Windows.Forms.ToolStripButton();
            this.changeFont = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFont = new System.Windows.Forms.FontDialog();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatBox
            // 
            this.ChatBox.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatBox.Location = new System.Drawing.Point(82, 12);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.Size = new System.Drawing.Size(800, 600);
            this.ChatBox.TabIndex = 0;
            this.ChatBox.Text = "";
            // 
            // ChatEntry
            // 
            this.ChatEntry.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatEntry.Location = new System.Drawing.Point(82, 618);
            this.ChatEntry.MaxLength = 150;
            this.ChatEntry.Name = "ChatEntry";
            this.ChatEntry.Size = new System.Drawing.Size(717, 74);
            this.ChatEntry.TabIndex = 1;
            this.ChatEntry.Text = "";
            this.ChatEntry.TextChanged += new System.EventHandler(this.ChatEntry_TextChanged);
            // 
            // Submit
            // 
            this.Submit.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Submit.Location = new System.Drawing.Point(810, 626);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(75, 53);
            this.Submit.TabIndex = 2;
            this.Submit.Text = "==>";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.t_botsettings,
            this.toolStripSeparator1,
            this.renameBot,
            this.toolStripSeparator2,
            this.t_chatsettings,
            this.toolStripSeparator3,
            this.autoCaps,
            this.changeFont,
            this.toolStripSeparator4});
            this.toolStrip.Location = new System.Drawing.Point(909, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(99, 732);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // t_botsettings
            // 
            this.t_botsettings.Name = "t_botsettings";
            this.t_botsettings.Size = new System.Drawing.Size(96, 17);
            this.t_botsettings.Text = "Bot Settings";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(96, 6);
            // 
            // renameBot
            // 
            this.renameBot.Image = ((System.Drawing.Image)(resources.GetObject("renameBot.Image")));
            this.renameBot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.renameBot.Name = "renameBot";
            this.renameBot.Size = new System.Drawing.Size(96, 21);
            this.renameBot.Text = "Rename Bot";
            this.renameBot.ToolTipText = "Change the display name of the social bot.";
            this.renameBot.Click += new System.EventHandler(this.renameBot_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(96, 6);
            // 
            // t_chatsettings
            // 
            this.t_chatsettings.Name = "t_chatsettings";
            this.t_chatsettings.Size = new System.Drawing.Size(96, 17);
            this.t_chatsettings.Text = "Chat Settings";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(96, 6);
            // 
            // autoCaps
            // 
            this.autoCaps.Checked = true;
            this.autoCaps.CheckOnClick = true;
            this.autoCaps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCaps.Image = ((System.Drawing.Image)(resources.GetObject("autoCaps.Image")));
            this.autoCaps.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.autoCaps.Name = "autoCaps";
            this.autoCaps.Size = new System.Drawing.Size(96, 21);
            this.autoCaps.Text = "AutoCaps";
            this.autoCaps.ToolTipText = "Automatically capitalize your chat messages";
            this.autoCaps.Click += new System.EventHandler(this.updateChatOptions);
            // 
            // changeFont
            // 
            this.changeFont.Image = ((System.Drawing.Image)(resources.GetObject("changeFont.Image")));
            this.changeFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.changeFont.Name = "changeFont";
            this.changeFont.Size = new System.Drawing.Size(96, 21);
            this.changeFont.Text = "Font...";
            this.changeFont.ToolTipText = "Select chat message font";
            this.changeFont.Click += new System.EventHandler(this.changeFont_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(96, 6);
            // 
            // selectFont
            // 
            this.selectFont.Color = System.Drawing.SystemColors.ControlText;
            this.selectFont.ShowColor = true;
            this.selectFont.ShowHelp = true;
            // 
            // ApplicationFrame
            // 
            this.AcceptButton = this.Submit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 732);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.ChatEntry);
            this.Controls.Add(this.ChatBox);
            this.Name = "ApplicationFrame";
            this.Text = "WSocialBot";
            this.Load += new System.EventHandler(this.ApplicationFrame_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ChatBox;
        private System.Windows.Forms.RichTextBox ChatEntry;
        private System.Windows.Forms.Button Submit;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton renameBot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton changeFont;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel t_botsettings;
        private System.Windows.Forms.FontDialog selectFont;
        private System.Windows.Forms.ToolStripButton autoCaps;
        private System.Windows.Forms.ToolStripLabel t_chatsettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}

