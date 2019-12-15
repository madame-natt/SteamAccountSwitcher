namespace SteamAccountSwitcher
{
    partial class AddAccount
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
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.descTextbox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.addAccountButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(76, 9);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(191, 20);
            this.usernameTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description:";
            // 
            // descTextbox
            // 
            this.descTextbox.Location = new System.Drawing.Point(76, 36);
            this.descTextbox.Multiline = true;
            this.descTextbox.Name = "descTextbox";
            this.descTextbox.Size = new System.Drawing.Size(191, 55);
            this.descTextbox.TabIndex = 3;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(12, 12);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(58, 13);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            // 
            // addAccountButton
            // 
            this.addAccountButton.Location = new System.Drawing.Point(12, 97);
            this.addAccountButton.Name = "addAccountButton";
            this.addAccountButton.Size = new System.Drawing.Size(120, 23);
            this.addAccountButton.TabIndex = 4;
            this.addAccountButton.Text = "Add Account";
            this.addAccountButton.UseVisualStyleBackColor = true;
            this.addAccountButton.Click += new System.EventHandler(this.AddAccountButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(151, 97);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(120, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 126);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addAccountButton);
            this.Controls.Add(this.descTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox descTextbox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Button addAccountButton;
        private System.Windows.Forms.Button cancelButton;
    }
}