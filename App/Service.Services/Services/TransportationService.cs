
using Core.Interfaces.Repositories;
using Service.Interfaces;
using TransportationDtoResponse = Core.DTOs.Response.TransportationDto;
using TransportationAvailabilityDtoResponse = Core.DTOs.Response.TransportationAvailabilityDto;
using TransportationAvailabilityDtoRequest = Core.DTOs.Request.TransportationAvailabilityDto;
using Core.Entities.Entities;
namespace Service.Services.Services
{
    public class TransportationService : ITransportationService
    {
        private readonly ITransportationRepository transportationRepository;
        public TransportationService(ITransportationRepository transportationRepository)
        {
            this.transportationRepository = transportationRepository;
        }

        public async Task<TransportationAvailabilityDtoResponse> CreateTransportationAvailabilityAsync(TransportationAvailabilityDtoRequest transportationAvailabilityDto)
        {
            TransportationAvailability transportationAvailability = new TransportationAvailability
            {
                TransportationId = transportationAvailabilityDto.TransportationId,
                SeatId = transportationAvailabilityDto.SeatId,
                FromDate = transportationAvailabilityDto.Departure,
                ToDate = transportationAvailabilityDto.Arrival
            };

            TransportationAvailability createdAvailability = await transportationRepository.CreateTransportationAvailabilityAsync(transportationAvailability);

            return new TransportationAvailabilityDtoResponse
            {
                Id = createdAvailability.Id,
                TransportationId = createdAvailability.TransportationId,
                SeatId = createdAvailability.SeatId,
                Departure = createdAvailability.FromDate,
                Arrival = createdAvailability.ToDate,
            };
        }

        public async Task<ICollection<TransportationDtoResponse>> GetAllTransportationsAsync()
        {
            ICollection<Transportation> transportations = await transportationRepository.GetAllTransportationsAsync();

            return transportations.Select(x => new TransportationDtoResponse
            {
                Id = x.Id,
                TransportationType = x.TransportationType,
                OfferId = x.OfferId,
                Price = x.Price,
                
            }).ToList();
        }

        public async Task<ICollection<TransportationDtoResponse>> GetPopular()
        {
            ICollection<Transportation> transportations = await transportationRepository.GetAllTransportationsAsync();

            return transportations.Select(x => new TransportationDtoResponse
            {
                Id = x.Id,
                TransportationType = x.TransportationType,
                OfferId = x.OfferId,
                Price = x.Price,
            }).ToList();
            }

        public async Task<ICollection<TransportationDtoResponse>> GetRecomendations()
        {
            ICollection<Transportation> transportations = await transportationRepository.GetAllTransportationsAsync();

            return transportations.Select(x => new TransportationDtoResponse
            {
                Id = x.Id,
                TransportationType = x.TransportationType,
                OfferId = x.OfferId,
                Price = x.Price,
            }).ToList();
        }

        public async Task<TransportationDtoResponse> GetTransportationAsync(Guid id, DateTime? departureTime, DateTime? arivalTime)
        {
            Transportation transportation = await transportationRepository.GetTransportationAsync(id);

            return new TransportationDtoResponse
            {
                Id = transportation.Id,
                TransportationType = transportation.TransportationType,
                OfferId = transportation.OfferId,
                Price = transportation.Price,
            };
        }
    }
}
