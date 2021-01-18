using Microsoft.Win32;
using SimpleSettingsManager;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace SteamAccountSwitcher
{
    public partial class AccountSwitcher : Form
    {
        #region Variables
        private AddAccount _addAccountForm = new AddAccount();
        private EditAccount _editAccountForm = new EditAccount();
        private Settings _options = new Settings();

        private string _defaultConfigPath = Environment.CurrentDirectory;
        private string _logPath;
        private bool _argsStartMinimised = false;

        private Account _selectedAccount;
        private AccountXML _accountXML;
        private SSM _ssm;
        private Logging _logging;
        #endregion

        #region Constructors
        public AccountSwitcher(bool startMinimised)
        {
            _argsStartMinimised = startMinimised;
            Init();
        }
        #endregion

        #region Public Getters
        public SSM ssm
        {
            get { return _ssm; }
        }

        public Settings options
        {
            get { return _options; }
        }
        #endregion

        #region Public Methods
        public void Log(string log, Logging.Severity severity = Logging.Severity.INFO)
        {
            if (_logging != null) _logging.Log(log, severity);
        }

        public void OpenAccountEditor(Account account)
        {
            if (!_editAccountForm.Visible)
            {
                _editAccountForm.Account = account;
                _editAccountForm.Show(this);
            }
            else if (_editAccountForm.Account != null && _editAccountForm.Account == account)
            {
                _editAccountForm.BringToFront();
            }
            else
            {
                _editAccountForm.Account = account;
                _editAccountForm.BringToFront();
            }
        }

        public void OpenSettings()
        {
            Log(String.Format("OptionsForm -> Showing options form"), Logging.Severity.DEBUG);

            if (!this.Visible)
                MaxmiseFromTray();

            if (!options.Visible)
                options.Show(this);
            else
                options.BringToFront();
        }

        public void AddAccount(string userName, string description)
        {
            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            AddAccount(unixTimestamp, userName, description);
        }

        public void SelectAccount(Account account)
        {
            if (_selectedAccount != null) _selectedAccount.Selected = false;
            _selectedAccount = account;
            _selectedAccount.Selected = true;
            loginButton.Enabled = true;
            editAccountToolStripMenuItem.Enabled = true;
            loginButton.Text = String.Format("Login ({0})", _selectedAccount.CustomDisplayName);

            Log(String.Format("SelectAccount -> Changed to ({0})", _selectedAccount.ToString()), Logging.Severity.DEBUG);
        }

        public void UnselectAccount()
        {
            if (_selectedAccount != null) _selectedAccount.Selected = false;
            _selectedAccount = null;
            loginButton.Enabled = false;
            editAccountToolStripMenuItem.Enabled = false;
            loginButton.Text = "Login";
        }

        public void EditAccount(Account account)
        {
            Log(String.Format("EditAccount -> Editing account '{0}'", account.Username), Logging.Severity.DEBUG);
            _accountXML.EditAccount(account);
        }

        public void EditAllAccounts()
        {
            foreach (Account account in accountsPanel.Controls)
            {
                account.Position = accountsPanel.Controls.GetChildIndex(account);
                _accountXML.EditAccount(account);
            }
        }

        public void RemoveAccount(Account account)
        {
            if (_selectedAccount == account) UnselectAccount();
            _accountXML.RemoveAccount(account);
            account.Dispose();
            UpdateUI();
        }

        public void MoveAccountBy(Account account, int index)
        {
            accountsPanel.Controls.SetChildIndex(account, accountsPanel.Controls.GetChildIndex(account) + index);
            EditAllAccounts();
            UpdateUI();
        }

        public bool IsAccountTop(Account account)
        {
            return (accountsPanel.Controls.GetChildIndex(account) == 0);
        }

        public bool IsAccountBottom(Account account)
        {
            return (accountsPanel.Controls.GetChildIndex(account) == (accountsPanel.Controls.Count - 1));
        }

        public void UpdateUI()
        {
            contextMenuStrip.Items.Clear();
            InitLogging();

            foreach (Account acc in accountsPanel.Controls)
            {
                acc.UpdateUI();
                contextMenuStrip.Items.Add(acc.CustomDisplayName, null, delegate { SelectAccount(acc); Login(acc); }).ToolTipText = acc.Username;
            }

            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add("Options", null, delegate { OpenSettings(); }).ToolTipText = "Open the options";
            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add("Exit", null, delegate { CloseForm(true); }).ToolTipText = "Close the program";
        }

        public void Login(Account account)
        {
            Task threaddedLogin = new Task(() =>
            {
                if (!CloseSteam())
                {
                    MessageBox.Show("Steam could not be closed!\nPlease relaunch Steam manually to switch accounts.", "Steam cannot be closed", MessageBoxButtons.OK);
                    Log(String.Format("Login -> Steam could not be closed!"), Logging.Severity.WARN);
                }
                Thread.Sleep(500);

                ExecuteCmdCommand(String.Format("reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f", account.Username));
                ExecuteCmdCommand("reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f");
                Log(String.Format("Login -> Added reg keys (AutoLoginUser & RememberPassword)"), Logging.Severity.DEBUG);

                if (options.UseSteamPath)
                {
                    ExecuteCmdCommand(options.SteamPath);
                    Log(String.Format("Login -> Steam started using Steam Path"), Logging.Severity.DEBUG);
                }
                else
                {
                    ExecuteCmdCommand("start steam://open/main");
                    Log(String.Format("Login -> Steam started using 'steam://open/main'"), Logging.Severity.DEBUG);
                }
            });
            threaddedLogin.Start();
        }
        #endregion

        #region Private Methods
        private void Init()
        {
            InitializeComponent();

            {
                string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "mullak99", "SteamAccountSwitcher");

                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "accounts.xml")) && File.Exists(Path.Combine(Environment.CurrentDirectory, "settings.xml")))
                    _defaultConfigPath = Environment.CurrentDirectory;
                else if (File.Exists(Path.Combine(documentsPath, "accounts.xml")) && File.Exists(Path.Combine(documentsPath, "settings.xml")))
                    _defaultConfigPath = documentsPath;

                _accountXML = new AccountXML(Path.Combine(_defaultConfigPath, "accounts.xml"), this);
                _ssm = new SSM(new SSM_File(Path.Combine(_defaultConfigPath, "settings.xml"), SSM_File.Mode.XML));
                _logPath = Path.Combine(_defaultConfigPath, "SAS.log");
            }

            _addAccountForm.Parent = this;
            _editAccountForm.Parent = this;
            _options.Parent = this;

            _ssm.Open();
            _options.InitSettings();
            LoadAccountsFromXML();
            InitLogging();

            accountsPanel.HorizontalScroll.Maximum = 0;
            accountsPanel.AutoScroll = false;
            accountsPanel.VerticalScroll.Visible = false;
            accountsPanel.AutoScroll = true;

            if (_argsStartMinimised || _options.StartMinimised)
                MinimiseToTray(false);
        }

        private void InitLogging()
        {
            if (options.LogToFile)
            {
                if (_logging == null)
                {
                    _logging = new Logging(_logPath, options.VerboseLogging);

                    Log(String.Format("InitLogging -> SteamAccountSwitcher {0}", Program.GetVersion()));
                    Log(String.Format("InitLogging -> {0}", options.ToString()), Logging.Severity.DEBUG);
                }
                else if (_logging.DebugLogging != options.VerboseLogging)
                {
                    _logging.DebugLogging = options.VerboseLogging;
                    Log(String.Format("InitLogging -> {0}", options.ToString()), Logging.Severity.DEBUG);
                }
            }
            else _logging = null;
        }

        private void AddAccount(Int64 uID, string userName, string description, string customDisplayName = null, int pos = -1, bool loadingFromFile = false)
        {
            if (String.IsNullOrWhiteSpace(customDisplayName)) customDisplayName = userName;
            if (pos == -1) pos = accountsPanel.Controls.Count;
            Account newAccount = new Account(uID, userName, description, customDisplayName, pos);
            newAccount.Parent = this;
            Size currSize = newAccount.Size;
            if (accountsPanel.VerticalScroll.Visible) // Rather unelegant.
                currSize.Width = accountsPanel.Width - 2 - (SystemInformation.VerticalScrollBarWidth); 
            else
                currSize.Width = accountsPanel.Width - 2;
            newAccount.Size = currSize;

            if (!loadingFromFile) _accountXML.AddAccount(newAccount);

            accountsPanel.Controls.Add(newAccount);
            accountsPanel.Controls.SetChildIndex(newAccount, pos);

            Log(String.Format("AddedAccount (FromXML: {3}) -> '{0}', '{1}', '{2}'", uID, userName, description, loadingFromFile), Logging.Severity.INFO);

            UpdateUI();
        }

        private void LoadAccountsFromXML()
        {
            try
            {
                string currAccount = GetCurrentAutoLoginUsername();
                List<Account> accounts = _accountXML.LoadAllAccounts();

                if (accounts != null && accounts.Count > 0)
                {
                    accounts.Sort((i1, i2) => i1.Position.CompareTo(i2.Position));

                    if (accounts != null)
                        foreach (Account account in accounts)
                            AddAccount(account.UniqueID, account.Username, account.Description, account.CustomDisplayName, -1, true);

                    if (!String.IsNullOrWhiteSpace(currAccount))
                    {
                        Account acc = accountsPanel.Controls.OfType<Account>().First(s => s.Username == currAccount);
                        if (acc != null) SelectAccount(acc);
                    }
                }
            }
            catch (Exception e)
            {
                Log(String.Format("LoadAccountsFromXML -> An unexpected error occured! Exception: {0}", e.ToString()), Logging.Severity.ERROR);
            }
        }

        private string GetCurrentAutoLoginUsername()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("AutoLoginUser");
                        if (o != null)
                        {
                            string user = o.ToString();
                            Log(String.Format("GetCurrentAutoLoginUsername -> Found AutoLoginUser ('{0}')", user), Logging.Severity.DEBUG);
                            return user;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void ExecuteCmdCommand(string command, bool asAdmin = false)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            if (asAdmin) startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
        }

        private bool CloseSteam(ushort attempt = 0)
        {
            if (IsProcessOpen("Steam"))
            {
                Log(String.Format("CloseSteam -> Steam process is running!"), Logging.Severity.DEBUG);

                ExecuteCmdCommand("taskkill /F /IM GameOverlayUI.exe", false);
                ExecuteCmdCommand("taskkill /F /IM Steam.exe", false);
                attempt++;

                Thread.Sleep(options.ThreadPauseTime);
                if (IsProcessOpen("Steam"))
                {
                    Log(String.Format("CloseSteam -> Steam process is STILL running! (Close Attempts: {0})", attempt), Logging.Severity.DEBUG);
                    if (attempt < options.MaxCloseAttempts)
                        CloseSteam(attempt);
                    else if (MessageBox.Show("Steam could not be closed!\nAttempt to close as admin?", "Steam cannot be closed", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        CloseSteamAsAdmin();
                    else return false;
                }
                return true;
            }
            else Log(String.Format("CloseSteam -> Steam process is not running!", attempt), Logging.Severity.DEBUG);

            return true;
        }

        private bool CloseSteamAsAdmin(ushort attempt = 0)
        {
            if (IsProcessOpen("Steam"))
            {
                Log(String.Format("CloseSteamAsAdmin -> Steam process is running!"), Logging.Severity.DEBUG);

                try
                {
                    ExecuteCmdCommand("taskkill /F /IM GameOverlayUI.exe", true);
                    ExecuteCmdCommand("taskkill /F /IM Steam.exe", true);
                }
                catch
                {
                    Log(String.Format("CloseSteamAsAdmin -> Could not kill Steam process as admin!"), Logging.Severity.ERROR);
                }
                attempt++;

                Thread.Sleep(options.ThreadPauseTime);
                if (IsProcessOpen("Steam"))
                {
                    Log(String.Format("CloseSteamAsAdmin -> Steam process is STILL running! (Close Attempts: {0})", attempt), Logging.Severity.DEBUG);
                    if (attempt < options.MaxCloseAttempts)
                        CloseSteamAsAdmin(attempt);
                    else return false;
                }
            }
            else Log(String.Format("CloseSteam -> Steam process is not running!", attempt), Logging.Severity.DEBUG);

            return true;
        }

        private bool IsProcessOpen(string name)
        {
            try
            {
                return Process.GetProcessesByName(name).Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CloseForm(bool forceClose = false)
        {
            Log(String.Format("CloseForm (forceClose: {0}) -> Closing SAS", forceClose), Logging.Severity.INFO);

            if (forceClose) OnFormClosing(new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
            else OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        private void MinimiseToTray(bool showBalloon = true)
        {
            Log(String.Format("MinimiseToTray (showBalloon: {0}) -> SAS has been minimised to the tray!", showBalloon), Logging.Severity.DEBUG);

            if (showBalloon)
            {
                notifyIcon.BalloonTipText = " ";
                notifyIcon.BalloonTipTitle = "Steam Account Switcher is running in the background.";
                notifyIcon.ShowBalloonTip(500);
            }

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            _editAccountForm.Hide();
            this.Hide();
        }

        private void MaxmiseFromTray()
        {
            Log(String.Format("MaxmiseFromTray -> SAS has been maximised from the tray!"), Logging.Severity.DEBUG);

            this.Show();
            this.BringToFront();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }
        #endregion

        #region UI Methods
        private void AddAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
                MaxmiseFromTray();

            if (!_addAccountForm.Visible)
                _addAccountForm.Show(this);
            else
                _addAccountForm.BringToFront();
        }

        private void EditAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAccountEditor(_selectedAccount);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && options.CloseToTray)
            {
                e.Cancel = true;
                MinimiseToTray();
            }
            else
            {
                base.OnFormClosing(e);

                _editAccountForm.Dispose();
                this.Dispose();
            }
        }

        private void accountsPanel_Resize(object sender, EventArgs e)
        {
            foreach (Control control in accountsPanel.Controls)
            {
                Size size = control.Size;
                if (accountsPanel.VerticalScroll.Visible) // Rather unelegant.
                    size.Width = accountsPanel.Width - 2 - (SystemInformation.VerticalScrollBarWidth);
                else
                    size.Width = accountsPanel.Width - 2;
                control.Size = size;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (_selectedAccount != null)
                Login(_selectedAccount);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettings();
        }

        private void aboutSASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* TODO: Make About Form */
            Log(String.Format("AboutForm -> Showing about form"), Logging.Severity.DEBUG);
            MessageBox.Show(String.Format("Steam Account Switcher {0}", Program.GetVersion()), "About", MessageBoxButtons.OK);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visible)
                    MinimiseToTray(false);
                else
                    MaxmiseFromTray();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (this.Visible)
                {
                    MinimiseToTray();
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    MaxmiseFromTray();
                    this.BringToFront();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForm(true);
        }

        private void AccountSwitcher_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && options.MinimiseToTray)
                CloseForm();
        }

        private void AccountSwitcher_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                CloseForm();
        }
        #endregion
    }
}
