namespace JobSpace.Controllers
{
    public interface ISignaController
    {
        ISignaController SetCustomer(string customer);
        ISignaController SetJobNumber(string number);
        ISignaController SetJobDescription(string description);
        ISignaController SetPageCount(int cntPages);
        ISignaController SetPageWidth(decimal width);
        ISignaController SetPageHeight(decimal width);
        void Save();

    }
}