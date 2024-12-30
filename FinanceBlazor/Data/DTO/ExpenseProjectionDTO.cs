public class ExpenseProjectionDto
{
    public string ExpenseName { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentFrequency { get; set; }
}