
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using UserDtoRegisterRequest = Core.DTOs.Request.User.UserRegisterDto;
using UserDtoResponse = Core.DTOs.Response.UserDto;
using UserActivityDtoResponse = Core.DTOs.Response.UserActivitiesDto;
using UserActivityDtoRequest = Core.DTOs.Request.UserActivitiesDto;
using BookingDtoResponse = Core.DTOs.Response.BookingDto;
using BookingDtoRequest = Core.DTOs.Request.BookingDto;
using Core.Interfaces.Repositories;
using Core.Entities.Entities;
using Core.Enums;
using Core.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Text.Json;

namespace Service.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly IScheduler scheduler;
        public UserService(IUserRepository userRepository, IConfiguration configuration, IScheduler scheduler)
        {
            this.userRepository = userRepository;
            this.configuration = configuration; 
            this.scheduler = scheduler;
        }

        public async Task<UserActivityDtoResponse> CreateUserActivityForUserAsync(UserActivityDtoRequest userActivityDto)
        {
            UserActivity userActivity = new UserActivity
            {
                UserId = userActivityDto.UserId,
                TimeStamp = DateTime.UtcNow,
                TargetId = userActivityDto.TargetId,
                TargetType = userActivityDto.TargetType,
                Query = userActivityDto.Query,
                ActivityType = userActivityDto.ActivityType,
            };

            UserActivity createdUserActivity = await userRepository.CreateUserActivityForUserAsync(userActivity);
            var user = await userRepository.GetUserByIdAsync(createdUserActivity.UserId);
            var activities = await userRepository.GetUserActivitiesByUserEmailAsync(user.Email);
            if (activities != null && activities.Count >= 3)
            {
                await TriggerJobAsync("UpdateRecomendationsForUserJob", activities);
            }

            return new UserActivityDtoResponse
            {
                Id = createdUserActivity.Id,
                UserId = createdUserActivity.UserId,
                TimeStamp = createdUserActivity.TimeStamp,
                TargetId = createdUserActivity.TargetId,
                TargetType = createdUserActivity.TargetType,
                Query = createdUserActivity.Query,
                ActivityType = createdUserActivity.ActivityType
            };
        }

    
        public async Task<ICollection<UserActivityDtoResponse>> GetUserActivitiesByUserEmailAsync(string email)
        {
            ICollection<UserActivity> userActivities = await userRepository.GetUserActivitiesByUserEmailAsync(email);

            return userActivities.Select(x => new UserActivityDtoResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                TimeStamp = x.TimeStamp,
                TargetId = x.TargetId,
                TargetType = x.TargetType,
                Query = x.Query,
                ActivityType = x.ActivityType
            }).ToList();
        }

        public Task<UserDtoResponse> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            SignInResult result = await userRepository.LoginAsync(email, password);

            if (!result.Succeeded)
            {
                throw new EmailOrPasswordIncorrectException("Email or password incorrect.");
            }

            var user = await userRepository.GetUserByEmailAsync(email);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> LogoutAsync()
        {
            await userRepository.LogOutAsync();
            return true;
        }

        public async Task<bool> RegisterAsync(UserDtoRegisterRequest registerUser)
        {
            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                throw new PasswordDoNotMatchException("The passwords do not match.");
            }

            if (await userRepository.UserAlreadyExistsAsync(registerUser.Email))
            {
                throw new UserAlreadyExistsException("User with the email:" + registerUser.Email + " already exists.");
            }

            TripWhizUser user = new TripWhizUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
            };

            IdentityResult result = await userRepository.RegisterAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                throw new RegistrationFaildException("registration faild try registering again.");
            }
            return result.Succeeded;
        }


        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            return await userRepository.UserAlreadyExistsAsync(email);
        }

        private async Task TriggerJobAsync(string jobId,  ICollection<UserActivity> activities)
        {
            var jobKey = new JobKey(nameof(jobId));

            var jobData = new JobDataMap
        {
            { "activities", JsonSerializer.Serialize(activities) }
        };

            await scheduler.TriggerJob(jobKey, jobData);
        }
    }
}
