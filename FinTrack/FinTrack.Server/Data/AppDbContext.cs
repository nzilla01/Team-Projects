using FinTrack.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Server.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SavingsGoal> SavingsGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SavingsGoal>()
                .HasOne(g => g.User)
                .WithMany(u => u.SavingsGoals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Transaction>()
                .Property(t => t.Amount).HasColumnType("decimal(18,2)");
            builder.Entity<SavingsGoal>()
                .Property(g => g.TargetAmount).HasColumnType("decimal(18,2)");
            builder.Entity<SavingsGoal>()
                .Property(g => g.CurrentAmount).HasColumnType("decimal(18,2)");

            builder.Entity<Category>().HasData(
                new Category { Id = 1,  Name = "Food & Dining",  Icon = "🍔", Color = "#EF4444", Type = CategoryType.Expense },
                new Category { Id = 2,  Name = "Rent & Housing", Icon = "🏠", Color = "#F59E0B", Type = CategoryType.Expense },
                new Category { Id = 3,  Name = "Transport",      Icon = "🚗", Color = "#3B82F6", Type = CategoryType.Expense },
                new Category { Id = 4,  Name = "Entertainment",  Icon = "🎮", Color = "#8B5CF6", Type = CategoryType.Expense },
                new Category { Id = 5,  Name = "Healthcare",     Icon = "💊", Color = "#EC4899", Type = CategoryType.Expense },
                new Category { Id = 6,  Name = "Education",      Icon = "📚", Color = "#06B6D4", Type = CategoryType.Expense },
                new Category { Id = 7,  Name = "Shopping",       Icon = "🛒", Color = "#F97316", Type = CategoryType.Expense },
                new Category { Id = 8,  Name = "Salary",         Icon = "💼", Color = "#1D9E75", Type = CategoryType.Income  },
                new Category { Id = 9,  Name = "Freelance",      Icon = "💻", Color = "#10B981", Type = CategoryType.Income  },
                new Category { Id = 10, Name = "Other Income",   Icon = "💰", Color = "#34D399", Type = CategoryType.Income  }
            );
        }
    }
}
