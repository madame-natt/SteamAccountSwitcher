using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class AccountSwitcher : Form
    {
        private AddAccount addAccountForm = new AddAccount();
        private EditAccount editAccountForm = new EditAccount();

        private Account selectedAccount;

        private AccountXML accountXML = new AccountXML("accounts.xml");

        public AccountSwitcher()
        {
            InitializeComponent();
            LoadAccountsFromXML();

            editAccountForm.Parent = this;
            editAccountForm.Parent = this;

            accountsPanel.HorizontalScroll.Maximum = 0;
            accountsPanel.AutoScroll = false;
            accountsPanel.VerticalScroll.Visible = false;
            accountsPanel.AutoScroll = true;
        }

        public void SelectAccount(Account account)
        {
            if (selectedAccount != null) selectedAccount.Selected = false;
            selectedAccount = account;
            selectedAccount.Selected = true;
            loginButton.Enabled = true;
            editAccountToolStripMenuItem.Enabled = true;
            loginButton.Text = String.Format("Login ({0})", selectedAccount.CustomDisplayName);
        }

        public void UnselectAccount()
        {
            if (selectedAccount != null) selectedAccount.Selected = false;
            selectedAccount = null;
            loginButton.Enabled = false;
            editAccountToolStripMenuItem.Enabled = false;
            loginButton.Text = "Login";
        }

        public void UpdateUI()
        {
            foreach (Account acc in accountsPanel.Controls)
            {
                acc.UpdateUI();
            }
        }

        public void MoveAccountUp(Account account)
        {
            accountsPanel.Controls.SetChildIndex(account, accountsPanel.Controls.GetChildIndex(account) - 1);
            EditAllAccounts();
            UpdateUI();
        }

        public void MoveAccountDown(Account account)
        {
            accountsPanel.Controls.SetChildIndex(account, accountsPanel.Controls.GetChildIndex(account) + 1);
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

            if (!loadingFromFile) accountXML.AddAccount(newAccount);
            accountsPanel.Controls.Add(newAccount);
            accountsPanel.Controls.SetChildIndex(newAccount, pos);
            UpdateUI();
        }

        private void LoadAccountsFromXML()
        {
            List<Account> accounts = accountXML.LoadAllAccounts();
            accounts.Sort((i1, i2) => i1.Position.CompareTo(i2.Position));

            if (accounts != null)
            {
                foreach (Account account in accounts)
                {
                    AddAccount(account.UniqueID, account.Username, account.Description, account.CustomDisplayName, -1, true);
                }
            }
        }

        public void OpenAccountEditor(Account account)
        {
            if (!editAccountForm.Visible)
            {
                Point currLoc = this.Location;
                currLoc.X = currLoc.X + (int)Math.Floor((double)this.Size.Width / 8.0);
                currLoc.Y = currLoc.Y + (int)Math.Floor((double)this.Size.Height / 8.0);

                editAccountForm.Location = currLoc;
                editAccountForm.Parent = this;
                editAccountForm.SetAccount(account);
                editAccountForm.Show(this);
            }
            else if (editAccountForm.GetAccount() != null && editAccountForm.GetAccount() == account)
            {
                editAccountForm.BringToFront();
            }
            else
            {
                editAccountForm.SetAccount(account);
                editAccountForm.BringToFront();
            }
        }

        public void EditAccount(Account account)
        {
            accountXML.EditAccount(account);
        }

        public void EditAllAccounts()
        {
            foreach (Account account in accountsPanel.Controls)
            {
                account.Position = accountsPanel.Controls.GetChildIndex(account);
                accountXML.EditAccount(account);
            }
        }

        public void RemoveAccount(Account account)
        {
            if (selectedAccount == account) UnselectAccount();
            accountXML.RemoveAccount(account);
            account.Dispose();
            UpdateUI();
        }

        public void Login(Account account)
        {
            CloseSteam();
            Thread.Sleep(500);

            ExecuteCmdCommand(String.Format("reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f", account.Username));
            ExecuteCmdCommand("reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f");
            ExecuteCmdCommand("start steam://open/main"); // Add option to launch directly with EXE
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
            if (IsProcessOpen("csgo") || IsProcessOpen("Steam"))
            {
                ExecuteCmdCommand("taskkill /F /IM csgo.exe");
                ExecuteCmdCommand("taskkill /F /IM GameOverlayUI.exe");
                ExecuteCmdCommand("taskkill /F /IM Steam.exe");
                ExecuteCmdCommand("timeout /t 3");

                if ((IsProcessOpen("csgo") || IsProcessOpen("Steam")) && !alreadyRan)
                    CloseSteam(true, true);
                else { /* Show Error if failed */ }
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

        private void AddAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!editAccountForm.Visible)
            {
                Point currLoc = this.Location;
                currLoc.X = currLoc.X + (int)Math.Floor((double)this.Size.Width / 8.0);
                currLoc.Y = currLoc.Y + (int)Math.Floor((double)this.Size.Height / 8.0);

                editAccountForm.Location = currLoc;
                editAccountForm.Parent = this;
                editAccountForm.Show(this);
            }
            else editAccountForm.BringToFront();
        }

        private void EditAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAccountEditor(selectedAccount);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            editAccountForm.Dispose();
            this.Dispose();
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
            if (selectedAccount != null)
                Login(selectedAccount);
        }
    }
}
