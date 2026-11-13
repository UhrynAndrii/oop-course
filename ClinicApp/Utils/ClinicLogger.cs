using System;
using System.IO;
using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;

        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        private void WriteLog(string level, string message)
        {
            string logEntry = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                "] [" + level.PadRight(5) + "] " + message;

            File.AppendAllText(_logPath, logEntry + Environment.NewLine, Encoding.UTF8);
        }

        public string[] GetLastLines(int n)
        {
            if (!File.Exists(_logPath) || n <= 0)
            {
                return new string[0];
            }

            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);

            int startIndex = lines.Length - n;

            if (startIndex < 0)
            {
                startIndex = 0;
            }

            int resultLength = lines.Length - startIndex;
            string[] result = new string[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                result[i] = lines[startIndex + i];
            }

            return result;
        }

        public void Clear()
        {
            if (File.Exists(_logPath))
            {
                File.Delete(_logPath);
            }
        }

        public bool Exists()
        {
            return File.Exists(_logPath);
        }
    }
}