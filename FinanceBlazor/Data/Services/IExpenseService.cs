using FinanceBlazor.Models;

public interface IExpenseService
{
    // CRUD operations
    Task<IEnumerable<RecurringExpense>> GetAllExpensesAsync();
    Task<RecurringExpense> GetExpenseByIdAsync(int id);
    Task AddExpenseAsync(RecurringExpense expense);
    Task UpdateExpenseAsync(RecurringExpense expense);
    Task DeleteExpenseAsync(int id);

    // Robust views
    Task<IEnumerable<ExpenseViewDto>> GetExpenseSummaryByCategoryAsync();
    Task<IEnumerable<ExpenseProjectionDto>> GetProjectedExpensesAsync(DateTime from, DateTime to);
}