var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddSingleton<HardcoverOrderCalculator.Services.CalculatorDataProvider>();
builder.Services.AddSingleton<HardcoverOrderCalculator.Services.HardcoverCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
