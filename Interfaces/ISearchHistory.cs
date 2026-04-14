namespace Interfaces
{
    public interface ISearchHistory
    {
        string[] GetHistory();
        void Add(string str);
    }
}
