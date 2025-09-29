
using Core.DTOs.Request;

namespace Infrastructure.Services.Groq
{
    public interface IGroqService
    {
        Task GetTrendingOffers();
        Task GetOfferAsync();
        Task GetAccomodationAsync(string place);
        Task GetRecomendedOffers(ICollection<CustomeActivityDto> userActivities);
    }
}





