
using Core.Entities.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories
{
    public class TransportationRepository : ITransportationRepository
    {
        private readonly TripWhizDbContext _context;
        public TransportationRepository(TripWhizDbContext context)
        {
            _context = context;
        }

        public async Task<Transportation> CreateTransportationAsync(Transportation transportation)
        {
            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();

            for (int i = 0; i < transportation.NumberOfSeats; i++)
            {
                _context.Seats.Add(new Seat
                {
                    TransportationId = transportation.Id
                });
            }

            await _context.SaveChangesAsync();
            return transportation;
        }

        public async Task<TransportationAvailability> CreateTransportationAvailabilityAsync(TransportationAvailability transportationAvailability)
        {
            _context.TransportationsAvailabilities.Add(transportationAvailability);
            await _context.SaveChangesAsync();
            return transportationAvailability;
        }

        public async Task<ICollection<Transportation>> GetAllTransportationsAsync()
        {
            return await _context.Transportations.ToListAsync();
        }

        public async Task<Transportation> GetTransportationAsync(Guid id)
        {
            return await _context.Transportations.Include(x => x.Seats).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<TransportationAvailability>> GetTransportationAvailabilityAsync(Guid transportationId)
        {
            return await _context.TransportationsAvailabilities.Where(x => x.TransportationId == transportationId).ToListAsync();
        }

        public async Task<Transportation> UpdateTransportationAsync(Guid transactionId, Transportation transportation)
        {
            Transportation existingTransportation = await GetTransportationAsync(transactionId);

            _context.Transportations.Update(transportation);
            await _context.SaveChangesAsync();
            return existingTransportation;
        }

        public async Task DeleteTransportationAcailabilitiesWithPassedDates()
        {
            var transportationAvailabilities = _context.TransportationsAvailabilities.ToList();
            foreach (var transportation in transportationAvailabilities)
            {
                if (transportation.ToDate < DateTime.UtcNow)
                {
                    _context.TransportationsAvailabilities.Remove(transportation);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransportationAvailabilityAsync(Guid transporationAvailabilityId)
        {
            var transportationAvailability = await _context.TransportationsAvailabilities.FirstOrDefaultAsync(x => x.Id == transporationAvailabilityId) ?? throw new TransportAvailabilityNotFoundException("The transportation availability with the id: " + transporationAvailabilityId + " does not exist.");
            _context.TransportationsAvailabilities.Remove(transportationAvailability);
            await _context.SaveChangesAsync();
        }
    }
}
