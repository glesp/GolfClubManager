// BookingService.cs (Frontend Service for API Calls)
using System.Net.Http.Json;
using GolfClubManagerWASM.DTOs;

namespace GolfClubManagerWASM.Services;

public class BookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TeeTimeSlotDTO>> GetAvailableSlotsAsync(DateTime date)
    {
        return await _httpClient.GetFromJsonAsync<List<TeeTimeSlotDTO>>($"api/TeeTime/GetTeeTimeSlots/{date:yyyy-MM-dd}") ?? new List<TeeTimeSlotDTO>();
    }

    public async Task<List<BookingDisplayDTO>> GetBookingsForDateAsync(DateTime date)
    {
        return await _httpClient.GetFromJsonAsync<List<BookingDisplayDTO>>($"api/Booking/bookingsForDate/{date:yyyy-MM-dd}") ?? new List<BookingDisplayDTO>();
    }

    public async Task<bool> BookTeeTimeAsync(BookingDTO bookingDTO)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Booking", bookingDTO);
        return response.IsSuccessStatusCode;
    }
}