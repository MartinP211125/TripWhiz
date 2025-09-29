
using Microsoft.AspNetCore.Identity;
using UserDtoRegisterRequest = Core.DTOs.Request.User.UserRegisterDto;
using UserDtoResponse = Core.DTOs.Response.UserDto;
using UserActivityDtoResponse = Core.DTOs.Response.UserActivitiesDto;
using UserActivityDtoRequest = Core.DTOs.Request.UserActivitiesDto;
using BookingDtoResponse = Core.DTOs.Response.BookingDto;
using BookingDtoRequest = Core.DTOs.Request.BookingDto;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(UserDtoRegisterRequest user);
        Task<string> LoginAsync(string email, string password);
        Task<bool> UserAlreadyExistsAsync(string email);
        Task<UserDtoResponse> GetUserByEmailAsync(string email);
        Task<bool> LogoutAsync();
        Task<ICollection<UserActivityDtoResponse>> GetUserActivitiesByUserEmailAsync(string email);
        Task<UserActivityDtoResponse> CreateUserActivityForUserAsync(UserActivityDtoRequest userActivity);
    }
}