using System;
using System.Diagnostics;
using System.Linq;

namespace SteamAccountSwitcher.Utils
{
	public class Win32
	{
		public static void ExecuteCommand(string command, bool asAdmin = false)
		{
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				FileName = "cmd.exe",
				Arguments = "/C " + command,
				Verb = asAdmin ? "runas" : string.Empty
			};
			process.StartInfo = startInfo;
			process.Start();
		}

		public static void KillTask(string taskName, bool force = false, bool asAdmin = false)
		{
			ExecuteCommand($"taskkill {(force ? "/F" : string.Empty)} /IM {taskName}", asAdmin);
		}

		public static bool IsProcessOpen(string name)
		{
			try
			{
				return Process.GetProcessesByName(name).Any();
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
