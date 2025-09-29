
using Core.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterAsync(TripWhizUser user, string password);
        Task<SignInResult> LoginAsync(string email, string password);
        Task LogOutAsync();
        Task<bool> UserAlreadyExistsAsync(string email);
        Task<TripWhizUser> GetUserByIdAsync(string id);
        Task<TripWhizUser> GetUserByEmailAsync(string email);
        Task<ICollection<UserActivity>> GetUserActivitiesByUserEmailAsync(string email);
        Task<UserActivity> CreateUserActivityForUserAsync(UserActivity userActivity);
    }
}
