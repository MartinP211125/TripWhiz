
using Core.Entities.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly TripWhizDbContext _context;
        public OfferRepository(TripWhizDbContext context)
        {
            _context = context;
        }

        public async Task<Offer> CreateOfferAsync(Offer offer)
        {
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<Offer> DeleteOfferAsync(Guid offerId)
        {
            Offer offer = await GetOfferByIdAsync(offerId);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<ICollection<Offer>> GetAllAsync()
        {
            return await _context.Offers.ToListAsync();
        }

        public async Task<ICollection<Offer>> GetAllPopularAsync()
        {
            return await _context.Offers.Where(x => x.OfferType == OfferType.Trending).ToListAsync();
        }

        public async Task<ICollection<Offer>> GetAllRecomendationsAsync()
        {
            return await _context.Offers.Where(x => x.OfferType == OfferType.Recomended).ToListAsync();
        }

        public async Task<Offer> GetOfferByIdAsync(Guid offerId)
        {
            return await _context.Offers.Include(x => x.Transportation).FirstOrDefaultAsync(x => x.Id == offerId);
        }

        public async Task<Offer> UpdateOfferAsync(Guid offerId, Offer offer)
        {
            Offer existingOffer = await GetOfferByIdAsync(offerId);
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
            return existingOffer;
        }
    }
}
