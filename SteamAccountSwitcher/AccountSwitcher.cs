using Microsoft.Win32;
using SimpleSettingsManager;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;

namespace SteamAccountSwitcher
{
    public partial class AccountSwitcher : Form
    {
        private AddAccount _addAccountForm = new AddAccount();
        private EditAccount _editAccountForm = new EditAccount();
        public Settings options = new Settings();

        private Account _selectedAccount;

        private AccountXML _accountXML;
        public SSM ssm;

        private bool _argsStartMinimised = false;

        public AccountSwitcher()
        {
            InitializeComponent();
            Init();
        }

        public AccountSwitcher(bool startMinimised)
        {
            _argsStartMinimised = startMinimised;
            InitializeComponent();
            Init();
        }

        public AccountSwitcher(string accountXML, string settingsXML) : this()
        {
            _accountXML = new AccountXML(accountXML);
            ssm = new SSM(new SSM_File(settingsXML, SSM_File.Mode.XML));
        }

        public void SelectAccount(Account account)
        {
            if (_selectedAccount != null) _selectedAccount.Selected = false;
            _selectedAccount = account;
            _selectedAccount.Selected = true;
            loginButton.Enabled = true;
            editAccountToolStripMenuItem.Enabled = true;
            loginButton.Text = String.Format("Login ({0})", _selectedAccount.CustomDisplayName);
        }

        public void UnselectAccount()
        {
            if (_selectedAccount != null) _selectedAccount.Selected = false;
            _selectedAccount = null;
            loginButton.Enabled = false;
            editAccountToolStripMenuItem.Enabled = false;
            loginButton.Text = "Login";
        }

        public void UpdateUI()
        {
            contextMenuStrip.Items.Clear();

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

        public void AddAccount(string userName, string description)
        {
            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            AddAccount(unixTimestamp, userName, description);
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

            UpdateUI();
        }

        private void LoadAccountsFromXML()
        {
            string currAccount = GetCurrentAutoLoginUsername();
            List<Account> accounts = _accountXML.LoadAllAccounts();
            accounts.Sort((i1, i2) => i1.Position.CompareTo(i2.Position));

            if (accounts != null)
            {
                foreach (Account account in accounts)
                {
                    AddAccount(account.UniqueID, account.Username, account.Description, account.CustomDisplayName, -1, true);
                }
            }

            if (!String.IsNullOrWhiteSpace(currAccount))
            {
                Account acc = accountsPanel.Controls.OfType<Account>().First(s => s.Username == currAccount);
                if (acc != null) SelectAccount(acc);
            }
        }

        private void Init()
        {
            if (_accountXML == null)
                _accountXML = new AccountXML("accounts.xml");
            if (ssm == null)
                ssm = new SSM(new SSM_File("settings.xml", SSM_File.Mode.XML));

            _addAccountForm.Parent = this;
            _addAccountForm.Owner = this;
            _editAccountForm.Parent = this;
            _editAccountForm.Owner = this;
            options.Parent = this;
            options.Owner = this;

            ssm.Open();
            options.InitSettings();
            LoadAccountsFromXML();

            accountsPanel.HorizontalScroll.Maximum = 0;
            accountsPanel.AutoScroll = false;
            accountsPanel.VerticalScroll.Visible = false;
            accountsPanel.AutoScroll = true;

            if (_argsStartMinimised || options.StartMinimised)
                MinimiseToTray(false);
        }

        public void OpenAccountEditor(Account account)
        {
            if (!_editAccountForm.Visible)
            {
                _editAccountForm.SetAccount(account);
                _editAccountForm.Show(this);
            }
            else if (_editAccountForm.GetAccount() != null && _editAccountForm.GetAccount() == account)
            {
                _editAccountForm.BringToFront();
            }
            else
            {
                _editAccountForm.SetAccount(account);
                _editAccountForm.BringToFront();
            }
        }

        public void OpenSettings()
        {
            if (!this.Visible)
                MaxmiseFromTray();

            if (!options.Visible)
                options.Show(this);
            else
                options.BringToFront();
        }

        public void EditAccount(Account account)
        {
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

        public void Login(Account account)
        {
            CloseSteam();
            Thread.Sleep(500);

            ExecuteCmdCommand(String.Format("reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f", account.Username));
            ExecuteCmdCommand("reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f");

            if (options.UseSteamPath)
                ExecuteCmdCommand(options.SteamPath);
            else
                ExecuteCmdCommand("start steam://open/main");
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
                            return o.ToString();
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

        private void CloseSteam(bool asAdmin = false, bool alreadyRan = false)
        {
            if (IsProcessOpen("Steam"))
            {
                ExecuteCmdCommand("taskkill /F /IM GameOverlayUI.exe", asAdmin);
                ExecuteCmdCommand("taskkill /F /IM Steam.exe", asAdmin);
                ExecuteCmdCommand("timeout /t 3");

                if ((IsProcessOpen("Steam")) && !alreadyRan)
                    CloseSteam(true, true);
                else if (!IsRunAsAdmin())
                {
                    if (MessageBox.Show("Steam could not be closed!\nAttempt to close as admin?", "Steam cannot be closed", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        RunAsAdmin();
                }
                else
                    MessageBox.Show("Steam could not be closed!\nPlease relaunch Steam manually to switch accounts.", "Steam cannot be closed", MessageBoxButtons.OK);
            }
        }

        private bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name)) return true;
            }
            return false;
        }

        internal bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RunAsAdmin()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Application.ExecutablePath;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    return;
                }

                CloseForm(true);
            }
        }

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

        private void CloseForm(bool forceClose = false)
        {
            if (forceClose) OnFormClosing(new FormClosingEventArgs(CloseReason.ApplicationExitCall, false));
            else OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false));
        }

        public void MinimiseToTray(bool showBalloon = true)
        {
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
            this.Show();
            this.BringToFront();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
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
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
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
    }
}
