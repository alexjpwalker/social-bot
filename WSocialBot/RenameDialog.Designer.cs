namespace WSocialBot
{
    partial class RenameDialog
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
            this.Confirm = new System.Windows.Forms.Button();
            this.textField = new System.Windows.Forms.TextBox();
            this.t_enterNew = new System.Windows.Forms.Label();
            this.Randomize = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // Confirm
            // 
            this.Confirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Confirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Confirm.Location = new System.Drawing.Point(83, 180);
            this.Confirm.Margin = new System.Windows.Forms.Padding(4);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(100, 28);
            this.Confirm.TabIndex = 0;
            this.Confirm.Text = "OK";
            this.Confirm.UseVisualStyleBackColor = true;
            // 
            // textField
            // 
            this.textField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textField.Location = new System.Drawing.Point(83, 108);
            this.textField.Margin = new System.Windows.Forms.Padding(4);
            this.textField.Name = "textField";
            this.textField.Size = new System.Drawing.Size(241, 22);
            this.textField.TabIndex = 1;
            // 
            // t_enterNew
            // 
            this.t_enterNew.AutoSize = true;
            this.t_enterNew.Location = new System.Drawing.Point(150, 62);
            this.t_enterNew.Name = "t_enterNew";
            this.t_enterNew.Size = new System.Drawing.Size(106, 16);
            this.t_enterNew.TabIndex = 2;
            this.t_enterNew.Text = "Enter new name:";
            // 
            // Randomize
            // 
            this.Randomize.AutoSize = true;
            this.Randomize.Location = new System.Drawing.Point(247, 186);
            this.Randomize.Name = "Randomize";
            this.Randomize.Size = new System.Drawing.Size(77, 16);
            this.Randomize.TabIndex = 4;
            this.Randomize.TabStop = true;
            this.Randomize.Text = "Randomize";
            this.Randomize.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Randomize_LinkClicked);
            // 
            // RenameDialog
            // 
            this.AcceptButton = this.Confirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 251);
            this.Controls.Add(this.Randomize);
            this.Controls.Add(this.t_enterNew);
            this.Controls.Add(this.textField);
            this.Controls.Add(this.Confirm);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RenameDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rename Bot";
            this.Load += new System.EventHandler(this.RenameDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.TextBox textField;
        private System.Windows.Forms.Label t_enterNew;
        private System.Windows.Forms.LinkLabel Randomize;
    }
}