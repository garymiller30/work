using HardcoverOrderCalculator.Models;
using HardcoverOrderCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HardcoverOrderCalculator.Pages;

public class IndexModel : PageModel
{
    private readonly CalculatorDataProvider _dataProvider;
    private readonly HardcoverCalculator _calculator;

    public IndexModel(CalculatorDataProvider dataProvider, HardcoverCalculator calculator)
    {
        _dataProvider = dataProvider;
        _calculator = calculator;
    }

    [BindProperty]
    public OrderInput Order { get; set; } = new();

    public CalculationResult? Result { get; private set; }

    public CalculatorData Data { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Data = await _dataProvider.GetAsync();
        Order = Data.DefaultOrder;
        Result = _calculator.Calculate(Order, Data);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Data = await _dataProvider.GetAsync();
        Result = _calculator.Calculate(Order, Data);
        return Page();
    }
}
