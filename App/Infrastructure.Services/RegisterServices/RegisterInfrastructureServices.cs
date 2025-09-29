using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Context;
using Core.Interfaces.Repositories;
using Infrastructure.Services.Repositories;
using Infrastructure.Services.Groq;
using Core.Entities.Entities;


namespace Infrastructure.Services.RegisterServices
{
    public static class RegisterInfrastructureServices
    {
        public static void RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TripWhizDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<TripWhizUser, IdentityRole>()
             .AddEntityFrameworkStores<TripWhizDbContext>()
             .AddDefaultTokenProviders();

            services.AddScoped<IAccomodationRepository, AccomodationRepository>();
            services.AddScoped<ITransportationRepository, TransportationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddSingleton<Random>();
        }

    }
}
