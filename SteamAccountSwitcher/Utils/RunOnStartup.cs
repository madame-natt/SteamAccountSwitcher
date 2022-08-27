using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace SteamAccountSwitcher.RegistryUtils
{
	public class RunOnStartup
	{
		private static readonly RegistryKey _regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
		private static readonly string _appTitle = Application.ProductName;
		private static readonly string _appPath = Application.ExecutablePath;

		public static bool AddToStartup()
		{
			try
			{
				_regKey?.SetValue(_appTitle, _appPath);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool RemoveFromStartup()
		{
			string appTitle = Application.ProductName;

			try
			{
				_regKey?.DeleteValue(appTitle);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static void SetStartupToggle(bool value)
		{
			bool isAddedToStartup = IsAddedToStartup();
			switch (value)
			{
				case true when !isAddedToStartup:
					AddToStartup();
					break;
				case false when isAddedToStartup:
					RemoveFromStartup();
					break;
			}
		}

		public static bool IsAddedToStartup()
		{
			return _regKey.GetValue(_appTitle) != null;
		}
	}
}
