using Interfaces;
using System;

namespace MailNotifier.Shablons
{
    [Serializable]
    public class MailTemplate
    {
        public string ShablonName { get; set; }
        public string SendTo { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }

        public string GetHeader(IUserProfile profile, IJob job)
        {
            if (job == null)
                return Header;

            //УСК, $plugin:"Форми":[TotalForms] шт. ($plugin:"Форми":[FormFormat]) -$OrderNumber-

            var str = Header.Replace("$OrderNumber", job.Number);
            str = profile.Plugins.ReplaceStr(job, str);


            return str;
                   
        }
    }
}
