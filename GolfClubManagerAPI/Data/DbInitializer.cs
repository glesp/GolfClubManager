using GolfClubManagerAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GolfClubManagerAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Make sure DB exists
            context.Database.EnsureCreated();
            
            Console.WriteLine("Starting database initialization...");
            
            // Only seed if tables are empty
            if (!context.Members.Any())
            {
                SeedMembers(context);
                Console.WriteLine("Members seeded successfully");
            }
            
            // Add tee time slots if none exist
            if (!context.TeeTimeSlots.Any())
            {
                SeedTeeTimeSlots(context);
                Console.WriteLine("Tee time slots seeded successfully");
            }
            
            // Add bookings if none exist - only after members and slots are seeded
            if (!context.TeeTimeBookings.Any() && context.Members.Any() && context.TeeTimeSlots.Any())
            {
                SeedBookings(context);
                Console.WriteLine("Bookings seeded successfully");
            }
        }
        
        private static void SeedMembers(ApplicationDbContext context)
        {
            try
            {
                // Using exact members from your database but WITHOUT the Id field
                var members = new List<Member>
                {
                    new Member { Name = "John Doe", Email = "johndoe@gmail.com", Handicap = 15, MembershipNumber = 1001, Gender = "Male" },
                    new Member { Name = "Emma O'Brien", Email = "emma.obrien@gmail.com", Handicap = 9, MembershipNumber = 1002, Gender = "Female" },
                    new Member { Name = "Daniel McCarthy", Email = "daniel.mccarthy@gmail.com", Handicap = 20, MembershipNumber = 1003, Gender = "Male" },
                    new Member { Name = "Sarah O'Connor", Email = "sarah.oconnor@gmail.com", Handicap = 12, MembershipNumber = 1004, Gender = "Female" },
                    new Member { Name = "Liam Murphy", Email = "liam.murphy@gmail.com", Handicap = 5, MembershipNumber = 1005, Gender = "Male" },
                    new Member { Name = "Ciara Kelly", Email = "ciara.kelly@gmail.com", Handicap = 18, MembershipNumber = 1006, Gender = "Female" },
                    new Member { Name = "Patrick O'Sullivan", Email = "patrick.osullivan@gmail.com", Handicap = 7, MembershipNumber = 1007, Gender = "Male" },
                    new Member { Name = "Aoife Byrne", Email = "aoife.byrne@gmail.com", Handicap = 14, MembershipNumber = 1008, Gender = "Female" },
                    new Member { Name = "Cian Duffy", Email = "cian.duffy@gmail.com", Handicap = 25, MembershipNumber = 1009, Gender = "Male" },
                    new Member { Name = "Niamh Fitzpatrick", Email = "niamh.fitzpatrick@gmail.com", Handicap = 11, MembershipNumber = 1010, Gender = "Female" },
                    new Member { Name = "Conor Walsh", Email = "conor.walsh@gmail.com", Handicap = 8, MembershipNumber = 1011, Gender = "Male" },
                    new Member { Name = "Sinead O'Neill", Email = "sinead.oneill@gmail.com", Handicap = 19, MembershipNumber = 1012, Gender = "Female" },
                    new Member { Name = "Ronan McCabe", Email = "ronan.mccabe@gmail.com", Handicap = 3, MembershipNumber = 1013, Gender = "Male" },
                    new Member { Name = "Laura Gallagher", Email = "laura.gallagher@gmail.com", Handicap = 22, MembershipNumber = 1014, Gender = "Female" },
                    new Member { Name = "Shane Nolan", Email = "shane.nolan@gmail.com", Handicap = 17, MembershipNumber = 1015, Gender = "Male" },
                    new Member { Name = "Megan Doyle", Email = "megan.doyle@gmail.com", Handicap = 6, MembershipNumber = 1016, Gender = "Female" },
                    new Member { Name = "Darragh O'Reilly", Email = "darragh.oreilly@gmail.com", Handicap = 12, MembershipNumber = 1017, Gender = "Male" },
                    new Member { Name = "Clare Healy", Email = "clare.healy@gmail.com", Handicap = 16, MembershipNumber = 1018, Gender = "Female" },
                    new Member { Name = "Owen Kavanagh", Email = "owen.kavanagh@gmail.com", Handicap = 10, MembershipNumber = 1019, Gender = "Male" },
                    new Member { Name = "Rebecca Brennan", Email = "rebecca.brennan@gmail.com", Handicap = 5, MembershipNumber = 1020, Gender = "Female" },
                    new Member { Name = "Brian O'Callaghan", Email = "brian.ocallaghan@gmail.com", Handicap = 13, MembershipNumber = 1021, Gender = "Male" },
                    new Member { Name = "Eimear Dunne", Email = "eimear.dunne@gmail.com", Handicap = 20, MembershipNumber = 1022, Gender = "Female" },
                    new Member { Name = "Tommy Sheridan", Email = "tommy.sheridan@gmail.com", Handicap = 4, MembershipNumber = 1023, Gender = "Male" },
                    new Member { Name = "Fiona Maguire", Email = "fiona.maguire@gmail.com", Handicap = 9, MembershipNumber = 1024, Gender = "Female" },
                    new Member { Name = "Kevin O'Donnell", Email = "kevin.odonnell@gmail.com", Handicap = 18, MembershipNumber = 1025, Gender = "Male" },
                    new Member { Name = "Siobhan Casey", Email = "siobhan.casey@gmail.com", Handicap = 21, MembershipNumber = 1026, Gender = "Female" },
                    new Member { Name = "James Nolan", Email = "james.nolan@gmail.com", Handicap = 11, MembershipNumber = 1027, Gender = "Male" },
                    new Member { Name = "Kate Farrell", Email = "kate.farrell@gmail.com", Handicap = 7, MembershipNumber = 1028, Gender = "Female" },
                    new Member { Name = "Cathal Moran", Email = "cathal.moran@gmail.com", Handicap = 23, MembershipNumber = 1029, Gender = "Male" },
                    new Member { Name = "Emily Keane", Email = "emily.keane@gmail.com", Handicap = 14, MembershipNumber = 1030, Gender = "Female" },
                    new Member { Name = "Conor Gillespie", Email = "conorgillespie@gmail.com", Handicap = 21, MembershipNumber = 1031, Gender = "Male" },
                    new Member { Name = "Conor Gillespie", Email = "conorgillespie1@gmail.com", Handicap = 12, MembershipNumber = 1032, Gender = "Male" },
                    new Member { Name = "Edel G", Email = "a@a.com", Handicap = 22, MembershipNumber = 1033, Gender = "Male" }
                };
                
                // Add in smaller batches to avoid parameter limits
                foreach (var chunk in members.Chunk(10))
                {
                    context.Members.AddRange(chunk);
                    context.SaveChanges(); // Save after each chunk
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedMembers: {ex.Message}");
                // Don't rethrow - let the app continue
            }
        }
        
        private static void SeedTeeTimeSlots(ApplicationDbContext context)
        {
            try
            {
                var slots = new List<TeeTimeSlot>();
                
                // Create slots for March 20 - April 3, 2025 to match your database
                DateTime startDate = new DateTime(2025, 3, 20);
                DateTime endDate = new DateTime(2025, 4, 3);
                
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // Create slots from 7 AM to 5 PM, 15-minute intervals
                    for (int hour = 7; hour < 17; hour++)
                    {
                        for (int minute = 0; minute < 60; minute += 15)
                        {
                            var slotTime = date.AddHours(hour).AddMinutes(minute);
                            slots.Add(new TeeTimeSlot { BookingTime = slotTime });
                        }
                    }
                }
                
                // Add in chunks to avoid too many parameters
                foreach (var chunk in slots.Chunk(50))
                {
                    context.TeeTimeSlots.AddRange(chunk);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedTeeTimeSlots: {ex.Message}");
            }
        }
        
        private static void SeedBookings(ApplicationDbContext context)
        {
            try
            {
                // Give the system a chance to generate slot IDs
                var slots = context.TeeTimeSlots.ToList();
                
                Console.WriteLine($"Found {slots.Count} slots for bookings");
                
                // Create bookings - we'll do this one by one to be safe
                AddBookingForMemberAndTime(context, "daniel.mccarthy@gmail.com", "2025-03-21 09:00:00");
                AddBookingForMemberAndTime(context, "cian.duffy@gmail.com", "2025-03-20 07:00:00");
                AddBookingForMemberAndTime(context, "cian.duffy@gmail.com", "2025-03-21 07:45:00");
                AddBookingForMemberAndTime(context, "cian.duffy@gmail.com", "2025-03-23 07:00:00");
                AddBookingForMemberAndTime(context, "brian.ocallaghan@gmail.com", "2025-03-20 07:00:00");
                AddBookingForMemberAndTime(context, "brian.ocallaghan@gmail.com", "2025-03-21 07:45:00");
                AddBookingForMemberAndTime(context, "brian.ocallaghan@gmail.com", "2025-03-22 07:00:00");
                AddBookingForMemberAndTime(context, "brian.ocallaghan@gmail.com", "2025-03-23 07:00:00");
                AddBookingForMemberAndTime(context, "eimear.dunne@gmail.com", "2025-03-20 07:15:00");
                AddBookingForMemberAndTime(context, "eimear.dunne@gmail.com", "2025-03-21 09:00:00");
                AddBookingForMemberAndTime(context, "tommy.sheridan@gmail.com", "2025-03-21 11:00:00");
                AddBookingForMemberAndTime(context, "siobhan.casey@gmail.com", "2025-03-21 11:00:00");
                AddBookingForMemberAndTime(context, "emily.keane@gmail.com", "2025-03-21 11:00:00");
                AddBookingForMemberAndTime(context, "emily.keane@gmail.com", "2025-03-22 07:00:00");
                AddBookingForMemberAndTime(context, "conorgillespie@gmail.com", "2025-03-20 07:15:00");
                AddBookingForMemberAndTime(context, "conorgillespie@gmail.com", "2025-03-21 10:00:00");
                AddBookingForMemberAndTime(context, "a@a.com", "2025-03-21 10:00:00");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeedBookings: {ex.Message}");
            }
        }
        
        // Helper to find member by email and slot by time 
        private static void AddBookingForMemberAndTime(ApplicationDbContext context, string email, string bookingTimeStr)
        {
            try
            {
                // Find member by email
                var member = context.Members.FirstOrDefault(m => m.Email == email);
                if (member == null) 
                {
                    Console.WriteLine($"Member with email {email} not found");
                    return;
                }
                
                // Parse booking time
                var bookingTime = DateTime.Parse(bookingTimeStr);
                
                // Find slot by time
                var slot = context.TeeTimeSlots.FirstOrDefault(s => s.BookingTime == bookingTime);
                if (slot == null)
                {
                    Console.WriteLine($"Slot for time {bookingTimeStr} not found");
                    return;
                }
                
                // Create and save booking
                var booking = new TeeTimeBooking
                {
                    MemberId = member.Id,
                    TeeTimeSlotId = slot.Id
                };
                
                context.TeeTimeBookings.Add(booking);
                context.SaveChanges();
                Console.WriteLine($"Added booking for {email} at {bookingTimeStr}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding booking: {ex.Message}");
            }
        }
    }
}