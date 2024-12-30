using FinanceBlazor.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var expenses = await _expenseService.GetAllExpensesAsync();
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RecurringExpense expense)
    {
        await _expenseService.AddExpenseAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RecurringExpense expense)
    {
        if (id != expense.Id) return BadRequest();
        await _expenseService.UpdateExpenseAsync(expense);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _expenseService.DeleteExpenseAsync(id);
        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _expenseService.GetExpenseSummaryByCategoryAsync();
        return Ok(summary);
    }

    [HttpGet("projections")]
    public async Task<IActionResult> GetProjections(DateTime from, DateTime to)
    {
        var projections = await _expenseService.GetProjectedExpensesAsync(from, to);
        return Ok(projections);
    }
}