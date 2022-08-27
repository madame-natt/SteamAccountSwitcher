using System.Reflection;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
	static class Program
	{
		private const bool IsPreReleaseBuild = true;
		private const string PreReleaseTag = "BETA-1";

		static void Main(string[] args)
		{
			bool startMinimised = false;
			foreach (string arg in args)
			{
				string param = arg.TrimStart('/', '-').Trim();

				if (param == "minimised" || param == "minimized" || param == "mini")
				{
					startMinimised = true;
				}
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new AccountSwitcher(startMinimised));
		}

		public static string GetVersion()
		{
			#pragma warning disable CS0162 // Unreachable code detected
			string[] ver = (typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
			if (!IsPreReleaseBuild)
				return "v" + ver[0] + "." + ver[1] + "." + ver[2];
			return "v" + ver[0] + "." + ver[1] + "." + ver[2] + "-" + PreReleaseTag;
			#pragma warning restore CS0162 // Unreachable code detected
		}
	}
}
