namespace Interfaces
{
    public interface IPluginBase
    {
        string PluginName { get; }
        string PluginDescription { get; }
        void ShowSettingsDlg();
    }
}
