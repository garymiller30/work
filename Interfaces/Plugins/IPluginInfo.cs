using System.Windows.Forms;

namespace Interfaces
{
    public interface IPluginInfo : IPluginBase
    {

        IUserProfile UserProfile { get; set; }

        /// <summary>
        /// викликається першим
        /// </summary>
        /// <param name="profile"></param>
        //void SetUserProfile(object profile);

        UserControl GetUserControl();
        /// <summary>
        /// викликається другим
        /// </summary>
        void Start();

        string GetPluginName();

        //void SetCurJobCallBack(object curJob);
        //void SetCurJobPathCallBack(object curJobPath);
        /// <summary>
        /// коди змінюється поточна робота
        /// </summary>
        /// <param name="curJob"></param>
        void SetCurJob(IJob curJob);

        void BeforeJobChange(IJob job);
        void AfterJobChange(IJob job);

    }
}
