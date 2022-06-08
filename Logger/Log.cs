using System;
using System.Collections.Generic;
using System.Linq;

namespace Logger
{
    public static class Log
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static readonly List<LogProfile> Profiles = new List<LogProfile>(10);

        internal static EventHandler<LogRow> OnAdd = delegate { };

        private static Logger _writer;

        public static void Info(object profile, string header, object msg)
        {

            var logProfile = GetLogProfile(profile);
            Logger.Info($"{logProfile.Profile} - {header} : {msg}");

            var row = new LogRow(header, "INFO", msg);

            logProfile.Rows.Add(row);

            OnAdd(null, row);
        }

        private static LogProfile GetLogProfile(object profile)
        {
            var p = Profiles.FirstOrDefault(x => x.Profile.Equals(profile));
            if (p == null)
            {
                p = new LogProfile() { Profile = profile };
                Profiles.Add(p);
            }

            return p;
        }

        public static void Error(object profile, string header, object msg)
        {

            var logProfile = GetLogProfile(profile);
            var str = $"{logProfile.Profile} - {header} : {msg}";
            Logger.Error(str);
            var row = new LogRow(header, "ERROR", msg);

            logProfile.Rows.Add(row);
            OnAdd(null, row);
        }

        public static void Warning(object profile, string header, object msg)
        {

            var logProfile = GetLogProfile(profile);
            Logger.Warn($"{logProfile.Profile} - {header} : {msg}");
            var row = new LogRow(header, "WARNING", msg);
            logProfile.Rows.Add(row);
            OnAdd(null, row);

        }

        public static void ShowWindow()
        {

            if (_writer == null)
            {
                _writer = new Logger();
                foreach (var logProfile in Profiles)
                {
                    _writer.Add(logProfile.Rows);
                }
            }

            _writer.ShowLoggerWindow();
        }
    }
}
