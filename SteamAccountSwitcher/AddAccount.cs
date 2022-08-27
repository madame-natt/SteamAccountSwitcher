using System;
using System.Drawing;
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
			Hide();
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

			if (e.CloseReason == CloseReason.WindowsShutDown)
				return;

			e.Cancel = true;
			CloseForm();
		}

		private void usernameTextbox_TextChanged(object sender, EventArgs e)
		{
			addAccountButton.Enabled = !String.IsNullOrWhiteSpace(usernameTextbox.Text);
		}

		private void AddAccount_Load(object sender, EventArgs e)
		{
			if (Owner != null)
				Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
		}
	}
}
