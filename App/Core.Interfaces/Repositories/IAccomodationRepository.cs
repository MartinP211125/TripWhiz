
using Core.Entities.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAccomodationRepository
    {
        Task<Accomodation> GetAccomodationAsync(Guid accomodationId);
        Task<ICollection<Accomodation>> GetAccomodationsByPlaceAsync(Guid placeId);
        Task<ICollection<Accomodation>> GetAllAccomodationsAsync();
        Task<Accomodation> CreateAccomodationAsync(Accomodation accomodation);
        Task<Accomodation> UpdateAccomodationAsync(Guid accomodationId, Accomodation accomodation);
        Task<ICollection<AccomodationAvailability>> GetAccomodationAvailabilityAsync(Guid accomodationId);
        Task<AccomodationAvailability> CreateAccomodationAvailabilityAsync(AccomodationAvailability accomodationAvailability);
        Task<Place> GetPlaceAsync(Guid id);
        Task<Place> GetPlaceByCityAsync(string city);
        Task<Place> CreatePlaceAsync (Place place);
        Task<ICollection<Place>> GetPlacesAsync();
        Task DeleteAccomodationAcailabilitiesWithPassedDates();
        Task<ICollection<Room>> GetRoomsByAccomodationIdAsync(Guid accomodationId);
        Task DeleteAccomodationAvailabilityById(Guid accomodationAvailabilityId);
    }
}
