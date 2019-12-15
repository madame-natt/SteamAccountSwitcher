namespace SteamAccountSwitcher
{
    partial class Account
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.userNamePanel = new System.Windows.Forms.Panel();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.utilsPanel = new System.Windows.Forms.Panel();
            this.deleteAccount = new System.Windows.Forms.Button();
            this.descriptionPanel = new System.Windows.Forms.Panel();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.editButton = new System.Windows.Forms.Button();
            this.userNamePanel.SuspendLayout();
            this.utilsPanel.SuspendLayout();
            this.descriptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // userNamePanel
            // 
            this.userNamePanel.Controls.Add(this.userNameLabel);
            this.userNamePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.userNamePanel.Location = new System.Drawing.Point(0, 0);
            this.userNamePanel.Name = "userNamePanel";
            this.userNamePanel.Size = new System.Drawing.Size(110, 48);
            this.userNamePanel.TabIndex = 0;
            this.userNamePanel.Click += new System.EventHandler(this.accountInterface_Click);
            // 
            // userNameLabel
            // 
            this.userNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userNameLabel.Location = new System.Drawing.Point(0, 0);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(110, 48);
            this.userNameLabel.TabIndex = 0;
            this.userNameLabel.Text = "username";
            this.userNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.userNameLabel.Click += new System.EventHandler(this.accountInterface_Click);
            // 
            // utilsPanel
            // 
            this.utilsPanel.Controls.Add(this.editButton);
            this.utilsPanel.Controls.Add(this.deleteAccount);
            this.utilsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.utilsPanel.Location = new System.Drawing.Point(274, 0);
            this.utilsPanel.Name = "utilsPanel";
            this.utilsPanel.Size = new System.Drawing.Size(83, 48);
            this.utilsPanel.TabIndex = 1;
            this.utilsPanel.Click += new System.EventHandler(this.accountInterface_Click);
            // 
            // deleteAccount
            // 
            this.deleteAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteAccount.Location = new System.Drawing.Point(6, 1);
            this.deleteAccount.Name = "deleteAccount";
            this.deleteAccount.Size = new System.Drawing.Size(74, 22);
            this.deleteAccount.TabIndex = 0;
            this.deleteAccount.Text = "Delete";
            this.deleteAccount.UseVisualStyleBackColor = true;
            this.deleteAccount.Click += new System.EventHandler(this.deleteAccount_Click);
            // 
            // descriptionPanel
            // 
            this.descriptionPanel.Controls.Add(this.descriptionLabel);
            this.descriptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionPanel.Location = new System.Drawing.Point(110, 0);
            this.descriptionPanel.Name = "descriptionPanel";
            this.descriptionPanel.Size = new System.Drawing.Size(164, 48);
            this.descriptionPanel.TabIndex = 2;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionLabel.Location = new System.Drawing.Point(0, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(164, 48);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.descriptionLabel.Click += new System.EventHandler(this.accountInterface_Click);
            // 
            // editButton
            // 
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.Location = new System.Drawing.Point(6, 24);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(74, 22);
            this.editButton.TabIndex = 1;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            // 
            // Account
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.descriptionPanel);
            this.Controls.Add(this.utilsPanel);
            this.Controls.Add(this.userNamePanel);
            this.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.Name = "Account";
            this.Size = new System.Drawing.Size(357, 48);
            this.userNamePanel.ResumeLayout(false);
            this.utilsPanel.ResumeLayout(false);
            this.descriptionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel userNamePanel;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Panel utilsPanel;
        private System.Windows.Forms.Panel descriptionPanel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button deleteAccount;
        private System.Windows.Forms.Button editButton;
    }
}
