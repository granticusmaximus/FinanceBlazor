namespace FinanceBlazor.Models
{
   public class RecurringExpense
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string PaymentFrequency { get; set; }
        public DateTime PaymentDate { get; set; }
        
        // The Category reference
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}