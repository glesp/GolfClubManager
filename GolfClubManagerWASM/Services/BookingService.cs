using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GolfClubManagerWASM.DTOs;

namespace GolfClubManagerWASM.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TeeTimeSlotDTO>> GetAvailableSlotsAsync(DateTime date)
        {
            try
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetFromJsonAsync<List<TeeTimeSlotDTO>>($"api/TeeTime?date={formattedDate}");
                return response ?? new List<TeeTimeSlotDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching available slots: {ex.Message}");
                return new List<TeeTimeSlotDTO>();
            }
        }

        public async Task<List<BookingDisplayDTO>> GetBookingsForDateAsync(DateTime date)
        {
            try
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var response = await _httpClient.GetFromJsonAsync<List<BookingDisplayDTO>>($"api/Booking?date={formattedDate}");
                return response ?? new List<BookingDisplayDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching bookings: {ex.Message}");
                return new List<BookingDisplayDTO>();
            }
        }

        public async Task<bool> BookTeeTimeAsync(BookingDTO bookingDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Booking", bookingDTO);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error booking tee time: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MemberDTO>> GetAllMembersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<MemberDTO>>("api/Member");
                return response ?? new List<MemberDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching members: {ex.Message}");
                return new List<MemberDTO>();
            }
        }

        public async Task<int> AddMemberAsync(PlayerDTO newPlayer)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Member", newPlayer);
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<int>();
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error adding member: {ex.Message}");
                return 0;
            }
        }
    }
}