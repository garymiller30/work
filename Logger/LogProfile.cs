using System.Collections.Generic;

namespace Logger
{
    public class LogProfile
    {
        public object Profile;
        public readonly List<LogRow> Rows = new List<LogRow>(48);
    }


}
