namespace FinanceBlazor.Models
{
    public class Income
    {
        public int Id { get; set; }
        public decimal TotalAmount {  get; set; }
        public DateTime PayDate { get; set; }
        public decimal Spending {  get; set; }
        public decimal Savings { get; set; }
    }
}