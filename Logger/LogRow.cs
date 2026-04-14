using System;

namespace Logger
{
    public class LogRow
    {
        public DateTime Date { get; } = DateTime.Now;
        public string Header { get; }
        public string Status { get; }
        public object Message { get; }


        public LogRow(string header,string status,object message)
        {
            Header = header;
            Status = status;
            Message = message;
        }
    }
}
