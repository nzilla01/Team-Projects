using FinTrack.Server.Data;
using FinTrack.Shared.DTOs;
using FinTrack.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinTrack.Server.Controllers
{
    // ── CATEGORIES ────────────────────────────────────────────────
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var cats = await _db.Categories
                .Select(c => new CategoryDto
                {
                    Id    = c.Id,
                    Name  = c.Name,
                    Icon  = c.Icon,
                    Color = c.Color,
                    Type  = c.Type.ToString()
                }).ToListAsync();
            return Ok(cats);
        }
    }

    // ── SAVINGS GOALS ─────────────────────────────────────────────
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SavingsGoalsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public SavingsGoalsController(AppDbContext db) => _db = db;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub")!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SavingsGoalDto>>> GetAll()
        {
            var goals = await _db.SavingsGoals
                .Where(g => g.UserId == UserId)
                .Select(g => new SavingsGoalDto
                {
                    Id                 = g.Id,
                    Title              = g.Title,
                    Description        = g.Description,
                    TargetAmount       = g.TargetAmount,
                    CurrentAmount      = g.CurrentAmount,
                    ProgressPercentage = g.TargetAmount > 0
                        ? Math.Min(100, (g.CurrentAmount / g.TargetAmount) * 100) : 0,
                    TargetDate         = g.TargetDate,
                    IsCompleted        = g.IsCompleted
                }).ToListAsync();
            return Ok(goals);
        }

        [HttpPost]
        public async Task<ActionResult<SavingsGoalDto>> Create([FromBody] CreateSavingsGoalDto dto)
        {
            var goal = new SavingsGoal
            {
                UserId       = UserId,
                Title        = dto.Title,
                Description  = dto.Description,
                TargetAmount = dto.TargetAmount,
                TargetDate   = dto.TargetDate
            };
            _db.SavingsGoals.Add(goal);
            await _db.SaveChangesAsync();
            return Ok(new SavingsGoalDto
            {
                Id           = goal.Id,
                Title        = goal.Title,
                Description  = goal.Description,
                TargetAmount = goal.TargetAmount,
                TargetDate   = goal.TargetDate
            });
        }

        [HttpPut("{id}/deposit")]
        public async Task<IActionResult> Deposit(int id, [FromBody] decimal amount)
        {
            var goal = await _db.SavingsGoals
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == UserId);
            if (goal == null) return NotFound();

            goal.CurrentAmount += amount;
            if (goal.CurrentAmount >= goal.TargetAmount)
                goal.IsCompleted = true;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var goal = await _db.SavingsGoals
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == UserId);
            if (goal == null) return NotFound();
            _db.SavingsGoals.Remove(goal);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

    // ── DASHBOARD ─────────────────────────────────────────────────
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) => _db = db;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub")!;

        [HttpGet]
        public async Task<ActionResult<DashboardDto>> Get()
        {
            var transactions = await _db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == UserId)
                .ToListAsync();

            var totalIncome   = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            var totalExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);

            var recent = transactions
                .OrderByDescending(t => t.Date)
                .Take(5)
                .Select(t => new TransactionDto
                {
                    Id            = t.Id,
                    CategoryId    = t.CategoryId,
                    CategoryName  = t.Category?.Name  ?? "",
                    CategoryIcon  = t.Category?.Icon  ?? "",
                    CategoryColor = t.Category?.Color ?? "",
                    Amount        = t.Amount,
                    Description   = t.Description,
                    Date          = t.Date,
                    Type          = t.Type.ToString()
                }).ToList();

            var expenseTransactions = transactions.Where(t => t.Type == TransactionType.Expense).ToList();
            var spendingByCategory = expenseTransactions
                .GroupBy(t => t.Category)
                .Select(g => new CategorySpendDto
                {
                    CategoryName  = g.Key?.Name  ?? "Other",
                    CategoryColor = g.Key?.Color ?? "#6B7280",
                    Amount        = g.Sum(t => t.Amount),
                    Percentage    = totalExpenses > 0
                        ? Math.Round((g.Sum(t => t.Amount) / totalExpenses) * 100, 1) : 0
                })
                .OrderByDescending(c => c.Amount)
                .ToList();

            return Ok(new DashboardDto
            {
                TotalIncome        = totalIncome,
                TotalExpenses      = totalExpenses,
                TotalBalance       = totalIncome - totalExpenses,
                RecentTransactions = recent,
                SpendingByCategory = spendingByCategory
            });
        }
    }
}
