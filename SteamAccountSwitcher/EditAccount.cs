using System;
using System.Drawing;
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

		public Account Account
		{
			get => _account;
			set
			{
				_account = value;

				displayNameTextbox.Text = _account.CustomDisplayName;
				usernameTextbox.Text = _account.Username;
				descTextbox.Text = _account.Description;
			}
		}

		private void ResetInput()
		{
			displayNameTextbox.ResetText();
			usernameTextbox.ResetText();
			descTextbox.ResetText();
		}

		private void CloseForm()
		{
			Hide();
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

			if (e.CloseReason == CloseReason.WindowsShutDown)
				return;

			e.Cancel = true;
			CloseForm();
		}

		private void usernameTextbox_TextChanged(object sender, EventArgs e)
		{
			editAccountButton.Enabled = !String.IsNullOrWhiteSpace(usernameTextbox.Text);
		}

		private void EditAccount_Load(object sender, EventArgs e)
		{
			if (Owner != null)
				Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
		}
	}
}
