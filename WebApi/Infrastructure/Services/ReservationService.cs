using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IPropertyRepository _propertyRepository;

    public ReservationService(
        IReservationRepository repository,
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository)
    {
        _repository = repository;
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsAsync(
        Guid? propertyId = null,
        Guid? roomTypeId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? guestName = null)
    {
        return await _repository.GetFilteredAsync(
            propertyId, roomTypeId, startDate, endDate, guestName);
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        if (reservation == null)
            throw new ArgumentNullException(nameof(reservation));

        if (reservation.ArrivalDate >= reservation.DepartureDate)
            throw new ArgumentException("Departure date must be after arrival date.");

        if (reservation.ArrivalDate < DateTime.Today)
            throw new ArgumentException("Arrival date cannot be in the past.");

        var roomType = await _roomTypeRepository.GetByIdAsync(reservation.RoomTypeId);
        if (roomType == null)
            throw new ArgumentException($"RoomType with Id {reservation.RoomTypeId} not found.");

        var property = await _propertyRepository.GetByIdAsync(reservation.PropertyId);
        if (property == null)
            throw new ArgumentException($"Property with Id {reservation.PropertyId} not found.");

        if (roomType.PropertyId != reservation.PropertyId)
            throw new ArgumentException("RoomType does not belong to the specified Property.");

        var isAvailable = await IsRoomTypeAvailableAsync(
            reservation.RoomTypeId,
            reservation.ArrivalDate,
            reservation.DepartureDate);

        if (!isAvailable)
            throw new InvalidOperationException("RoomType is not available for the specified dates.");

        var nights = (reservation.DepartureDate - reservation.ArrivalDate).Days;
        reservation.Total = roomType.DailyPrice * nights;
        reservation.Currency = roomType.Currency;
        reservation.Id = Guid.NewGuid();
        reservation.IsCancelled = false;

        return await _repository.AddAsync(reservation);
    }

    public async Task<bool> CancelReservationAsync(Guid id)
    {
        var reservation = await _repository.GetByIdAsync(id);
        if (reservation == null || reservation.IsCancelled)
            return false;

        return await _repository.CancelAsync(id);
    }

    public async Task<bool> IsRoomTypeAvailableAsync(
        Guid roomTypeId,
        DateTime arrivalDate,
        DateTime departureDate)
    {
        var existingReservations = await _repository.GetFilteredAsync(
            propertyId: null,
            roomTypeId: roomTypeId,
            startDate: arrivalDate,
            endDate: departureDate,
            guestName: null);
        
        return existingReservations.All(r => r.IsCancelled);
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsForRoomTypeAsync(
        Guid roomTypeId,
        DateTime startDate,
        DateTime endDate)
    {
        var allReservations = await _repository.GetFilteredAsync(
            propertyId: null,
            roomTypeId: roomTypeId,
            startDate: null,
            endDate: null,
            guestName: null);

        return allReservations
            .Where(r => !r.IsCancelled)
            .Where(r => startDate < r.DepartureDate && endDate > r.ArrivalDate)
            .OrderBy(r => r.ArrivalDate);
    }
}