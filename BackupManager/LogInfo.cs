using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupManager
{
    public static class LogInfo
    {
        static EventLog log;

        public static void LogErrorWrite(string message)
        {
            log = new EventLog();
            string sLog = "Application";
            string logSource = "BackupManager";
            log.Source = logSource;
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, sLog);

            log.WriteEntry(message, EventLogEntryType.Error);
        }

        public static void LogErrorWrite(Exception ex)
        {
            log = new EventLog();
            string sLog = "Application";
            string logSource = "BackupManager";
            log.Source = logSource;
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, sLog);

            string message = ex.Message;
            message += "StackTrace: " + Environment.NewLine;
            message += ex.StackTrace;

            log.WriteEntry(message, EventLogEntryType.Error);
        }

        public static void LogErrorWrite(string message, Exception ex)
        {
            log = new EventLog();
            string sLog = "Application";
            string logSource = "BackupManager";
            log.Source = logSource;
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, sLog);

            message += Environment.NewLine;
            message += ex.Message + Environment.NewLine;
            message += "StackTrace: " + Environment.NewLine;
            message += ex.StackTrace;

            log.WriteEntry(message, EventLogEntryType.Error);
        }

        public static void LogWarningWrite(string message)
        {
            log = new EventLog();
            string sLog = "Application";
            string logSource = "BackupManager";
            log.Source = logSource;
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, sLog);

            log.WriteEntry(message, EventLogEntryType.Warning);
        }

        public static void LogInfoWrite(string message)
        {
            log = new EventLog();
            string sLog = "Application";
            string logSource = "BackupManager";
            log.Source = logSource;
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, sLog);

            log.WriteEntry(message, EventLogEntryType.Information);
        }
    }
}
