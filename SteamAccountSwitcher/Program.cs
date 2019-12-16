using System;
using System.Reflection;
using System.Windows.Forms;

namespace SteamAccountSwitcher
{
    static class Program
    {
        private const bool IsPreReleaseBuild = true;
        private const string PreReleaseTag = "DEV_191216-1";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AccountSwitcher());
        }

        public static string GetVersion()
        {
            #pragma warning disable CS0162 // Unreachable code detected
            string[] ver = (typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
            if (!IsPreReleaseBuild)
                return "v" + ver[0] + "." + ver[1] + "." + ver[2];
            else
                return "v" + ver[0] + "." + ver[1] + "." + ver[2] + "-" + PreReleaseTag;
            #pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
