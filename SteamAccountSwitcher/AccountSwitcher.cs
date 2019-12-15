using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class AccountSwitcher : Form
    {
        private AddAccount accountForm = new AddAccount();
        private Account selectedAccount;

        public AccountSwitcher()
        {
            InitializeComponent();
            UnselectAccount();

            accountForm.Parent = this;

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
            loginButton.Text = String.Format("Login ({0})", selectedAccount.Username);
        }

        public void UnselectAccount()
        {
            if (selectedAccount != null) selectedAccount.Selected = false;
            selectedAccount = null;
            loginButton.Enabled = false;
            loginButton.Text = "Login";
        }

        public void AddAccount(string userName, string description)
        {
            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            AddAccount(unixTimestamp, userName, description);
        }

        private void AddAccount(Int64 uID, string userName, string description)
        {
            Account newAccount = new Account(uID, userName, description);
            newAccount.Parent = this;
            Size currSize = newAccount.Size;
            if (accountsPanel.VerticalScroll.Visible) // Rather unelegant.
                currSize.Width = accountsPanel.Width - 2 - (SystemInformation.VerticalScrollBarWidth); 
            else
                currSize.Width = accountsPanel.Width - 2;
            newAccount.Size = currSize;

            accountsPanel.Controls.Add(newAccount);
        }

        public void RemoveAccount(Account account)
        {
            if (selectedAccount == account) UnselectAccount();
            account.Dispose();
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
            if (!accountForm.Visible)
            {
                Point currLoc = this.Location;
                currLoc.X = currLoc.X + (int)Math.Floor((double)this.Size.Width / 8.0);
                currLoc.Y = currLoc.Y + (int)Math.Floor((double)this.Size.Height / 8.0);

                accountForm.Location = currLoc;
                accountForm.Parent = this;
                accountForm.Show(this);
            }
            else accountForm.BringToFront();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            accountForm.Dispose();
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
            CloseSteam();
            Thread.Sleep(500);

            ExecuteCmdCommand(String.Format("reg add \"HKCU\\Software\\Valve\\Steam\" /v AutoLoginUser /t REG_SZ /d {0} /f", selectedAccount.Username));
            ExecuteCmdCommand("reg add \"HKCU\\Software\\Valve\\Steam\" /v RememberPassword /t REG_DWORD /d 1 /f");
            ExecuteCmdCommand("start steam://open/main"); // Add option to launch directly with EXE
        }
    }
}
