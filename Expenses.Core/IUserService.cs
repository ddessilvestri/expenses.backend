using Expenses.Core.DTO;
using Expenses.DB;

namespace Expenses.Core
{
    public interface IUserService
    {
        Task<AuthenticatedUser> SignUp(User user);
        Task<AuthenticatedUser> SignIn(User user);

        Task<AuthenticatedUser> ExternalSignIn(User user);
    }
}
