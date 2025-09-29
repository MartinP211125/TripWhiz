
using Service.Interfaces;
using OfferDtoResponse = Core.DTOs.Response.OfferDto;
using OfferDtoRequest = Core.DTOs.Request.OfferDto;
using Core.Interfaces.Repositories;
using Core.Entities.Entities;
using Core.Exceptions;
using Core.DTOs.Request;
using Core.Enums;

namespace Service.Services.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository offerRepository;
        public OfferService(IOfferRepository offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public async Task<OfferDtoResponse> CreateOfferAsync(CreateOfferRequest offerDto)
        {
            Offer offer = new Offer
            {
                AccomodationId = offerDto.AccomodationId,
                Title = offerDto.Title,
                Description = offerDto.Description,
                ImageUrl = offerDto.ImageUrl,
                CreatedBy = offerDto.CreatedBy,
                Price = offerDto.Price,
                FromDate = offerDto.FromDate,
                ToDate = offerDto.ToDate,
            };

            Offer createdOffer = await offerRepository.CreateOfferAsync(offer);

            return new OfferDtoResponse
            {
                Id = createdOffer.Id,
                AccomodationId = createdOffer.AccomodationId,
                Title = createdOffer.Title,
                Description = createdOffer.Description,
                ImageUrl = createdOffer.ImageUrl,
                CreatedBy = createdOffer.CreatedBy,
                Price = createdOffer.Price,
                FromDate = createdOffer.FromDate,
                ToDate = createdOffer.ToDate,
            };
        }

        public async Task<OfferDtoResponse> DeleteOfferAsync(Guid offerId)
        {
            OfferDtoResponse offer = await GetOfferByIdAsync(offerId);
            await offerRepository.DeleteOfferAsync(offerId);

            return offer;
        }

        public async Task<ICollection<OfferDtoResponse>> GetAllAsync()
        {
            ICollection<Offer> offers = await offerRepository.GetAllAsync();

            return offers.Select(x => new OfferDtoResponse
            {
                Id = x.Id,
                AccomodationId = x.AccomodationId,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CreatedBy = x.CreatedBy,
                Price = x.Price,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
            }).ToList();
        }

        public async Task<OfferDtoResponse> GetOfferByIdAsync(Guid offerId)
        {
            Offer offer = await offerRepository.GetOfferByIdAsync(offerId) ?? throw new OfferNullException("Offer with id " + offerId + " not found.");

            return new OfferDtoResponse
            {
                Id = offer.Id,
                AccomodationId = offer.AccomodationId,
                Title = offer.Title,
                Description = offer.Description,
                ImageUrl = offer.ImageUrl,
                CreatedBy = offer.CreatedBy,
                Price = offer.Price,
                FromDate = offer.FromDate,
                ToDate = offer.ToDate,
            };
        }

        public async Task<ICollection<OfferDtoResponse>> GetPopular()
        {
            ICollection<Offer> offers = await offerRepository.GetAllPopularAsync();

            return offers.Select(x => new OfferDtoResponse
            {
                Id = x.Id,
                AccomodationId = x.AccomodationId,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CreatedBy = x.CreatedBy,
                Price = x.Price,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
            }).ToList();
        }

        public async Task<ICollection<OfferDtoResponse>> GetRecomendations()
        {
            ICollection<Offer> offers = await offerRepository.GetAllRecomendationsAsync();

            return offers.Select(x => new OfferDtoResponse
            {
                Id = x.Id,
                AccomodationId = x.AccomodationId,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                CreatedBy = x.CreatedBy,
                Price = x.Price,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
            }).ToList();
        }
    }
}
