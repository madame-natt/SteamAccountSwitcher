using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class Settings : Form
    {
        public new AccountSwitcher Parent { get; set; }

        private bool _hasCached = false;

        private bool _developerMode, _verboseLogging, _logToFile, _useSteamPath, _closeToTray, _minToTray, _showTrayBalloon, _startMinimised;
        private string _pathToSteam;
        private ushort _maxCloseAttempts, _threadPausetimeMillis;

        public Settings()
        {
            InitializeComponent();
        }

        public void InitSettings()
        {
            Parent.ssm.AddBoolean("developerMode", false, "Toggles Developer Mode.", "developer");
            Parent.ssm.AddBoolean("verboseLogging", false, "Logs more information. Useful when debugging.", "developer");
            Parent.ssm.AddBoolean("logToFile", false, "Writes log to a file (SAS.log).", "developer");

            Parent.ssm.AddBoolean("useSteamPath", false, "Directly launches the Steam.exe to run Steam.", "user");
            Parent.ssm.AddString("pathToSteam", "C:\\Program Files (x86)\\Steam\\Steam.exe", "Path to Steam executable. Not used when 'useSteamPath' is 'false'.", "user");

            Parent.ssm.AddBoolean("closeToTray", true, "Close SAS to system notification tray.", "user");
            Parent.ssm.AddBoolean("minimiseToTray", false, "Minimise SAS to system notification tray.", "user");
            Parent.ssm.AddBoolean("showTrayBalloon", true, "Show balloon when SAS is minimised to the tray.", "user");

            Parent.ssm.AddBoolean("startMinimised", false, "Start the program in a minimised state.", "user");
            Parent.ssm.AddUInt16("maxCloseAttempts", 3, "Number of close attempts SAS will perform.", "user");
            Parent.ssm.AddUInt16("threadPausetimeMillis", 2000, "Number of milliseconds to wait before checking is Steam is still running.", "user");

            LoadSettings();
            CancelChange();
        }

        public void LoadSettings()
        {
            _developerMode = Parent.ssm.GetBoolean("developerMode");
            _verboseLogging = Parent.ssm.GetBoolean("verboseLogging");
            _logToFile = Parent.ssm.GetBoolean("logToFile");

            _useSteamPath = Parent.ssm.GetBoolean("useSteamPath");
            _pathToSteam = Parent.ssm.GetString("pathToSteam");

            _closeToTray = Parent.ssm.GetBoolean("closeToTray");
            _minToTray = Parent.ssm.GetBoolean("minimiseToTray");
            _showTrayBalloon = Parent.ssm.GetBoolean("showTrayBalloon");

            _startMinimised = Parent.ssm.GetBoolean("startMinimised");
            _maxCloseAttempts = Parent.ssm.GetUInt16("maxCloseAttempts");
            _threadPausetimeMillis = Parent.ssm.GetUInt16("threadPausetimeMillis");

            _hasCached = true;
        }

        public override string ToString()
        {
            List<string> allSettings = new List<string>();

            allSettings.Add(String.Format("developerMode: {0}", _developerMode));
            allSettings.Add(String.Format("verboseLogging: {0}", _verboseLogging));
            allSettings.Add(String.Format("logToFile: {0}", _logToFile));

            allSettings.Add(String.Format("useSteamPath: {0}", _useSteamPath));
            allSettings.Add(String.Format("pathToSteam: {0}", _pathToSteam));

            allSettings.Add(String.Format("closeToTray: {0}", _closeToTray));
            allSettings.Add(String.Format("minimiseToTray: {0}", _minToTray));
            allSettings.Add(String.Format("showTrayBalloon: {0}", _showTrayBalloon));

            allSettings.Add(String.Format("startMinimised: {0}", _startMinimised));
            allSettings.Add(String.Format("maxCloseAttempts: {0}", _maxCloseAttempts));
            allSettings.Add(String.Format("threadPausetimeMillis: {0}", _threadPausetimeMillis));

            return String.Join(", ", allSettings);
        }

        private void ApplySettings()
        {
            DeveloperMode = devModeCheck.Checked;
            VerboseLogging = verboseModeCheck.Checked;
            LogToFile = logToFileCheck.Checked;
            UseSteamPath = useSteamPathCheck.Checked;
            SteamPath = steamPathTextbox.Text;
            CloseToTray = closeToTrayCheck.Checked;
            MinimiseToTray = minToTrayCheck.Checked;
            ShowTrayBalloon = showBalloonCheck.Checked;
            StartMinimised = startMinimisedCheck.Checked;

            steamPathTextbox.Enabled = useSteamPathCheck.Checked;
            findSteamButton.Enabled = useSteamPathCheck.Checked;

            Parent.UpdateUI();
            LoadSettings();
        }

        private void CancelChange()
        {
            devModeCheck.Checked = _developerMode;
            verboseModeCheck.Checked = _verboseLogging;
            logToFileCheck.Checked = _logToFile;
            useSteamPathCheck.Checked = _useSteamPath;
            steamPathTextbox.Text = _pathToSteam;
            closeToTrayCheck.Checked = _closeToTray;
            minToTrayCheck.Checked = _minToTray;
            showBalloonCheck.Checked = _showTrayBalloon;
            startMinimisedCheck.Checked = _startMinimised;

            steamPathTextbox.Enabled = useSteamPathCheck.Checked;
            findSteamButton.Enabled = useSteamPathCheck.Checked;

            Parent.UpdateUI();
        }

        public bool DeveloperMode
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _developerMode;
            }
            set
            {
                Parent.ssm.SetBoolean("developerMode", value);
                _developerMode = value;
            }
        }

        public bool VerboseLogging
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _verboseLogging;
            }
            set
            {
                Parent.ssm.SetBoolean("verboseLogging", value);
                _verboseLogging = value;
            }
        }

        public bool LogToFile
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _logToFile;
            }
            set
            {
                Parent.ssm.SetBoolean("logToFile", value);
                _logToFile = value;
            }
        }

        public bool UseSteamPath
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _useSteamPath;
            }
            set
            {
                Parent.ssm.SetBoolean("useSteamPath", value);
                _useSteamPath = value;
            }
        }

        public ushort MaxCloseAttempts
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _maxCloseAttempts;
            }
            set
            {
                Parent.ssm.SetUInt16("maxCloseAttempts", value);
                _maxCloseAttempts = value;
            }
        }

        public ushort ThreadPauseTime
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _threadPausetimeMillis;
            }
            set
            {
                Parent.ssm.SetUInt16("threadPausetimeMillis", value);
                _threadPausetimeMillis = value;
            }
        }

        public string SteamPath
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _pathToSteam;
            }
            set
            {
                Parent.ssm.SetString("pathToSteam", value);
                _pathToSteam = value;
            }
        }

        public bool CloseToTray
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _closeToTray;
            }
            set
            {
                Parent.ssm.SetBoolean("closeToTray", value);
                _useSteamPath = value;
            }
        }

        public bool MinimiseToTray
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _minToTray;
            }
            set
            {
                Parent.ssm.SetBoolean("minimiseToTray", value);
                _minToTray = value;
            }
        }

        public bool StartMinimised
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _startMinimised;
            }
            set
            {
                Parent.ssm.SetBoolean("startMinimised", value);
                _startMinimised = value;
            }
        }

        public bool ShowTrayBalloon
        {
            get
            {
                if (!_hasCached) LoadSettings();
                return _showTrayBalloon;
            }
            set
            {
                Parent.ssm.SetBoolean("showTrayBalloon", value);
                _showTrayBalloon = value;
            }
        }

        private void useSteamPathCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            steamPathTextbox.Enabled = useSteamPathCheck.Checked;
            findSteamButton.Enabled = useSteamPathCheck.Checked;
        }

        private void applySettings_Click(object sender, System.EventArgs e)
        {
            ApplySettings();
            CloseForm();
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            CancelChange();
            CloseForm();
        }

        private void Settings_Load(object sender, System.EventArgs e)
        {
            if (Owner != null)
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);

            CancelChange();
        }

        private void findSteamButton_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                steamPathTextbox.Text = Path.GetFullPath(openFileDialog.FileName);
        }

        private void steamPathTextbox_TextChanged(object sender, System.EventArgs e)
        {
            toolTip.SetToolTip(steamPathTextbox, steamPathTextbox.Text);
        }

        private void CloseForm()
        {
            this.Hide();
            CancelChange();
            //ResetInput();
            Parent.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            else
            {
                e.Cancel = true;
                CloseForm();
            }
        }
    }
}
