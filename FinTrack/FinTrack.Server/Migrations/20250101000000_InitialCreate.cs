using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id    = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name  = table.Column<string>(nullable: false),
                    Icon  = table.Column<string>(nullable: false, defaultValue: "💰"),
                    Color = table.Column<string>(nullable: false, defaultValue: "#1D9E75"),
                    Type  = table.Column<int>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id          = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId      = table.Column<string>(nullable: false),
                    CategoryId  = table.Column<int>(nullable: false),
                    Amount      = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Date        = table.Column<DateTime>(nullable: false),
                    Type        = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey("FK_Transactions_Categories_CategoryId", x => x.CategoryId, "Categories", "Id", onDelete: ReferentialAction.Restrict);
                    table.ForeignKey("FK_Transactions_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavingsGoals",
                columns: table => new
                {
                    Id            = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId        = table.Column<string>(nullable: false),
                    Title         = table.Column<string>(nullable: false),
                    Description   = table.Column<string>(nullable: false),
                    TargetAmount  = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TargetDate    = table.Column<DateTime>(nullable: false),
                    CreatedAt     = table.Column<DateTime>(nullable: false),
                    IsCompleted   = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsGoals", x => x.Id);
                    table.ForeignKey("FK_SavingsGoals_AspNetUsers_UserId", x => x.UserId, "AspNetUsers", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData("Categories",
                columns: new[] { "Id", "Name", "Icon", "Color", "Type" },
                values: new object[,]
                {
                    { 1,  "Food & Dining",  "🍔", "#EF4444", 1 },
                    { 2,  "Rent & Housing", "🏠", "#F59E0B", 1 },
                    { 3,  "Transport",      "🚗", "#3B82F6", 1 },
                    { 4,  "Entertainment",  "🎮", "#8B5CF6", 1 },
                    { 5,  "Healthcare",     "💊", "#EC4899", 1 },
                    { 6,  "Education",      "📚", "#06B6D4", 1 },
                    { 7,  "Shopping",       "🛒", "#F97316", 1 },
                    { 8,  "Salary",         "💼", "#1D9E75", 0 },
                    { 9,  "Freelance",      "💻", "#10B981", 0 },
                    { 10, "Other Income",   "💰", "#34D399", 0 },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SavingsGoals");
            migrationBuilder.DropTable("Transactions");
            migrationBuilder.DropTable("Categories");
        }
    }
}
