using System;
using System.IO;

namespace SteamAccountSwitcher
{
	public class Logging
	{
		private string _path;

		public Logging(string path, bool debugLogging = false)
		{
			_path = path;
			DebugLogging = debugLogging;
		}

		public bool DebugLogging { get; set; }

		public void Log(string log, Severity severity)
		{
			switch (severity)
			{
				case Severity.DEBUG:
					if (DebugLogging) LogToFile($"[{GetTimestamp()}] [DEBUG]: {log}");
					break;
				case Severity.INFO:
					LogToFile($"[{GetTimestamp()}] [INFO]: {log}");
					break;
				case Severity.WARN:
					LogToFile($"[{GetTimestamp()}] [WARN]: {log}");
					break;
				case Severity.ERROR:
					LogToFile($"[{GetTimestamp()}] [ERROR]: {log}");
					break;
			}
		}

		private void LogToFile(string message)
		{
			File.AppendAllText(_path, $"{message}\r\n");
		}

		private string GetTimestamp()
		{
			return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff tt");
		}

		public enum Severity
		{
			DEBUG,
			INFO,
			WARN,
			ERROR
		};
	}
}
