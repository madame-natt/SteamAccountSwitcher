using System;
using System.IO;

namespace SteamAccountSwitcher
{
    public class Logging
    {
        private string _path;
        private bool _debugLogging;

        public Logging(string path, bool debugLogging = false)
        {
            _path = path;
            _debugLogging = debugLogging;
        }

        public bool DebugLogging
        {
            set
            {
                _debugLogging = value;
            }
            get
            {
                return _debugLogging;
            }
        }

        public void Log(string log, Severity severity)
        {
            switch (severity)
            {
                case Severity.DEBUG:
                    if (_debugLogging) LogToFile(String.Format("[{0}] [DEBUG]: {1}", GetTimestamp(), log));
                    break;
                case Severity.INFO:
                    LogToFile(String.Format("[{0}] [INFO]: {1}", GetTimestamp(), log));
                    break;
                case Severity.WARN:
                    LogToFile(String.Format("[{0}] [WARN]: {1}", GetTimestamp(), log));
                    break;
                case Severity.ERROR:
                    LogToFile(String.Format("[{0}] [ERROR]: {1}", GetTimestamp(), log));
                    break;
                default:
                    break;
            }
        }

        private void LogToFile(string message)
        {
            File.AppendAllText(_path, String.Format("{0}\r\n", message));
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
