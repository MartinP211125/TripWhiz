using Core.DTOs.Request;
using Core.Entities.Entities;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services.Groq
{
    public class GroqService : IGroqService
    {
            private readonly HttpClient _httpClient;
            private readonly HttpClient _pexelsClient;
            private readonly IAccomodationRepository _accomodationRepository;
            private readonly IOfferRepository _offerRepository;
            private readonly ITransportationRepository _transportationRepository;
            private readonly string _pexelsApiKey;

            public GroqService(
                HttpClient httpClient,
                IAccomodationRepository accomodationRepository,
                IOfferRepository offerRepository,
                ITransportationRepository transportationRepository,
                HttpClient pexelsClient,
                string pexelsApiKey)
            {
                _httpClient = httpClient;
                _accomodationRepository = accomodationRepository;
                _offerRepository = offerRepository;
                _transportationRepository = transportationRepository;
                _pexelsClient = pexelsClient;
                _pexelsApiKey = pexelsApiKey;
        }

            public async Task GetAccomodationAsync(string place)
            {
                string prompt = GenerateAccomodationPrompt(place);
                var payload = CreatePayload(prompt);

                var response = await _httpClient.PostAsJsonAsync("/openai/v1/chat/completions", payload);
                response.EnsureSuccessStatusCode();

                var groqResponse = await response.Content.ReadFromJsonAsync<GroqApiResponse>();
                var content = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(content))
                    throw new FailedGroqResponseException("Groq response content was empty");

                var json = ExtractJsonFromMarkdown(content);

                var result = JsonSerializer.Deserialize<ICollection<AccommodationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Count == 0)
                    throw new FailedGroqResponseException("Groq failed to load accommodations");

                await SaveAccomodationsWithImagesAsync(result);
            }

            public async Task GetOfferAsync()
            {
                string prompt = GenerateOfferPrompt();
                var payload = CreatePayload(prompt);

                var response = await _httpClient.PostAsJsonAsync("/openai/v1/chat/completions", payload);
                response.EnsureSuccessStatusCode();

                var groqResponse = await response.Content.ReadFromJsonAsync<GroqApiResponse>();
                var content = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(content))
                    throw new FailedGroqResponseException("Groq response content was empty");

                var json = ExtractJsonFromMarkdown(content);

                var result = JsonSerializer.Deserialize<ICollection<TransportationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Count == 0)
                    throw new FailedGroqResponseException("Groq failed to load offers");

                await SaveOffersWithImagesAsync(result);
            }

            public async Task GetTrendingOffers()
            {
                string prompt = GenerateTrendingOfferPrompt();
                var payload = CreatePayload(prompt);

                var response = await _httpClient.PostAsJsonAsync("/openai/v1/chat/completions", payload);
                response.EnsureSuccessStatusCode();

                var groqResponse = await response.Content.ReadFromJsonAsync<GroqApiResponse>();
                var content = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(content))
                    throw new FailedGroqResponseException("Groq response content was empty");

                var json = ExtractJsonFromMarkdown(content);

                var result = JsonSerializer.Deserialize<ICollection<TransportationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Count == 0)
                    throw new FailedGroqResponseException("Groq failed to load trending offers");

                await SaveOffersWithImagesAsync(result);
            }

            public async Task GetRecomendedOffers(ICollection<CustomeActivityDto> userActivities)
            {
                string prompt = GenerateRecomendedOfferPrompt(userActivities);
                var payload = CreatePayload(prompt);

                var response = await _httpClient.PostAsJsonAsync("/openai/v1/chat/completions", payload);
                response.EnsureSuccessStatusCode();

                var groqResponse = await response.Content.ReadFromJsonAsync<GroqApiResponse>();
                var content = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

                if (string.IsNullOrWhiteSpace(content))
                    throw new FailedGroqResponseException("Groq response content was empty");

                var json = ExtractJsonFromMarkdown(content);

                var result = JsonSerializer.Deserialize<ICollection<TransportationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Count == 0)
                    throw new FailedGroqResponseException("Groq failed to load recommended offers");

                await SaveOffersWithImagesAsync(result);
            }

            private static object CreatePayload(string prompt)
            {
                return new
                {
                    model = "meta-llama/llama-4-scout-17b-16e-instruct",
                    messages = new[] { new { role = "user", content = prompt } }
                };
            }

            private static string ExtractJsonFromMarkdown(string content)
            {
                var jsonStart = content.IndexOf("```json", StringComparison.OrdinalIgnoreCase);
                var jsonEnd = content.IndexOf("```", jsonStart + 1);

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                    return content.Substring(jsonStart + 7, jsonEnd - jsonStart - 7).Trim();

                return content.Trim();
            }

            private async Task SaveAccomodationsWithImagesAsync(ICollection<AccommodationDto> dtos)
            {
                foreach (var dto in dtos)
                {
                    if (dto?.Place == null ||
                        string.IsNullOrWhiteSpace(dto.Place.City) ||
                        string.IsNullOrWhiteSpace(dto.Place.Country))
                        continue;

                    var place = (await _accomodationRepository.GetPlacesAsync())
                        .FirstOrDefault(p => p.City == dto.Place.City && p.Country == dto.Place.Country);

                    if (place == null)
                    {
                        place = new Place
                        {
                            City = dto.Place.City,
                            Country = dto.Place.Country
                        };
                        await _accomodationRepository.CreatePlaceAsync(place);
                    }

                    var imageUrl = await GetImageFromPexelsAsync(dto.Name, dto.Place.City);

                    var accommodation = new Accomodation
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        StayType = (StayType)dto.StayType,
                        ImageUrl = imageUrl,
                        NumberOfRooms = dto.NumberOfRooms,
                        OfferType = (OfferType)dto.OfferType,
                        PlaceId = place.Id
                    };

                    await _accomodationRepository.CreateAccomodationAsync(accommodation);
                }
            }

            private async Task SaveOffersWithImagesAsync(ICollection<TransportationDto> dtos)
            
        {
                foreach (var dto in dtos)
                {
                    if (dto?.Offer?.Accommodation?.Place == null ||
                        string.IsNullOrWhiteSpace(dto.Offer.Accommodation.Place.City) ||
                        string.IsNullOrWhiteSpace(dto.Offer.Accommodation.Place.Country))
                        continue;

                    var place = (await _accomodationRepository.GetPlacesAsync())
                        .FirstOrDefault(p => p.City == dto.Offer.Accommodation.Place.City &&
                                             p.Country == dto.Offer.Accommodation.Place.Country);

                    if (place == null)
                    {
                        place = new Place
                        {
                            City = dto.Offer.Accommodation.Place.City,
                            Country = dto.Offer.Accommodation.Place.Country
                        };
                        await _accomodationRepository.CreatePlaceAsync(place);
                    }

                    var imageUrl = await GetImageFromPexelsAsync(dto.Offer.Accommodation.Name, dto.Offer.Accommodation.Place.City);

                    var accommodation = new Accomodation
                    {
                        Name = dto.Offer.Accommodation.Name,
                        Description = dto.Offer.Accommodation.Description,
                        StayType = (StayType)dto.Offer.Accommodation.StayType,
                        ImageUrl = imageUrl,
                        NumberOfRooms = dto.Offer.Accommodation.NumberOfRooms,
                        OfferType = (OfferType)dto.Offer.Accommodation.OfferType,
                        PlaceId = place.Id
                    };

                    await _accomodationRepository.CreateAccomodationAsync(accommodation);

                    var offer = new Offer
                    {
                        Title = dto.Offer.Title,
                        Description = dto.Offer.Description,
                        ImageUrl = imageUrl,
                        Price = dto.Offer.Price,
                        FromDate = dto.Offer.FromDate,
                        ToDate = dto.Offer.ToDate,
                        OfferType = (OfferType)dto.Offer.OfferType,
                        Accomodation = accommodation
                    };

                    await _offerRepository.CreateOfferAsync(offer);

                    var transportation = new Transportation
                    {
                        TransportationType = (TransportationType)dto.TransportationType,
                        Price = dto.Price,
                        NumberOfSeats = dto.NumberOfSeats,
                        Offer = offer
                    };

                    await _transportationRepository.CreateTransportationAsync(transportation);
                }
            }

            private async Task<string?> GetImageFromPexelsAsync(string accommodationName, string city)
            {
                var query = $"{accommodationName} {city}";
                var response = await _pexelsClient.GetAsync($"https://api.pexels.com/v1/search?query={Uri.EscapeDataString(query)}&per_page=1");

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadFromJsonAsync<PexelsSearchResponse>();
                return content?.Photos?.FirstOrDefault()?.Src?.Medium;
            }

            private class PexelsSearchResponse
            {
                public List<PexelsPhoto>? Photos { get; set; }
            }

            private class PexelsPhoto
            {
                public PexelsSrc? Src { get; set; }
            }

            private class PexelsSrc
            {
                public string? Medium { get; set; }
            }

            public class GroqApiResponse
            {
                public List<GroqChoice> Choices { get; set; }
            }

            public class GroqChoice
            {
                public GroqMessage Message { get; set; }
            }

            public class GroqMessage
            {
                public string Content { get; set; }
            }


            private string GenerateAccomodationPrompt(string place) => $@"
            Generate a JSON array of 20 accommodations (hotels/apartments) in {place} using camelCase and this structure:
            - name (string)
            - description (string)
            - stayType (enum): Hotel = 0, Apartment = 1
            - imageUrl (string)
            - numberOfRooms (int)
            - offerType (enum): Standard = 0, Trending = 1, Recomended = 2 
            - place:
                - city (string)
                - country (string)";

        private string GenerateOfferPrompt()
        {
            var (fromDate, toDate) = GetFutureDates();

            return $@"
                Generate a JSON array of 20 travel offers. Use camelCase for all keys.
                Each object must follow this structure:

                [
                  {{
                    transportationType: 0,
                    price: 100.0,
                    numberOfSeats: 50,
                    offer: {{
                      title: ""Sample title"",
                      description: ""..."",
                      imageUrl: ""..."",
                      price: 250.0,
                      fromDate: ""{fromDate}"",
                      toDate: ""{toDate}"",
                      offerType: 0,
                      accommodation: {{
                        name: ""..."",
                        description: ""..."",
                        stayType: 0,
                        imageUrl: ""..."",
                        numberOfRooms: 2,
                        offerType: 0,
                        place: {{
                          city: ""..."",
                          country: ""...""
                        }}
                      }}
                    }}
                  }}
                ]";
        }



        private string GenerateTrendingOfferPrompt()
        {
            var (fromDate, toDate) = GetFutureDates();

            return $@"
                Generate a JSON array of 20 trending travel offers using camelCase keys.
                All offers must have offerType = 1 and accommodation.offerType = 1.

                Use the following structure:

                [
                  {{
                    transportationType: 0,
                    price: 100.0,
                    numberOfSeats: 50,
                    offer: {{
                      title: ""Sample title"",
                      description: ""..."",
                      imageUrl: ""..."",
                      price: 250.0,
                      fromDate: ""{fromDate}"",
                      toDate: ""{toDate}"",
                      offerType: 1,
                      accommodation: {{
                        name: ""..."",
                        description: ""..."",
                        stayType: 0,
                        imageUrl: ""..."",
                        numberOfRooms: 2,
                        offerType: 1,
                        place: {{
                          city: ""..."",
                          country: ""...""
                        }}
                      }}
                    }}
                  }}
                ]";
        }



        private string GenerateRecomendedOfferPrompt(ICollection<CustomeActivityDto> activities)
        {
            var (fromDate, toDate) = GetFutureDates();

            var prioritizedActivities = activities
                .OrderByDescending(a => a.TimeStamp)
                .Where(a => !string.IsNullOrWhiteSpace(a.Query))
                .Select(a =>
                {
                    string priority = a.ActivityType switch
                    {
                        ActivityType.Book => "High priority",
                        ActivityType.Search => "Medium priority",
                        ActivityType.Click => "Low priority",
                        _ => "Unknown priority"
                    };

                    return $"- {priority} → '{a.Query}' in {a.City}, {a.Country} (Stay: {a.StayType}, Desc: {a.Description})";
                })
                .ToList();

            return $@"
                Based on the following user travel activity, recommend 20 international travel offers using camelCase keys.

                User activity summary:
                {string.Join("\n", prioritizedActivities)}

                All offers must have offerType = 2 and accommodation.offerType = 2.

                Use the following structure:

                [
                  {{
                    transportationType: 0,
                    price: 100.0,
                    numberOfSeats: 50,
                    offer: {{
                      title: ""Sample title"",
                      description: ""..."",
                      imageUrl: ""..."",
                      price: 250.0,
                      fromDate: ""{fromDate}"",
                      toDate: ""{toDate}"",
                      offerType: 2,
                      accommodation: {{
                        name: ""..."",
                        description: ""..."",
                        stayType: 0,
                        imageUrl: ""..."",
                        numberOfRooms: 2,
                        offerType: 2,
                        place: {{
                          city: ""..."",
                          country: ""...""
                        }}
                      }}
                    }}
                  }}
                ]";
        }

        private (string fromDate, string toDate) GetFutureDates()
        {
            var rand = new Random();
            var daysUntilStart = rand.Next(1, 365);
            var duration = rand.Next(3, 20);
            var from = DateTime.UtcNow.Date.AddDays(daysUntilStart);
            var to = from.AddDays(duration);
            return (from.ToString("yyyy-MM-ddTHH:mm:ss"), to.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

    }
}