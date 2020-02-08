namespace SteamAccountSwitcher
{
    partial class Settings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applySettings = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.userPanel = new System.Windows.Forms.Panel();
            this.showBalloonCheck = new System.Windows.Forms.CheckBox();
            this.minToTrayCheck = new System.Windows.Forms.CheckBox();
            this.closeToTrayCheck = new System.Windows.Forms.CheckBox();
            this.findSteamButton = new System.Windows.Forms.Button();
            this.steamPathTextbox = new System.Windows.Forms.TextBox();
            this.useSteamPathCheck = new System.Windows.Forms.CheckBox();
            this.devPanel = new System.Windows.Forms.Panel();
            this.devModeCheck = new System.Windows.Forms.CheckBox();
            this.logToFileCheck = new System.Windows.Forms.CheckBox();
            this.verboseModeCheck = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonPanel.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.devPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.applySettings);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 154);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(339, 27);
            this.buttonPanel.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelButton.Location = new System.Drawing.Point(199, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(140, 27);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applySettings
            // 
            this.applySettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.applySettings.Location = new System.Drawing.Point(0, 0);
            this.applySettings.Name = "applySettings";
            this.applySettings.Size = new System.Drawing.Size(140, 27);
            this.applySettings.TabIndex = 0;
            this.applySettings.Text = "Apply";
            this.applySettings.UseVisualStyleBackColor = true;
            this.applySettings.Click += new System.EventHandler(this.applySettings_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.userPanel);
            this.settingsPanel.Controls.Add(this.devPanel);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(339, 154);
            this.settingsPanel.TabIndex = 1;
            // 
            // userPanel
            // 
            this.userPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userPanel.Controls.Add(this.showBalloonCheck);
            this.userPanel.Controls.Add(this.minToTrayCheck);
            this.userPanel.Controls.Add(this.closeToTrayCheck);
            this.userPanel.Controls.Add(this.findSteamButton);
            this.userPanel.Controls.Add(this.steamPathTextbox);
            this.userPanel.Controls.Add(this.useSteamPathCheck);
            this.userPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userPanel.Location = new System.Drawing.Point(134, 0);
            this.userPanel.Name = "userPanel";
            this.userPanel.Size = new System.Drawing.Size(205, 154);
            this.userPanel.TabIndex = 4;
            // 
            // showBalloonCheck
            // 
            this.showBalloonCheck.AutoSize = true;
            this.showBalloonCheck.Location = new System.Drawing.Point(10, 118);
            this.showBalloonCheck.Name = "showBalloonCheck";
            this.showBalloonCheck.Size = new System.Drawing.Size(120, 17);
            this.showBalloonCheck.TabIndex = 5;
            this.showBalloonCheck.Text = "Show Tray Balloons";
            this.toolTip.SetToolTip(this.showBalloonCheck, "Show balloon when SAS is minimised to the tray.");
            this.showBalloonCheck.UseVisualStyleBackColor = true;
            // 
            // minToTrayCheck
            // 
            this.minToTrayCheck.AutoSize = true;
            this.minToTrayCheck.Location = new System.Drawing.Point(10, 95);
            this.minToTrayCheck.Name = "minToTrayCheck";
            this.minToTrayCheck.Size = new System.Drawing.Size(139, 17);
            this.minToTrayCheck.TabIndex = 4;
            this.minToTrayCheck.Text = "Minimise to System Tray";
            this.toolTip.SetToolTip(this.minToTrayCheck, "Minimise SAS to system notification tray.");
            this.minToTrayCheck.UseVisualStyleBackColor = true;
            // 
            // closeToTrayCheck
            // 
            this.closeToTrayCheck.AutoSize = true;
            this.closeToTrayCheck.Location = new System.Drawing.Point(10, 72);
            this.closeToTrayCheck.Name = "closeToTrayCheck";
            this.closeToTrayCheck.Size = new System.Drawing.Size(125, 17);
            this.closeToTrayCheck.TabIndex = 3;
            this.closeToTrayCheck.Text = "Close to System Tray";
            this.toolTip.SetToolTip(this.closeToTrayCheck, "Close SAS to system notification tray.");
            this.closeToTrayCheck.UseVisualStyleBackColor = true;
            // 
            // findSteamButton
            // 
            this.findSteamButton.Location = new System.Drawing.Point(164, 32);
            this.findSteamButton.Name = "findSteamButton";
            this.findSteamButton.Size = new System.Drawing.Size(37, 20);
            this.findSteamButton.TabIndex = 2;
            this.findSteamButton.Text = "Find";
            this.toolTip.SetToolTip(this.findSteamButton, "Find the Steam executable manually.");
            this.findSteamButton.UseVisualStyleBackColor = true;
            this.findSteamButton.Click += new System.EventHandler(this.findSteamButton_Click);
            // 
            // steamPathTextbox
            // 
            this.steamPathTextbox.Location = new System.Drawing.Point(10, 32);
            this.steamPathTextbox.Name = "steamPathTextbox";
            this.steamPathTextbox.Size = new System.Drawing.Size(148, 20);
            this.steamPathTextbox.TabIndex = 1;
            this.steamPathTextbox.Text = "C:\\Program Files (x86)\\Steam\\Steam.exe";
            this.toolTip.SetToolTip(this.steamPathTextbox, "Path to Steam executable.");
            this.steamPathTextbox.TextChanged += new System.EventHandler(this.steamPathTextbox_TextChanged);
            // 
            // useSteamPathCheck
            // 
            this.useSteamPathCheck.AutoSize = true;
            this.useSteamPathCheck.Location = new System.Drawing.Point(10, 12);
            this.useSteamPathCheck.Name = "useSteamPathCheck";
            this.useSteamPathCheck.Size = new System.Drawing.Size(148, 17);
            this.useSteamPathCheck.TabIndex = 0;
            this.useSteamPathCheck.Text = "Launch using Steam Path";
            this.toolTip.SetToolTip(this.useSteamPathCheck, "Directly launches the Steam.exe to run Steam.");
            this.useSteamPathCheck.UseVisualStyleBackColor = true;
            this.useSteamPathCheck.CheckedChanged += new System.EventHandler(this.useSteamPathCheck_CheckedChanged);
            // 
            // devPanel
            // 
            this.devPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.devPanel.Controls.Add(this.devModeCheck);
            this.devPanel.Controls.Add(this.logToFileCheck);
            this.devPanel.Controls.Add(this.verboseModeCheck);
            this.devPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.devPanel.Location = new System.Drawing.Point(0, 0);
            this.devPanel.Name = "devPanel";
            this.devPanel.Size = new System.Drawing.Size(134, 154);
            this.devPanel.TabIndex = 3;
            // 
            // devModeCheck
            // 
            this.devModeCheck.AutoSize = true;
            this.devModeCheck.Location = new System.Drawing.Point(12, 12);
            this.devModeCheck.Name = "devModeCheck";
            this.devModeCheck.Size = new System.Drawing.Size(105, 17);
            this.devModeCheck.TabIndex = 0;
            this.devModeCheck.Text = "Developer Mode";
            this.toolTip.SetToolTip(this.devModeCheck, "Toggles Developer Mode.");
            this.devModeCheck.UseVisualStyleBackColor = true;
            // 
            // logToFileCheck
            // 
            this.logToFileCheck.AutoSize = true;
            this.logToFileCheck.Location = new System.Drawing.Point(12, 58);
            this.logToFileCheck.Name = "logToFileCheck";
            this.logToFileCheck.Size = new System.Drawing.Size(75, 17);
            this.logToFileCheck.TabIndex = 2;
            this.logToFileCheck.Text = "Log to File";
            this.toolTip.SetToolTip(this.logToFileCheck, "Writes log to a file (SAS.log).");
            this.logToFileCheck.UseVisualStyleBackColor = true;
            // 
            // verboseModeCheck
            // 
            this.verboseModeCheck.AutoSize = true;
            this.verboseModeCheck.Location = new System.Drawing.Point(12, 35);
            this.verboseModeCheck.Name = "verboseModeCheck";
            this.verboseModeCheck.Size = new System.Drawing.Size(95, 17);
            this.verboseModeCheck.TabIndex = 1;
            this.verboseModeCheck.Text = "Verbose Mode";
            this.toolTip.SetToolTip(this.verboseModeCheck, "Logs more information. Useful when debugging.");
            this.verboseModeCheck.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "Steam.exe";
            this.openFileDialog.Filter = "Executable Files (*.exe)|*.exe";
            this.openFileDialog.InitialDirectory = "C:\\";
            // 
            // Settings
            // 
            this.AcceptButton = this.applySettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(339, 181);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.buttonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.buttonPanel.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.userPanel.ResumeLayout(false);
            this.userPanel.PerformLayout();
            this.devPanel.ResumeLayout(false);
            this.devPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button applySettings;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.CheckBox devModeCheck;
        private System.Windows.Forms.Panel userPanel;
        private System.Windows.Forms.Panel devPanel;
        private System.Windows.Forms.CheckBox logToFileCheck;
        private System.Windows.Forms.CheckBox verboseModeCheck;
        private System.Windows.Forms.Button findSteamButton;
        private System.Windows.Forms.TextBox steamPathTextbox;
        private System.Windows.Forms.CheckBox useSteamPathCheck;
        private System.Windows.Forms.CheckBox closeToTrayCheck;
        private System.Windows.Forms.CheckBox minToTrayCheck;
        private System.Windows.Forms.CheckBox showBalloonCheck;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}