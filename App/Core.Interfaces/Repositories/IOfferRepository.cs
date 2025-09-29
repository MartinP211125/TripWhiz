
using Core.Entities.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IOfferRepository
    {
        Task<Offer> CreateOfferAsync(Offer offer);
        Task<Offer> UpdateOfferAsync(Guid offerId, Offer offer);
        Task<Offer> DeleteOfferAsync(Guid offerId);
        Task<Offer> GetOfferByIdAsync(Guid offerId);
        Task<ICollection<Offer>> GetAllAsync();
        Task<ICollection<Offer>> GetAllPopularAsync();
        Task<ICollection<Offer>> GetAllRecomendationsAsync();
    }
}
