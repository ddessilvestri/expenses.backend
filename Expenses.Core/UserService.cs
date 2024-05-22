using Expenses.Core.CustomExceptions;
using Expenses.Core.DTO;
using Expenses.Core.Utilities;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;


namespace Expenses.Core
{
    public class UserService: IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (dbUser == null || dbUser.Password == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }
            
            return new AuthenticatedUser
            {
                UserName = dbUser.UserName,
                Token = JwtGenerator.GenerateAuthToken(dbUser.UserName),
            };
        }


        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.Equals(user.UserName));

            if (checkUser != null)
            {
                throw new UserNameAlreadyExistsException("Username already exists");

            }
            if (!string.IsNullOrEmpty(user.Password))
            { 
                user.Password = _passwordHasher.HashPassword(user.Password);
            }
            else
            {
                user.Password = _passwordHasher.HashPassword(user.UserName);
            }
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return new AuthenticatedUser { 
                UserName = user.UserName,
                Token = JwtGenerator.GenerateAuthToken(user.UserName),
            };
        }

        public async Task<AuthenticatedUser> ExternalSignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ExternalId.Equals(user.ExternalId) && u.ExternalType.Equals(user.ExternalType));
            if (dbUser == null)
            {
                user.UserName = CreateUniqueUsernameFromEmail(user.Email);
                return await SignUp(user);
            }

            return new AuthenticatedUser()
            {
                UserName = dbUser.UserName,
                Token = JwtGenerator.GenerateAuthToken(dbUser.UserName)
            };
        }
        private string CreateUniqueUsernameFromEmail(string email)
        {
            var emailSplit = email.Split('@').First();
            var random = new Random();
            var username = emailSplit;

            while(_context.Users.Any(u => u.UserName.Equals(username)))
            {
                username = emailSplit + random.Next(1000000000);
            }

            return username;
        }
    }
}
