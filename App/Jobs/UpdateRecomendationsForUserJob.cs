using Core.DTOs.Request;
using Core.Entities.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Infrastructure.Services.Groq;
using Quartz;
using System.Text.Json;

namespace Infrastructure.Jobs
{
    public class UpdateRecomendationsForUserJob : IJob
    {
        private readonly IGroqService groqService;
        private readonly ITransportationRepository transportationRepository;
        private readonly IAccomodationRepository accomodationRepository;
        private readonly IOfferRepository offerRepository;

        public UpdateRecomendationsForUserJob(
            IGroqService groqService,
            IAccomodationRepository accomodationRepository,
            IOfferRepository offerRepository,
            ITransportationRepository transportationRepository)
        {
            this.groqService = groqService;
            this.accomodationRepository = accomodationRepository;
            this.offerRepository = offerRepository;
            this.transportationRepository = transportationRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string activitiesJson = context.MergedJobDataMap.GetString("activities") ?? "[]";

            var activities = JsonSerializer.Deserialize<ICollection<UserActivity>>(activitiesJson)
                             ?? new List<UserActivity>();

            var activitiesDto = new List<CustomeActivityDto>();

            foreach (var activity in activities)
            {
                if (!activity.TargetId.HasValue)
                    continue;

                string name = string.Empty;
                string description = string.Empty;
                string stayType = string.Empty;
                string city = string.Empty;
                string country = string.Empty;

                switch (activity.TargetType)
                {
                    case TargetType.Accomodation:
                        var accomodation = await accomodationRepository.GetAccomodationAsync(activity.TargetId.Value);
                        if (accomodation != null)
                        {
                            name = accomodation.Name;
                            description = accomodation.Description;
                            stayType = accomodation.StayType == 0 ? "Hotel" : "Apartment";
                            city = accomodation.Place?.City ?? string.Empty;
                            country = accomodation.Place?.Country ?? string.Empty;
                        }
                        break;

                    case TargetType.Offer:
                        var offer = await offerRepository.GetOfferByIdAsync(activity.TargetId.Value);
                        if (offer != null)
                        {
                            name = offer.Title;
                            description = offer.Description;
                            stayType = offer.Accomodation?.StayType == 0 ? "Hotel" : "Apartment";
                            city = offer.Accomodation?.Place?.City ?? string.Empty;
                            country = offer.Accomodation?.Place?.Country ?? string.Empty;
                        }
                        break;

                    case TargetType.Transportation:
                        var transportation = await transportationRepository.GetTransportationAsync(activity.TargetId.Value);
                        if (transportation != null)
                        {
                            name = transportation.TransportationType.ToString();
                            description = $"OfferId: {transportation.OfferId}";
                            stayType = "Transportation";
                            city = string.Empty;
                            country = string.Empty;
                        }
                        break;
                }

                activitiesDto.Add(new CustomeActivityDto
                {
                    Name = name,
                    Description = description,
                    StayType = stayType,
                    City = city,
                    Country = country,
                    TimeStamp = activity.TimeStamp,
                    ActivityType = activity.ActivityType,
                    Query = activity.Query,
                });
            }

            if (activitiesDto.Any())
            {
                await groqService.GetRecomendedOffers(activitiesDto);
            }
        }
    }
}
