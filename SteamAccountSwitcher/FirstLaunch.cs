using System;
using System.Drawing;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
	public partial class FirstLaunch : Form
	{
		public new AccountSwitcher Parent { get; set; }

		public FirstLaunch()
		{
			InitializeComponent();
		}

		private void ResetInput()
		{

		}

		private void CloseForm()
		{
			this.Hide();
			ResetInput();
			Parent.BringToFront();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			if (e.CloseReason == CloseReason.WindowsShutDown)return;
			else
			{
				e.Cancel = true;
				CloseForm();
			}
		}

		private void AddAccount_Load(object sender, EventArgs e)
		{
			if (Owner != null)
				Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
		}
	}
}
