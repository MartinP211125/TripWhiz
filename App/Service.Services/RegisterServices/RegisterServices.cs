using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service.Services.Services;


namespace Service.Services.RegisterServices
{
    public static class RegisterServices
    {
        public static void RegisterServiceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccomodationService, AccomodationService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<ITransportationService, TransportationService>();
            services.AddScoped<IUserService, UserService>();
        }

    }
}
