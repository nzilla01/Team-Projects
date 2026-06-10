using FinTrack.Server.Data;
using FinTrack.Shared.DTOs;
using FinTrack.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinTrack.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TransactionsController(AppDbContext db) => _db = db;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub")!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] string? type,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var q = _db.Transactions.Include(t => t.Category)
                        .Where(t => t.UserId == UserId).AsQueryable();

            if (categoryId.HasValue) q = q.Where(t => t.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(type) && Enum.TryParse<TransactionType>(type, out var tt))
                q = q.Where(t => t.Type == tt);
            if (from.HasValue) q = q.Where(t => t.Date >= from);
            if (to.HasValue)   q = q.Where(t => t.Date <= to);

            var list = await q.OrderByDescending(t => t.Date)
                .Select(t => ToDto(t)).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetById(int id)
        {
            var t = await _db.Transactions.Include(t => t.Category)
                        .FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
            return t == null ? NotFound() : Ok(ToDto(t));
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionDto dto)
        {
            var category = await _db.Categories.FindAsync(dto.CategoryId);
            if (category == null) return BadRequest(new { message = "Invalid category." });

            var t = new Transaction
            {
                UserId      = UserId,
                CategoryId  = dto.CategoryId,
                Amount      = dto.Amount,
                Description = dto.Description,
                Date        = dto.Date,
                Type        = Enum.Parse<TransactionType>(dto.Type)
            };
            _db.Transactions.Add(t);
            await _db.SaveChangesAsync();
            t.Category = category;
            return CreatedAtAction(nameof(GetById), new { id = t.Id }, ToDto(t));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTransactionDto dto)
        {
            var t = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
            if (t == null) return NotFound();

            t.CategoryId  = dto.CategoryId;
            t.Amount      = dto.Amount;
            t.Description = dto.Description;
            t.Date        = dto.Date;
            t.Type        = Enum.Parse<TransactionType>(dto.Type);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
            if (t == null) return NotFound();
            _db.Transactions.Remove(t);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private static TransactionDto ToDto(Transaction t) => new()
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
        };
    }
}
