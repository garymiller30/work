namespace Web_ActiveWorks.Models;

public sealed class JobListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int StatusCode { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsPayed { get; set; }
}
