
using OfferDtoResponse = Core.DTOs.Response.OfferDto;
using Core.DTOs.Request;

namespace Service.Interfaces
{
    public interface IOfferService
    {
        Task<OfferDtoResponse> CreateOfferAsync(CreateOfferRequest offer);
        Task<OfferDtoResponse> DeleteOfferAsync(Guid offerId);
        Task<OfferDtoResponse> GetOfferByIdAsync(Guid offerId);
        Task<ICollection<OfferDtoResponse>> GetAllAsync();
        Task<ICollection<OfferDtoResponse>> GetPopular();
        Task<ICollection<OfferDtoResponse>> GetRecomendations();
    }
}
