namespace SteamAccountSwitcher
{
    partial class EditAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAccount));
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.descLabel = new System.Windows.Forms.Label();
            this.descTextbox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.editAccountButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.displayNameLabel = new System.Windows.Forms.Label();
            this.displayNameTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(88, 32);
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(179, 20);
            this.usernameTextbox.TabIndex = 1;
            this.usernameTextbox.TextChanged += new System.EventHandler(this.usernameTextbox_TextChanged);
            // 
            // descLabel
            // 
            this.descLabel.Location = new System.Drawing.Point(7, 61);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(75, 13);
            this.descLabel.TabIndex = 2;
            this.descLabel.Text = "Description:";
            this.descLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // descTextbox
            // 
            this.descTextbox.Location = new System.Drawing.Point(88, 58);
            this.descTextbox.Multiline = true;
            this.descTextbox.Name = "descTextbox";
            this.descTextbox.Size = new System.Drawing.Size(179, 55);
            this.descTextbox.TabIndex = 2;
            // 
            // usernameLabel
            // 
            this.usernameLabel.Location = new System.Drawing.Point(7, 35);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(75, 13);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "Username:";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // editAccountButton
            // 
            this.editAccountButton.Enabled = false;
            this.editAccountButton.Location = new System.Drawing.Point(12, 123);
            this.editAccountButton.Name = "editAccountButton";
            this.editAccountButton.Size = new System.Drawing.Size(120, 23);
            this.editAccountButton.TabIndex = 3;
            this.editAccountButton.Text = "Edit Account";
            this.editAccountButton.UseVisualStyleBackColor = true;
            this.editAccountButton.Click += new System.EventHandler(this.EditAccountButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(151, 123);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(120, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // displayNameLabel
            // 
            this.displayNameLabel.Location = new System.Drawing.Point(7, 9);
            this.displayNameLabel.Name = "displayNameLabel";
            this.displayNameLabel.Size = new System.Drawing.Size(75, 13);
            this.displayNameLabel.TabIndex = 7;
            this.displayNameLabel.Text = "Display Name:";
            this.displayNameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // displayNameTextbox
            // 
            this.displayNameTextbox.Location = new System.Drawing.Point(88, 6);
            this.displayNameTextbox.Name = "displayNameTextbox";
            this.displayNameTextbox.Size = new System.Drawing.Size(179, 20);
            this.displayNameTextbox.TabIndex = 0;
            // 
            // EditAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 151);
            this.Controls.Add(this.displayNameLabel);
            this.Controls.Add(this.displayNameTextbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.editAccountButton);
            this.Controls.Add(this.descTextbox);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.usernameTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EditAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edit Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.TextBox descTextbox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Button editAccountButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label displayNameLabel;
        private System.Windows.Forms.TextBox displayNameTextbox;
    }
}