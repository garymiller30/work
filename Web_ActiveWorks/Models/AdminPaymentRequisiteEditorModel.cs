namespace Web_ActiveWorks.Models;

public sealed class AdminPaymentRequisiteEditorModel
{
    public string BankName { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Iban { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
}
