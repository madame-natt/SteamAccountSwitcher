using System;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    public partial class EditAccount : Form
    {
        public new AccountSwitcher Parent { get; set; }

        private Account _account;

        public EditAccount()
        {
            InitializeComponent();
        }

        public void SetAccount(Account account)
        {
            _account = account;

            displayNameTextbox.Text = _account.CustomDisplayName;
            usernameTextbox.Text = _account.Username;
            descTextbox.Text = _account.Description;
        }

        public Account GetAccount()
        {
            return _account;
        }

        private void ResetInput()
        {
            displayNameTextbox.ResetText();
            usernameTextbox.ResetText();
            descTextbox.ResetText();
        }

        private void CloseForm()
        {
            this.Hide();
            ResetInput();
            Parent.BringToFront();
        }

        private void EditAccountButton_Click(object sender, EventArgs e)
        {
            _account.Username = usernameTextbox.Text;
            _account.Description = descTextbox.Text;
            _account.CustomDisplayName = displayNameTextbox.Text;

            Parent.EditAccount(_account);
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

        private void usernameTextbox_TextChanged(object sender, EventArgs e)
        {
            editAccountButton.Enabled = !String.IsNullOrWhiteSpace(usernameTextbox.Text);
        }
    }
}
