using AccomodationDtoResponse = Core.DTOs.Response.AccomodationDto;
using AccomodationAvailabilityDtoRequest = Core.DTOs.Request.AccomodationAvailabilityDto;
using AccomodationAvailabilityDtoResponse = Core.DTOs.Response.AccomodationAvailabilityDto;

namespace Service.Interfaces
{
    public interface IAccomodationService
    {
        Task<AccomodationDtoResponse> GetAccomodationAsync(Guid accomodationId, DateTime? fromDate, DateTime? toDate);
        Task<ICollection<AccomodationDtoResponse>> GetAccomodationsByPlaceAsync(string place);
        Task<AccomodationAvailabilityDtoResponse> CreateAccomodationAvailabilityAsync(AccomodationAvailabilityDtoRequest accomodationAvailability);
        Task<ICollection<AccomodationDtoResponse>> GetPopular();
        Task<ICollection<AccomodationDtoResponse>> GetRecomendations();
    }
}
