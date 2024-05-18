using Expenses.DB;

namespace Expenses.Core.DTO
{
    public class ExpenseCore
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public static explicit operator ExpenseCore(Expense e) => new ExpenseCore
        {
            Id = e.Id,
            Amount = e.Amount,
            Description = e.Description
        };
    }
}
