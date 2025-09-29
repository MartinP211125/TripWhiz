
using TransportationDtoResponse = Core.DTOs.Response.TransportationDto;
using TransportationAvailabilityDtoResponse = Core.DTOs.Response.TransportationAvailabilityDto;
using TransportationAvailabilityDtoRequest = Core.DTOs.Request.TransportationAvailabilityDto;

namespace Service.Interfaces
{
    public interface ITransportationService
    {
        Task<TransportationDtoResponse> GetTransportationAsync(Guid id, DateTime? departureTime, DateTime? arivalTime);
        Task<ICollection<TransportationDtoResponse>> GetAllTransportationsAsync();
        Task<TransportationAvailabilityDtoResponse> CreateTransportationAvailabilityAsync(TransportationAvailabilityDtoRequest transportationAvailability);
        Task<ICollection<TransportationDtoResponse>> GetPopular();
        Task<ICollection<TransportationDtoResponse>> GetRecomendations();

    }
}
