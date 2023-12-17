using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Controllers
{
    public sealed class SignaChangeOrderNumberParams
    {
        public string OldNumber { get;set; }
        public string NewNumber { get;set; }
        public IJob Job { get;set; }
        public IUserProfile Profile { get;set; }
    }
}
