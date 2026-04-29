namespace HardcoverOrderCalculator.Models;

public sealed class CalculationResult
{
    public SummaryResult Summary { get; set; } = new();

    public List<CalculationSection> Sections { get; set; } = new();

    public List<WorkLine> WorkLines { get; set; } = new();

    public List<MaterialLine> MaterialLines { get; set; } = new();
}

public sealed class SummaryResult
{
    public decimal TotalHours { get; set; }

    public decimal WorkCost { get; set; }

    public decimal MaterialCost { get; set; }

    public decimal TotalCost { get; set; }

    public decimal CostPerBook { get; set; }

    public decimal PriceWithMarkup { get; set; }

    public decimal PricePerBookWithMarkup { get; set; }

    public decimal FinishedWidthMm { get; set; }

    public decimal FinishedHeightMm { get; set; }

    public decimal FinishedThicknessMm { get; set; }

    public decimal BookWeightGram { get; set; }

    public int Packs { get; set; }

    public int BooksPerPack { get; set; }
}

public sealed class CalculationSection
{
    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public List<CalculationStep> Steps { get; set; } = new();
}

public sealed class CalculationStep
{
    public string Label { get; set; } = "";

    public string Formula { get; set; } = "";

    public string Value { get; set; } = "";
}

public sealed class WorkLine
{
    public string Group { get; set; } = "";

    public string Operation { get; set; } = "";

    public string Format { get; set; } = "";

    public decimal Quantity { get; set; }

    public decimal Hours { get; set; }

    public decimal MaterialCost { get; set; }

    public decimal WorkCost { get; set; }

    public bool Enabled { get; set; }
}

public sealed class MaterialLine
{
    public string Name { get; set; } = "";

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = "";

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
}
