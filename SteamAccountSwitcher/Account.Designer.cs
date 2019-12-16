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
            this.components = new System.ComponentModel.Container();
            this.userNamePanel = new System.Windows.Forms.Panel();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.utilsPanel = new System.Windows.Forms.Panel();
            this.downPosButton = new System.Windows.Forms.Button();
            this.upPosButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteAccount = new System.Windows.Forms.Button();
            this.descriptionPanel = new System.Windows.Forms.Panel();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.userNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.userNameLabel.Click += new System.EventHandler(this.accountInterface_Click);
            this.userNameLabel.DoubleClick += new System.EventHandler(this.accountInterface_DoubleClick);
            // 
            // utilsPanel
            // 
            this.utilsPanel.Controls.Add(this.downPosButton);
            this.utilsPanel.Controls.Add(this.upPosButton);
            this.utilsPanel.Controls.Add(this.editButton);
            this.utilsPanel.Controls.Add(this.deleteAccount);
            this.utilsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.utilsPanel.Location = new System.Drawing.Point(274, 0);
            this.utilsPanel.Name = "utilsPanel";
            this.utilsPanel.Size = new System.Drawing.Size(83, 48);
            this.utilsPanel.TabIndex = 1;
            this.utilsPanel.Click += new System.EventHandler(this.accountInterface_Click);
            this.utilsPanel.DoubleClick += new System.EventHandler(this.accountInterface_DoubleClick);
            // 
            // downPosButton
            // 
            this.downPosButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downPosButton.Location = new System.Drawing.Point(4, 23);
            this.downPosButton.Name = "downPosButton";
            this.downPosButton.Size = new System.Drawing.Size(26, 22);
            this.downPosButton.TabIndex = 3;
            this.downPosButton.TabStop = false;
            this.downPosButton.Text = "\\/";
            this.toolTip.SetToolTip(this.downPosButton, "Move Down");
            this.downPosButton.UseVisualStyleBackColor = true;
            this.downPosButton.Click += new System.EventHandler(this.downPosButton_Click);
            // 
            // upPosButton
            // 
            this.upPosButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upPosButton.Location = new System.Drawing.Point(4, 1);
            this.upPosButton.Name = "upPosButton";
            this.upPosButton.Size = new System.Drawing.Size(26, 22);
            this.upPosButton.TabIndex = 2;
            this.upPosButton.TabStop = false;
            this.upPosButton.Text = "/\\";
            this.toolTip.SetToolTip(this.upPosButton, "Move Up");
            this.upPosButton.UseVisualStyleBackColor = true;
            this.upPosButton.Click += new System.EventHandler(this.upPosButton_Click);
            // 
            // editButton
            // 
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.Location = new System.Drawing.Point(32, 23);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(49, 22);
            this.editButton.TabIndex = 1;
            this.editButton.TabStop = false;
            this.editButton.Text = "Edit";
            this.toolTip.SetToolTip(this.editButton, "Edit Account");
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteAccount
            // 
            this.deleteAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteAccount.Location = new System.Drawing.Point(32, 1);
            this.deleteAccount.Name = "deleteAccount";
            this.deleteAccount.Size = new System.Drawing.Size(49, 22);
            this.deleteAccount.TabIndex = 0;
            this.deleteAccount.TabStop = false;
            this.deleteAccount.Text = "Delete";
            this.toolTip.SetToolTip(this.deleteAccount, "Delete Account");
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
            this.descriptionLabel.AutoEllipsis = true;
            this.descriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionLabel.Location = new System.Drawing.Point(0, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(164, 48);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.descriptionLabel.Click += new System.EventHandler(this.accountInterface_Click);
            this.descriptionLabel.DoubleClick += new System.EventHandler(this.accountInterface_DoubleClick);
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
            this.Click += new System.EventHandler(this.accountInterface_Click);
            this.DoubleClick += new System.EventHandler(this.accountInterface_DoubleClick);
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
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button downPosButton;
        private System.Windows.Forms.Button upPosButton;
    }
}
