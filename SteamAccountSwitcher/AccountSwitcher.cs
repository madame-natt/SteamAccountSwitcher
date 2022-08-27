using Microsoft.Win32;
using SimpleSettingsManager;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using SteamAccountSwitcher.Utils;

namespace SteamAccountSwitcher
{
	public partial class AccountSwitcher : Form
	{
		#region Variables
		private readonly AddAccount _addAccountForm = new AddAccount();
		private readonly EditAccount _editAccountForm = new EditAccount();
		private readonly Settings _options = new Settings();
		//private FirstLaunch _firstLaunch;

		private string _defaultConfigPath = Environment.CurrentDirectory;
		private string _logPath;
		private readonly bool _argsStartMinimised;

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
			get => _ssm;
		}

		public Settings options
		{
			get => _options;
		}
		#endregion

		#region Public Methods
		public void Log(string log, Logging.Severity severity = Logging.Severity.INFO)
		{
			if (_logging != null)
				_logging.Log(log, severity);
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
			Log("OptionsForm -> Showing options form", Logging.Severity.DEBUG);

			if (!this.Visible)
				MaxmiseFromTray();

			if (!options.Visible)
				options.OpenForm(this);
			else
				options.BringToFront();
		}

		public void AddAccount(string userName, string description)
		{
			AddAccount(DateTimeOffset.UtcNow.ToUnixTimeSeconds(), userName, description);
		}

		public void SelectAccount(Account account)
		{
			if (_selectedAccount != null) _selectedAccount.Selected = false;
			_selectedAccount = account;
			_selectedAccount.Selected = true;
			loginButton.Enabled = true;
			editAccountToolStripMenuItem.Enabled = true;
			loginButton.Text = $"Login ({_selectedAccount.CustomDisplayName})";

			Log($"SelectAccount -> Changed to ({_selectedAccount})", Logging.Severity.DEBUG);
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
			Log($"EditAccount -> Editing account '{account.Username}'", Logging.Severity.DEBUG);
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
			return accountsPanel.Controls.GetChildIndex(account) == accountsPanel.Controls.Count - 1;
		}

		public void UpdateUI()
		{
			contextMenuStrip.Items.Clear();
			InitLogging();

			foreach (Account acc in accountsPanel.Controls)
			{
				acc.UpdateUI();
				contextMenuStrip.Items.Add(acc.CustomDisplayName, null, delegate { SelectAccount(acc); acc.Login(); }).ToolTipText = acc.Username;
			}

			contextMenuStrip.Items.Add("-");
			contextMenuStrip.Items.Add("Options", null, delegate { OpenSettings(); }).ToolTipText = "Open the options";
			contextMenuStrip.Items.Add("-");
			contextMenuStrip.Items.Add("Exit", null, delegate { CloseForm(true); }).ToolTipText = "Close the program";
		}

		public bool CloseSteam(ushort attempt = 0)
		{
			if (Win32.IsProcessOpen("Steam"))
			{
				Log("CloseSteam -> Steam process is running!", Logging.Severity.DEBUG);
				CloseSteamTasks();

				attempt++;

				Thread.Sleep(options.ThreadPauseTime);
				if (Win32.IsProcessOpen("Steam"))
				{
					Log($"CloseSteam -> Steam process is STILL running! (Close Attempts: {attempt})", Logging.Severity.DEBUG);
					if (attempt < options.MaxCloseAttempts)
						CloseSteam(attempt);
					else if (MessageBox.Show("Steam could not be closed!\nAttempt to close as admin?", "Steam cannot be closed", MessageBoxButtons.YesNo) == DialogResult.Yes)
						CloseSteamAsAdmin();
					else return false;
				}
				return true;
			}
			Log("CloseSteam -> Steam process is not running!", Logging.Severity.DEBUG);

			return true;
		}

		public void CloseSteamAsAdmin(ushort attempt = 0)
		{
			if (Win32.IsProcessOpen("Steam"))
			{
				Log("CloseSteamAsAdmin -> Steam process is running!", Logging.Severity.DEBUG);
				try
				{
					CloseSteamTasks(true);
				}
				catch
				{
					Log("CloseSteamAsAdmin -> Could not kill Steam process as admin!", Logging.Severity.ERROR);
				}
				attempt++;

				Thread.Sleep(options.ThreadPauseTime);
				if (Win32.IsProcessOpen("Steam"))
				{
					Log($"CloseSteamAsAdmin -> Steam process is STILL running! (Close Attempts: {attempt})", Logging.Severity.DEBUG);
					if (attempt < options.MaxCloseAttempts)
						CloseSteamAsAdmin(attempt);
				}
			}
			else
				Log("CloseSteam -> Steam process is not running!", Logging.Severity.DEBUG);
		}
		#endregion

		#region Private Methods
		private void Init()
		{
			InitializeComponent();

			string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "mullak99", "SteamAccountSwitcher");

			if (File.Exists(Path.Combine(Environment.CurrentDirectory, "accounts.xml")) && File.Exists(Path.Combine(Environment.CurrentDirectory, "settings.xml")))
				_defaultConfigPath = Environment.CurrentDirectory;
			else if (File.Exists(Path.Combine(documentsPath, "accounts.xml")) && File.Exists(Path.Combine(documentsPath, "settings.xml")))
				_defaultConfigPath = documentsPath;

