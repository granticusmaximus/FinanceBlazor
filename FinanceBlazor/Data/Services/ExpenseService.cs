using Microsoft.EntityFrameworkCore;
using FinanceBlazor.Data;
using FinanceBlazor.Models; // Add the correct namespace for ApplicationDbContext

public class ExpenseService : IExpenseService
{
    private readonly ApplicationDbContext _context;

    public ExpenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecurringExpense>> GetAllExpensesAsync()
    {
        return await _context.RecurringExpenses.Include(e => e.Category).ToListAsync();
    }

    public async Task<RecurringExpense> GetExpenseByIdAsync(int id)
    {
        return await _context.RecurringExpenses.Include(e => e.Category)
                                               .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddExpenseAsync(RecurringExpense expense)
    {
        _context.RecurringExpenses.Add(expense);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExpenseAsync(RecurringExpense expense)
    {
        _context.RecurringExpenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpenseAsync(int id)
    {
        var expense = await _context.RecurringExpenses.FindAsync(id);
        if (expense != null)
        {
            _context.RecurringExpenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ExpenseViewDto>> GetExpenseSummaryByCategoryAsync()
    {
        return await _context.RecurringExpenses
            .GroupBy(e => e.Category)
            .Select(g => new ExpenseViewDto
            {
                CategoryName = g.Key.Name,
                TotalAmount = g.Sum(e => e.Amount),
                ExpenseCount = g.Count()
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<ExpenseProjectionDto>> GetProjectedExpensesAsync(DateTime from, DateTime to)
    {
        return await _context.RecurringExpenses
            .Where(e => e.PaymentDate >= from && e.PaymentDate <= to)
            .Select(e => new ExpenseProjectionDto
            {
                ExpenseName = e.Name,
                NextPaymentDate = e.PaymentDate,
                Amount = e.Amount,
                PaymentFrequency = e.PaymentFrequency
            })
            .ToListAsync();
    }
}