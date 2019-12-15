using System;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class AddAccount : Form
    {
        public new AccountSwitcher Parent { get; set; }

        public AddAccount()
        {
            InitializeComponent();
        }

        private void ResetInput()
        {
            usernameTextbox.ResetText();
            descTextbox.ResetText();
        }

        private void CloseForm()
        {
            this.Hide();
            ResetInput();
            Parent.BringToFront();
        }

        private void AddAccountButton_Click(object sender, EventArgs e)
        {
            Parent.AddAccount(usernameTextbox.Text, descTextbox.Text);
            CloseForm();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CloseForm();
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
