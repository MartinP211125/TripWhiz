
using Service.Interfaces;
using AccomodationDtoResponse = Core.DTOs.Response.AccomodationDto;
using AccomodationAvailabilityDtoRequest = Core.DTOs.Request.AccomodationAvailabilityDto;
using AccomodationAvailabilityDtoResponse = Core.DTOs.Response.AccomodationAvailabilityDto;
using Core.Interfaces.Repositories;
using Core.Entities.Entities;
using Core.Enums;
using Core.Exceptions;
using Quartz;

namespace Service.Services.Services
{
    public class AccomodationService : IAccomodationService
    {
        private readonly IAccomodationRepository accomodationRepository;
        private readonly IScheduler scheduler;

        public AccomodationService(IAccomodationRepository accomodationRepository, IScheduler scheduler)
        {
            this.accomodationRepository = accomodationRepository;
            this.scheduler = scheduler;
        }
        public async Task<AccomodationAvailabilityDtoResponse> CreateAccomodationAvailabilityAsync(AccomodationAvailabilityDtoRequest accomodationAvailabilityDto)
        {
            AccomodationAvailability accomodationAvailability = new AccomodationAvailability
            {
                RoomId = accomodationAvailabilityDto.RoomId,
                AccomodationId = accomodationAvailabilityDto.AccomodationId,
                FromDate = accomodationAvailabilityDto.FromDate,
                ToDate = accomodationAvailabilityDto.ToDate,
            };

            AccomodationAvailability createdAccomodationAvailability = await accomodationRepository.CreateAccomodationAvailabilityAsync(accomodationAvailability);

            return new AccomodationAvailabilityDtoResponse
            {
                Id = createdAccomodationAvailability.Id,
                RoomId = createdAccomodationAvailability.RoomId,
                AccomodationId = createdAccomodationAvailability.AccomodationId,
                FromDate = createdAccomodationAvailability.FromDate,
                ToDate= createdAccomodationAvailability.ToDate,
            };
        }

        public async Task<AccomodationDtoResponse> GetAccomodationAsync(Guid accomodationId, DateTime? fromDate, DateTime? toDate)
        {
            Accomodation accomodation = await accomodationRepository.GetAccomodationAsync(accomodationId) ?? throw new AccomodationNullException("Accomodation with Id " + accomodationId + " does not exist.");
            return new AccomodationDtoResponse
            {
                Id = accomodation.Id,
                PlaceId = accomodation.PlaceId,
                Name = accomodation.Name,
                Description = accomodation.Description,
                StayType = accomodation.StayType,
                ImageUrl = accomodation.ImageUrl,
            };
        }

        public async Task<ICollection<AccomodationDtoResponse>> GetAccomodationsByPlaceAsync(string place)
        {
            Place entity = await accomodationRepository.GetPlaceByCityAsync(place);
            ICollection<Accomodation> accomodations = await accomodationRepository.GetAccomodationsByPlaceAsync(entity.Id);
            if (accomodations == null || accomodations.Count <= 2) 
            {
                await TriggerJobAsync("GetMissingAccomodationsJob", place);
            }

            return accomodations.Select(x => new AccomodationDtoResponse
            {
                Id = x.Id,
                PlaceId = x.PlaceId,
                Name = x.Name,
                Description = x.Description,
                StayType = x.StayType,
                ImageUrl = x.ImageUrl,
            }).ToList();
        }

        public async Task<ICollection<AccomodationDtoResponse>> GetPopular()
        {
            ICollection<Accomodation> accomodations = (await accomodationRepository.GetAllAccomodationsAsync()).Where(x => x.OfferType == OfferType.Trending).ToList();
            return accomodations.Select(x => new AccomodationDtoResponse
            {
                Id = x.Id,
                PlaceId = x.PlaceId,
                Name = x.Name,
                Description = x.Description,
                StayType = x.StayType,
                ImageUrl = x.ImageUrl,
            }).ToList();
        }

        public async Task<ICollection<AccomodationDtoResponse>> GetRecomendations()
        {
            ICollection<Accomodation> accomodations = (await accomodationRepository.GetAllAccomodationsAsync()).Where(x => x.OfferType == OfferType.Recomended).ToList();
            return accomodations.Select(x => new AccomodationDtoResponse
            {
                Id = x.Id,
                PlaceId = x.PlaceId,
                Name = x.Name,
                Description = x.Description,
                StayType = x.StayType,
                ImageUrl = x.ImageUrl,
            }).ToList();
        }

        private async Task TriggerJobAsync(string jobId, string? place)
        {
            var jobKey = new JobKey(nameof(jobId));

            var jobData = new JobDataMap
        {
            { "place", place }
        };

            await scheduler.TriggerJob(jobKey, jobData);
        }
    }
}
