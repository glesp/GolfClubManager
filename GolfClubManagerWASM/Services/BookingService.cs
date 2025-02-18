using System.Net.Http.Json;
using GolfClubManagerWASM.Models;

namespace GolfClubManagerWASM.Services;

public class BookingService
{
    private readonly HttpClient _httpClient;
    private const string ApiBaseUrl = "http://localhost:5080/api/bookings"; // Ensure this is correct

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Fetch all bookings from the API
    /// </summary>
    public async Task<List<TeeTimeBooking>> GetBookingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TeeTimeBooking>>(ApiBaseUrl) ?? new List<TeeTimeBooking>();
    }

    /// <summary>
    /// Fetch available tee times for booking (next available slots)
    /// </summary>
    public async Task<List<DateTime>> GetAvailableTeeTimesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<DateTime>>($"{ApiBaseUrl}/available") ?? new List<DateTime>();
    }

    /// <summary>
    /// Attempt to book a tee time for a member
    /// </summary>
    public async Task<string> BookTeeTimeAsync(TeeTimeBooking booking)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiBaseUrl, booking);

        if (response.IsSuccessStatusCode)
            return "Success";

        var errorMessage = await response.Content.ReadAsStringAsync();
        return errorMessage;
    }


    /// <summary>
    /// Delete a booking (cancel)
    /// </summary>
    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{bookingId}");
        return response.IsSuccessStatusCode;
    }
}