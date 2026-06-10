namespace FinTrack.Shared.Models
{
    public class AppUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "💰";
        public string Color { get; set; } = "#1D9E75";
        public CategoryType Type { get; set; } = CategoryType.Expense;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public enum CategoryType { Income, Expense }

    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public TransactionType Type { get; set; }
        public AppUser? User { get; set; }
        public Category? Category { get; set; }
    }

    public enum TransactionType { Income, Expense }

    public class SavingsGoal
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime TargetDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;
        public AppUser? User { get; set; }
        public decimal ProgressPercentage =>
            TargetAmount > 0 ? Math.Min(100, (CurrentAmount / TargetAmount) * 100) : 0;
    }
}
