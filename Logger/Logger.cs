using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Logger
{
    public class Logger
    {

        private readonly FormLogger _formLogger;

        public Logger()
        {
            _formLogger = new FormLogger();
            _formLogger.CreateControl();
            _formLogger.Closing += _formLogger_Closing;

            Log.OnAdd += Add;
            _formLogger.Show(); // без цього вивалюється з помилкою про thread access
            _formLogger.Hide();
        }

        public void Add(object sender, LogRow e)
        {
            _formLogger.WriteLine(e);
        }

        public void Add(IEnumerable<LogRow> list)
        {
            foreach (var logRow in list)
            {
                _formLogger.WriteLine(logRow);
            }


        }

        private void _formLogger_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _formLogger.Hide();
        }

        public void Write(string header, string status, string message, bool showWindow = false)
        {

            _formLogger.WriteLine(new LogRow(header, status, message));
            if (showWindow)
                ShowLoggerWindow();
        }

        public void ShowLoggerWindow()
        {
            try
            {
                if (_formLogger.Created)
                {
                    _formLogger.Show();
                    _formLogger.Activate();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //_formLogger.Invoke(new Action(() => _formLogger.Show()));
        }
    }
}
