
using Core.Entities.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly UserManager<TripWhizUser> _userManager;
        private readonly SignInManager<TripWhizUser> _signInManager;
        private readonly TripWhizDbContext _context;

        public UserRepository(UserManager<TripWhizUser> userManager, SignInManager<TripWhizUser> signInManager, TripWhizDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<UserActivity> CreateUserActivityForUserAsync(UserActivity userActivity)
        {
            _context.UserActivities.Add(userActivity);
            await _context.SaveChangesAsync();
            return userActivity;
        }

        public async Task<ICollection<UserActivity>> GetUserActivitiesByUserEmailAsync(string email)
        {
            TripWhizUser user = await GetUserByEmailAsync(email);
            return await _context.UserActivities.Where(x => x.UserId == user.Id).ToListAsync();
        }

        public async Task<TripWhizUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<TripWhizUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(TripWhizUser user, string password)
        {
            return await _userManager.CreateAsync(user, password); 
        }

        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            return await GetUserByEmailAsync(email) != null;
        }
    }
}
