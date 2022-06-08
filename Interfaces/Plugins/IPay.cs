using System;

namespace Interfaces
{
    public interface IPay
    {
        DateTime Date { get; set; }
        decimal Sum { get; set; }
    }
}
