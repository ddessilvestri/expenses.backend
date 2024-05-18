using Expenses.Core.DTO;
using Expenses.DB;

namespace Expenses.Core
{
    public interface IExpensesServices
    {
        List<ExpenseCore> GetExpenses();
        ExpenseCore GetExpense(int id);
        ExpenseCore CreateExpense(Expense expense);
        void DeleteExpense(ExpenseCore expense);

        ExpenseCore EditExpense(ExpenseCore expense);

        
    }
}