			_accountXML = new AccountXML(Path.Combine(_defaultConfigPath, "accounts.xml"), this);
			_ssm = new SSM(new SSM_File(Path.Combine(_defaultConfigPath, "settings.xml"), SSM_File.Mode.XML));
			_logPath = Path.Combine(_defaultConfigPath, "SAS.log");

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

			// TODO: Implement First Launch Screen
			//if (!_options.HasSeenFirstLaunchScreen)
			//{
			//	_firstLaunch = new FirstLaunch();
			//	_firstLaunch.Parent = this;
			//	_firstLaunch.Show(this);
			//}
		}

		private void InitLogging()
		{
			if (options.LogToFile)
			{
				if (_logging == null)
				{
					_logging = new Logging(_logPath, options.VerboseLogging);

					Log($"InitLogging -> SteamAccountSwitcher {Program.GetVersion()}");
					Log($"InitLogging -> {options}", Logging.Severity.DEBUG);
				}
				else if (_logging.DebugLogging != options.VerboseLogging)
				{
					_logging.DebugLogging = options.VerboseLogging;
					Log($"InitLogging -> {options}", Logging.Severity.DEBUG);
				}
			}
			else _logging = null;
		}

		private void AddAccount(long uID, string userName, string description, string customDisplayName = null, int pos = -1, bool loadingFromFile = false)
		{
			if (string.IsNullOrWhiteSpace(customDisplayName)) customDisplayName = userName;
			if (pos == -1) pos = accountsPanel.Controls.Count;

			Account newAccount = new Account(uID, userName, description, customDisplayName, pos);
			newAccount.Parent = this;
			Size currSize = newAccount.Size;

			if (accountsPanel.VerticalScroll.Visible) // Rather unelegant.
				currSize.Width = accountsPanel.Width - 2 - (SystemInformation.VerticalScrollBarWidth);
			else
				currSize.Width = accountsPanel.Width - 2;
			newAccount.Size = currSize;

			if (!loadingFromFile)
				_accountXML.AddAccount(newAccount);

			accountsPanel.Controls.Add(newAccount);
			accountsPanel.Controls.SetChildIndex(newAccount, pos);

			Log($"AddedAccount (FromXML: {loadingFromFile}) -> '{uID}', '{userName}', '{description}'");

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

					foreach (Account account in accounts)
						AddAccount(account.UniqueID, account.Username, account.Description, account.CustomDisplayName, -1, true);

					if (!string.IsNullOrWhiteSpace(currAccount))
					{
						Account acc = accountsPanel.Controls.OfType<Account>().First(s => s.Username == currAccount);
						SelectAccount(acc);
					}
				}
			}
			catch (Exception e)
			{
				Log($"LoadAccountsFromXML -> An unexpected error occurred! Exception: {e}", Logging.Severity.ERROR);
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
							Log($"GetCurrentAutoLoginUsername -> Found AutoLoginUser ('{user}')", Logging.Severity.DEBUG);
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

		private void CloseSteamTasks(bool isAdmin = false)
		{
			Win32.KillTask("GameOverlayUI.exe", true, isAdmin);
			Win32.KillTask("Steam.exe", true, isAdmin);
		}

		private void CloseForm(bool forceClose = false)
		{
			Log($"CloseForm (forceClose: {forceClose}) -> Closing SAS");

			OnFormClosing(forceClose
				? new FormClosingEventArgs(CloseReason.ApplicationExitCall, false)
				: new FormClosingEventArgs(CloseReason.UserClosing, false));
		}

		private void MinimiseToTray(bool showBalloon = true)
		{
			Log($"MinimiseToTray (showBalloon: {showBalloon}) -> SAS has been minimised to the tray!", Logging.Severity.DEBUG);

			if (showBalloon)
			{
				notifyIcon.BalloonTipText = " ";
				notifyIcon.BalloonTipTitle = "Steam Account Switcher is running in the background.";
				notifyIcon.ShowBalloonTip(500);
			}

			WindowState = FormWindowState.Minimized;
			ShowInTaskbar = false;
			_editAccountForm.Hide();
			Hide();
		}

		private void MaxmiseFromTray()
		{
			Log("MaxmiseFromTray -> SAS has been maximised from the tray!", Logging.Severity.DEBUG);

			Show();
			BringToFront();
			ShowInTaskbar = true;
			WindowState = FormWindowState.Normal;
		}
		#endregion

		#region UI Methods
		private void AddAccountToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Visible)
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
				Dispose();
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
			_selectedAccount?.Login();
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenSettings();
		}

		private void aboutSASToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/* TODO: Make About Form */
			Log("AboutForm -> Showing about form", Logging.Severity.DEBUG);
			MessageBox.Show($"Steam Account Switcher {Program.GetVersion()}", "About", MessageBoxButtons.OK);
		}

		private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (Visible)
					MinimiseToTray(false);
				else
					MaxmiseFromTray();
			}
		}

		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				if (Visible)
					MinimiseToTray();
				else
				{
					WindowState = FormWindowState.Normal;
					MaxmiseFromTray();
					BringToFront();
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseForm(true);
		}

		private void AccountSwitcher_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized && options.MinimiseToTray)
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
