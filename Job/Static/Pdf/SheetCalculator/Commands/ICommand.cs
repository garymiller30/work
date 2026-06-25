namespace JobSpace.Static.Pdf.SheetCalculator.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute();
        void Undo();
    }
}
