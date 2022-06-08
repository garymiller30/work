namespace Interfaces
{
    public interface IFactory
    {
        IJob CreateJob();
        ICustomer CreateCustomer();
        void CreateJobFromFile(IJob j, string filePath);
        IPart CreatePart();
        IFileBrowser CreateFileBrowser();
    }
}
