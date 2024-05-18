using Expenses.Core.DTO;
using Expenses.DB;
using Microsoft.AspNetCore.Http;

namespace Expenses.Core
{
    public class ExpensesServices : IExpensesServices
    {
        private AppDbContext _context;
        private readonly User _user;
        public ExpensesServices(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                .First(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }
        public ExpenseCore GetExpense(int id) => _context.Expenses
             .Where(e => e.User.Id == _user.Id && e.Id == id)
             .Select(e => (ExpenseCore)e)
             .First();
        public List<ExpenseCore> GetExpenses() => _context.Expenses
             .Where(e => e.User.Id == _user.Id)
             .Select(e => (ExpenseCore)e)
             .ToList();

        public ExpenseCore CreateExpense(Expense expense) 
        {
            expense.User = _user;
            _context.Add(expense);
            _context.SaveChanges();
            return (ExpenseCore)expense;
        }

        public void DeleteExpense(ExpenseCore expense)
        {
            var dbExpense = _context.Expenses.First(e => e.User.Id == _user.Id && e.Id == expense.Id);
            _context.Expenses.Remove(dbExpense);
            _context.SaveChanges();
        }

        public ExpenseCore EditExpense(ExpenseCore expense)
        {

            var dbExpense = _context.Expenses.First(e => e.User.Id == _user.Id && e.Id == expense.Id);
            dbExpense.Description = expense.Description;
            dbExpense.Amount  = expense.Amount;
            _context.SaveChanges();

            return expense;
        }
    }
}