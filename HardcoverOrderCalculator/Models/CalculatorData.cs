namespace HardcoverOrderCalculator.Models;

public sealed class CalculatorData
{
    public Dictionary<string, SheetFormat> Formats { get; set; } = new();

    public Dictionary<string, decimal> MaterialPrices { get; set; } = new();

    public List<OperationNorm> OperationNorms { get; set; } = new();

    public OrderInput DefaultOrder { get; set; } = new();
}

public sealed class SheetFormat
{
    public decimal WidthMm { get; set; }

    public decimal HeightMm { get; set; }
}

public sealed class OperationNorm
{
    public string Name { get; set; } = "";

    public decimal SetupHours { get; set; }

    public decimal HoursPerUnit { get; set; }

    public decimal UnitsPerShift { get; set; }

    public decimal HourRate { get; set; }

    public decimal EnergyPerHour { get; set; }
}
