
using Service.Interfaces;
using BookingDtoResponse = Core.DTOs.Response.BookingDto;
using BookingDtoRequest = Core.DTOs.Request.BookingDto;
using Core.Interfaces.Repositories;
using Core.Entities.Entities;
using Core.Exceptions;
using Core.Enums;

namespace Service.Services.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IOfferRepository offerRepository;
        private readonly IAccomodationRepository accomodationRepository;
        private readonly ITransportationRepository transportationRepository;
        public BookingService(IBookingRepository bookingRepository, IOfferRepository offerRepository, IAccomodationRepository accomodationRepository, ITransportationRepository transportationRepository)
        {
            this.bookingRepository = bookingRepository;
            this.offerRepository = offerRepository;
            this.accomodationRepository = accomodationRepository;
            this.transportationRepository = transportationRepository;
        }
        public async Task<BookingDtoResponse> CancelBooking(Guid bookingId)
        {
            Booking existingBooking = await bookingRepository.GetBookingForUserAsync(bookingId) ?? throw new BookingNullException("Booking with the id " + bookingId + " does not exist");
            existingBooking.Status = Status.Cancelled;
            foreach(var room in existingBooking.BookingRooms)
            {
                await accomodationRepository.DeleteAccomodationAvailabilityById(room.AccomodationAvailabilityId);
            }
            foreach (var seat in existingBooking.BookingSeats)
            {
                await transportationRepository.DeleteTransportationAvailabilityAsync(seat.TransportationAvailabilityId);
            }
            await bookingRepository.UpdateBookingForUserAsync(bookingId, existingBooking);
            return new BookingDtoResponse
            {
                Id = bookingId,
                UserId = existingBooking.UserId,
                OfferId = existingBooking.OfferId,
                Status = existingBooking.Status,
                NumberOfPeople = existingBooking.NumberOfPeople,
                TotalPrice = existingBooking.TotalPrice
            };
        }

        public async Task<BookingDtoResponse> CreateBookingForUserAsync(BookingDtoRequest bookingDto, string userId)
        {
            var offer = await offerRepository.GetOfferByIdAsync(bookingDto.OfferId);
            var allRooms = await accomodationRepository.GetRoomsByAccomodationIdAsync(offer.AccomodationId);
            var roomBookings = await accomodationRepository.GetAccomodationAvailabilityAsync(offer.AccomodationId);
            var selectedEnumValue = bookingDto.TransportationType; 
            int index = (int)selectedEnumValue;
            var transportation = await transportationRepository.GetTransportationAsync(offer.Transportation.ElementAt(0).Id);
            var takenSeatCount = (await transportationRepository.GetTransportationAvailabilityAsync(transportation.Id))
                .Where(x => x.FromDate == offer.FromDate && offer.ToDate == x.ToDate).Count();

            var freeRooms = allRooms
                .Where(room =>
                {
                    var bookingsForRoom = roomBookings.Where(b => b.RoomId == room.Id);
                    return !bookingsForRoom.Any(b =>
                        offer.FromDate < b.ToDate && offer.ToDate > b.FromDate
                    );
                })
                .ToList();

            var selectedRooms = GetMinimumRooms(freeRooms, bookingDto.NumberOfPeople);

            if (!selectedRooms.Any())
            {
                throw new NoRoomsAvailableException("No available rooms for the selected period and number of people.");
            }

            if(transportation.NumberOfSeats <= takenSeatCount + bookingDto.NumberOfPeople)
            {
                throw new NoSeatsAvailableExceptions("No seats available.");
            }

            Booking booking = new Booking
            {
                UserId = userId,
                OfferId = bookingDto.OfferId,
                Status = Status.Confirmed,
                NumberOfPeople = bookingDto.NumberOfPeople,
                TotalPrice = offer.Price * bookingDto.NumberOfPeople,
            };

            Booking createBooking = await bookingRepository.CreateBookingForUserAsync(booking);

            await AddAccomodationAvailability(selectedRooms, offer.FromDate, offer.ToDate.Value, createBooking.Id);
            await AddTransportationAvailability(transportation.Seats.Skip(takenSeatCount).Take(bookingDto.NumberOfPeople).ToList(), offer.FromDate, offer.ToDate.Value, createBooking.Id);
            return new BookingDtoResponse
            {
                Id = createBooking.Id,
                UserId = createBooking.UserId,
                OfferId = createBooking.OfferId,
                Status = createBooking.Status,
                NumberOfPeople = createBooking.NumberOfPeople,
                TotalPrice = createBooking.TotalPrice
            };
        }

        public async Task<BookingDtoResponse> GetBookingForUserAsync(Guid bookingId)
        {
            Booking booking = await bookingRepository.GetBookingForUserAsync(bookingId);

            return new BookingDtoResponse
            {
                Id = booking.Id,
                UserId = booking.UserId,
                OfferId = booking.OfferId,
                Status = booking.Status,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice
            };
        }

        public async Task<ICollection<BookingDtoResponse>> GetBookingsForUserByEmailAsync(string email)
        {
            ICollection<Booking> bookings = await bookingRepository.GetBookingsForUserByEmailAsync(email);

            return bookings.Select(x => new BookingDtoResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                OfferId = x.OfferId,
                Status = x.Status,
                NumberOfPeople = x.NumberOfPeople,
                TotalPrice = x.TotalPrice
            }).ToList();
        }

        private static List<Room> GetMinimumRooms(List<Room> rooms, int groupSize)
        {
            var sortedRooms = rooms.OrderByDescending(r => r.Capacity).ToList();

            for (int i = 1; i <= sortedRooms.Count; i++)
            {
                var combinations = GetCombinations(sortedRooms, i);

                foreach (var combo in combinations)
                {
                    if (combo.Sum(r => r.Capacity) >= groupSize)
                    {
                        return combo.ToList();
                    }
                }
            }

            return new List<Room>();
        }

        private static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 0) return new[] { Enumerable.Empty<T>() };

            return list.SelectMany((item, index) =>
                GetCombinations(list.Skip(index + 1), length - 1)
                    .Select(rest => new[] { item }.Concat(rest)));
        }

        private async Task AddAccomodationAvailability(List<Room> rooms, DateTime FromDate, DateTime ToData, Guid bookingId)
        {
            var availabilities = new List<AccomodationAvailability>();
            foreach (var room in rooms)
            {
                availabilities.Add(await accomodationRepository.CreateAccomodationAvailabilityAsync(new AccomodationAvailability
                {
                    AccomodationId = room.AccomodationId,
                    RoomId = room.Id,
                    Room = room,
                    FromDate = FromDate,
                    ToDate = ToData
                }));
            }

            availabilities.Select(async x => await bookingRepository.CreateBookingRoomAsync(new BookingRoom { BookingId = bookingId, AccomodationAvailabilityId = x.Id }));
        }

        private async Task AddTransportationAvailability(List<Seat> seats, DateTime FromDate, DateTime ToData, Guid bookingId)
        {
            var availabilities = new List<TransportationAvailability>();
            foreach (var seat in seats)
            {
                availabilities.Add(await transportationRepository.CreateTransportationAvailabilityAsync(new TransportationAvailability
                {
                    TransportationId = seat.TransportationId,
                    SeatId = seat.Id,
                    Seat = seat,
                    FromDate = FromDate,
                    ToDate = ToData
                }));
            }

            availabilities.Select(async x => await bookingRepository.CreateBookingSeatAsync(new BookingSeat { BookingId = bookingId, TransportationAvailabilityId = x.Id }));
        }
    }
}
