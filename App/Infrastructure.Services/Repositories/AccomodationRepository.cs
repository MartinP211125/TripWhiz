
using Core.Entities.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services.Repositories
{
    public class AccomodationRepository : IAccomodationRepository
    {
        private readonly TripWhizDbContext _context;
        private readonly Random _random;
        public AccomodationRepository(TripWhizDbContext context, Random random)
        {
            _context = context;
            _random = random;
        }

        public async Task<Accomodation> CreateAccomodationAsync(Accomodation accomodation)
        {
            _context.Accomodations.Add(accomodation);
            await _context.SaveChangesAsync();

            for (int i = 0; i < accomodation.NumberOfRooms; i++)
            {
                _context.Rooms.Add(new Room
                {
                    Capacity = _random.Next(1, 8),
                    AccomodationId = accomodation.Id
                });
            }

            await _context.SaveChangesAsync(); 

            return accomodation;
        }


        public async Task<AccomodationAvailability> CreateAccomodationAvailabilityAsync(AccomodationAvailability accomodationAvailability)
        {
            _context.AccomodationAvailabilities.Add(accomodationAvailability);
            await _context.SaveChangesAsync();
            return accomodationAvailability;
        }

        public async Task<Accomodation> GetAccomodationAsync(Guid accomodationId)
        {
            return await _context.Accomodations.FirstOrDefaultAsync(x => x.Id == accomodationId);
        }

        public async Task<ICollection<AccomodationAvailability>> GetAccomodationAvailabilityAsync(Guid accomodationId)
        {
            return await _context.AccomodationAvailabilities.Where(x => x.AccomodationId == accomodationId).ToListAsync();
        }

        public async Task<ICollection<Accomodation>> GetAccomodationsByPlaceAsync(Guid placeId)
        {
            return await _context.Accomodations.Where(x => x.PlaceId == placeId).ToListAsync();
        }

        public async Task<ICollection<Accomodation>> GetAllAccomodationsAsync()
        {
            return await _context.Accomodations.ToListAsync();
        }

        public async Task<Place> GetPlaceAsync(Guid id)
        {
            return await _context.Places.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Place>> GetPlacesAsync()
        {
            return await _context.Places.ToListAsync();
        }

        public async Task<Place> CreatePlaceAsync(Place place)
        {
            _context.Places.Add(place);
            await _context.SaveChangesAsync();
            return place;
        }

        public async Task<Accomodation> UpdateAccomodationAsync(Guid accomodationId, Accomodation accomodation)
        {
            Accomodation existingAccomodation = await GetAccomodationAsync(accomodationId);

            _context.Accomodations.Update(accomodation);
            await _context.SaveChangesAsync();
            return accomodation;
        }

        public Task<Place> GetPlaceByCityAsync(string city)
        {
            return _context.Places.FirstOrDefaultAsync(x => x.City == city);
        }

        public async Task DeleteAccomodationAcailabilitiesWithPassedDates()
        {
            var accomodationAvailabilities = _context.AccomodationAvailabilities.ToList();
            foreach(var accomodation in accomodationAvailabilities)
            {
                if(accomodation.ToDate < DateTime.UtcNow)
                {
                    _context.AccomodationAvailabilities.Remove(accomodation);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Room>> GetRoomsByAccomodationIdAsync(Guid accomodationId)
        {
            return await _context.Rooms.Where(x => x.AccomodationId == accomodationId).ToListAsync();
        }

        public async Task DeleteAccomodationAvailabilityById(Guid accomodationAvailabilityId)
        {
            var accomodationAvailability = await _context.AccomodationAvailabilities.FirstOrDefaultAsync(x => x.Id == accomodationAvailabilityId) ?? throw new AccomodationAvailabilityNotFoundException("The accomodation availability with the id: " + accomodationAvailabilityId + " does not exist.");
            _context.AccomodationAvailabilities.Remove(accomodationAvailability);
            await _context.SaveChangesAsync();
        }
    }
}
