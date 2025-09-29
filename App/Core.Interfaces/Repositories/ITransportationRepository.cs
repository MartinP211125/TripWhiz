
using Core.Entities.Entities;
using System.Data.Entity;

namespace Core.Interfaces.Repositories
{
    public interface ITransportationRepository
    {
        Task<Transportation> GetTransportationAsync(Guid id);
        Task<ICollection<Transportation>> GetAllTransportationsAsync();
        Task<Transportation> CreateTransportationAsync(Transportation transportation);
        Task<Transportation> UpdateTransportationAsync(Guid transactionId, Transportation transportation);
        Task<ICollection<TransportationAvailability>> GetTransportationAvailabilityAsync(Guid transportationId);
        Task<TransportationAvailability> CreateTransportationAvailabilityAsync(TransportationAvailability transportationAvailability);
        Task DeleteTransportationAcailabilitiesWithPassedDates();
        Task DeleteTransportationAvailabilityAsync(Guid transporationAvailabilityId);
    }
}
