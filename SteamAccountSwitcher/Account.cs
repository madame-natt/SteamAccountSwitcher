using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using SteamAccountSwitcher.Utils;

namespace SteamAccountSwitcher
{
	public partial class Account : UserControl
	{
		public new AccountSwitcher Parent { get; set; }
		private bool selected;

		public Account()
		{
			InitializeComponent();
		}

		public Account(long uID) : this()
		{
			UniqueID = uID;
		}

		public override string ToString()
		{
			return $"UID: {UniqueID}, Username: {Username}, CustomDisplayName: {CustomDisplayName}, Description: {Description}";
		}

		public Account(long uID, string userName, string description, int pos) : this(uID)
		{
			Username = userName;
			Description = description;
			if (string.IsNullOrWhiteSpace(CustomDisplayName)) CustomDisplayName = Username;
			Position = pos;
		}

		public Account(long uID, string userName, string description, string customDisplayName, int pos) : this(uID, userName, description, pos)
		{
			CustomDisplayName = customDisplayName;
		}

		public void Login()
		{
			Task threadedLogin = new Task(() =>
			{
				if (!Parent.CloseSteam())
				{
					MessageBox.Show("Steam could not be closed!\nPlease relaunch Steam manually to switch accounts.", "Steam cannot be closed", MessageBoxButtons.OK);
					Parent.Log("Login -> Steam could not be closed!", Logging.Severity.WARN);
				}
				Thread.Sleep(500);

				RegistryKey steamRegKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam", true);
				steamRegKey?.SetValue("AutoLoginUser", Username, RegistryValueKind.String);
				steamRegKey?.SetValue("RememberPassword", 1, RegistryValueKind.DWord);

				Parent.Log("Login -> Added reg keys (AutoLoginUser & RememberPassword)", Logging.Severity.DEBUG);

				if (Parent.options.UseSteamPath)
				{
					Win32.ExecuteCommand(Parent.options.SteamPath);
					Parent.Log("Login -> Steam started using Steam Path", Logging.Severity.DEBUG);
				}
				else
				{
					Win32.ExecuteCommand("start steam://open/main");
					Parent.Log("Login -> Steam started using 'steam://open/main'", Logging.Severity.DEBUG);
				}
			});
			threadedLogin.Start();
		}

		public long UniqueID { get; }

		public string Username
		{
			get => toolTip.GetToolTip(userNameLabel);
			set => toolTip.SetToolTip(userNameLabel, value);
		}

		public string CustomDisplayName
		{
			get => userNameLabel.Text;
			set => userNameLabel.Text = value;
		}

		public string Description
		{
			get => descriptionLabel.Text;
			set
			{
				descriptionLabel.Text = value;
				toolTip.SetToolTip(descriptionLabel, value);
			}
		}

		public bool Selected
		{
			get => selected;
			set
			{
				SetColour(value ? SystemColors.ActiveBorder : SystemColors.Control);
				selected = value;
			}
		}

		public int Position { get; set; }

		public void UpdateUI()
		{
			upPosButton.Enabled = !Parent.IsAccountTop(this);
			downPosButton.Enabled = !Parent.IsAccountBottom(this);
		}

		private void SetColour(Color color)
		{
			userNameLabel.BackColor = color;
			userNamePanel.BackColor = color;

			descriptionLabel.BackColor = color;
			descriptionPanel.BackColor = color;

			utilsPanel.BackColor = color;
		}

		private void accountInterface_Click(object sender, EventArgs e)
		{
			Parent.SelectAccount(this);
		}

		private void accountInterface_DoubleClick(object sender, EventArgs e)
		{
			Login();
		}

		private void deleteAccount_Click(object sender, EventArgs e)
		{
			Parent.RemoveAccount(this);
		}

		private void editButton_Click(object sender, EventArgs e)
		{
			Parent.OpenAccountEditor(this);
		}

		private void upPosButton_Click(object sender, EventArgs e)
		{
			Parent.MoveAccountBy(this, -1);
		}

		private void downPosButton_Click(object sender, EventArgs e)
		{
			Parent.MoveAccountBy(this, 1);
		}
	}
}
