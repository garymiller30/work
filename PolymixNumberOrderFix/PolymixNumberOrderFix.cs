using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;

namespace PolymixNumberOrderFix
{
    public class PolymixNumberOrderFix : IPluginInfo
    {
        public string PluginName { get; } = "Polymix number order fix";
        public string PluginDescription { get; } = "Вирівнює номер замовнення нулями: 00005, 01234 і т.п.";
        public void ShowSettingsDlg()
        {
//            throw new NotImplementedException();
        }

        public void SetUserProfile(object profile)
        {
            //throw new NotImplementedException();
        }

        public IUserProfile UserProfile { get; set; }

        public UserControl GetUserControl()
        {
            return null;
            //throw new NotImplementedException();
        }

        public void Start()
        {
            //throw new NotImplementedException();
        }

        public string GetPluginName()
        {
            return PluginName;
            //throw new NotImplementedException();
        }

        public void SetCurJobCallBack(object curJob)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJobPathCallBack(object curJobPath)
        {
            //throw new NotImplementedException();
        }

        public void SetCurJob(IJob curJob)
        {
            //throw new NotImplementedException();
        }

        public void BeforeJobChange(IJob job)
        {
            //throw new NotImplementedException();
        }

        public void AfterJobChange(IJob job)
        {
            
            if (job is IJob curJob)
            {
                if (int.TryParse(curJob.Number, out int number))
                {
                    curJob.Number = number.ToString("D5");
                }

                
            }
        }
    }
}
