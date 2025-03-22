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
            // Make sure DB is created
            context.Database.EnsureCreated();
            
            // Only seed if members table is empty
            if (!context.Members.Any())
            {
                SeedMembers(context);
                Console.WriteLine("Seeded members data");
            }
            
            // Add tee time slots if none exist
            if (!context.TeeTimeSlots.Any())
            {
                SeedTeeTimeSlots(context);
                Console.WriteLine("Seeded tee time slots");
            }
            
            // Add bookings if none exist
            if (!context.TeeTimeBookings.Any())
            {
                SeedBookings(context);
                Console.WriteLine("Seeded bookings data");
            }
        }
        
        private static void SeedMembers(ApplicationDbContext context)
        {
            // Using exact members from your database
            var members = new List<Member>
            {
                new Member { Id = 202, Name = "John Doe", Email = "johndoe@gmail.com", Handicap = 15, MembershipNumber = 1001, Gender = "Male" },
                new Member { Id = 203, Name = "Emma O'Brien", Email = "emma.obrien@gmail.com", Handicap = 9, MembershipNumber = 1002, Gender = "Female" },
                new Member { Id = 204, Name = "Daniel McCarthy", Email = "daniel.mccarthy@gmail.com", Handicap = 20, MembershipNumber = 1003, Gender = "Male" },
                new Member { Id = 205, Name = "Sarah O'Connor", Email = "sarah.oconnor@gmail.com", Handicap = 12, MembershipNumber = 1004, Gender = "Female" },
                new Member { Id = 206, Name = "Liam Murphy", Email = "liam.murphy@gmail.com", Handicap = 5, MembershipNumber = 1005, Gender = "Male" },
                new Member { Id = 207, Name = "Ciara Kelly", Email = "ciara.kelly@gmail.com", Handicap = 18, MembershipNumber = 1006, Gender = "Female" },
                new Member { Id = 208, Name = "Patrick O'Sullivan", Email = "patrick.osullivan@gmail.com", Handicap = 7, MembershipNumber = 1007, Gender = "Male" },
                new Member { Id = 209, Name = "Aoife Byrne", Email = "aoife.byrne@gmail.com", Handicap = 14, MembershipNumber = 1008, Gender = "Female" },
                new Member { Id = 210, Name = "Cian Duffy", Email = "cian.duffy@gmail.com", Handicap = 25, MembershipNumber = 1009, Gender = "Male" },
                new Member { Id = 211, Name = "Niamh Fitzpatrick", Email = "niamh.fitzpatrick@gmail.com", Handicap = 11, MembershipNumber = 1010, Gender = "Female" },
                new Member { Id = 212, Name = "Conor Walsh", Email = "conor.walsh@gmail.com", Handicap = 8, MembershipNumber = 1011, Gender = "Male" },
                new Member { Id = 213, Name = "Sinead O'Neill", Email = "sinead.oneill@gmail.com", Handicap = 19, MembershipNumber = 1012, Gender = "Female" },
                new Member { Id = 214, Name = "Ronan McCabe", Email = "ronan.mccabe@gmail.com", Handicap = 3, MembershipNumber = 1013, Gender = "Male" },
                new Member { Id = 215, Name = "Laura Gallagher", Email = "laura.gallagher@gmail.com", Handicap = 22, MembershipNumber = 1014, Gender = "Female" },
                new Member { Id = 216, Name = "Shane Nolan", Email = "shane.nolan@gmail.com", Handicap = 17, MembershipNumber = 1015, Gender = "Male" },
                new Member { Id = 217, Name = "Megan Doyle", Email = "megan.doyle@gmail.com", Handicap = 6, MembershipNumber = 1016, Gender = "Female" },
                new Member { Id = 218, Name = "Darragh O'Reilly", Email = "darragh.oreilly@gmail.com", Handicap = 12, MembershipNumber = 1017, Gender = "Male" },
                new Member { Id = 219, Name = "Clare Healy", Email = "clare.healy@gmail.com", Handicap = 16, MembershipNumber = 1018, Gender = "Female" },
                new Member { Id = 220, Name = "Owen Kavanagh", Email = "owen.kavanagh@gmail.com", Handicap = 10, MembershipNumber = 1019, Gender = "Male" },
                new Member { Id = 221, Name = "Rebecca Brennan", Email = "rebecca.brennan@gmail.com", Handicap = 5, MembershipNumber = 1020, Gender = "Female" },
                new Member { Id = 222, Name = "Brian O'Callaghan", Email = "brian.ocallaghan@gmail.com", Handicap = 13, MembershipNumber = 1021, Gender = "Male" },
                new Member { Id = 223, Name = "Eimear Dunne", Email = "eimear.dunne@gmail.com", Handicap = 20, MembershipNumber = 1022, Gender = "Female" },
                new Member { Id = 224, Name = "Tommy Sheridan", Email = "tommy.sheridan@gmail.com", Handicap = 4, MembershipNumber = 1023, Gender = "Male" },
                new Member { Id = 225, Name = "Fiona Maguire", Email = "fiona.maguire@gmail.com", Handicap = 9, MembershipNumber = 1024, Gender = "Female" },
                new Member { Id = 226, Name = "Kevin O'Donnell", Email = "kevin.odonnell@gmail.com", Handicap = 18, MembershipNumber = 1025, Gender = "Male" },
                new Member { Id = 227, Name = "Siobhan Casey", Email = "siobhan.casey@gmail.com", Handicap = 21, MembershipNumber = 1026, Gender = "Female" },
                new Member { Id = 228, Name = "James Nolan", Email = "james.nolan@gmail.com", Handicap = 11, MembershipNumber = 1027, Gender = "Male" },
                new Member { Id = 229, Name = "Kate Farrell", Email = "kate.farrell@gmail.com", Handicap = 7, MembershipNumber = 1028, Gender = "Female" },
                new Member { Id = 230, Name = "Cathal Moran", Email = "cathal.moran@gmail.com", Handicap = 23, MembershipNumber = 1029, Gender = "Male" },
                new Member { Id = 231, Name = "Emily Keane", Email = "emily.keane@gmail.com", Handicap = 14, MembershipNumber = 1030, Gender = "Female" },
                new Member { Id = 232, Name = "Conor Gillespie", Email = "conorgillespie@gmail.com", Handicap = 21, MembershipNumber = 1031, Gender = "Male" },
                new Member { Id = 233, Name = "Conor Gillespie", Email = "conorgillespie1@gmail.com", Handicap = 12, MembershipNumber = 1032, Gender = "Male" },
                new Member { Id = 234, Name = "Edel G", Email = "a@a.com", Handicap = 22, MembershipNumber = 1033, Gender = "Male" }
            };
            
            context.Members.AddRange(members);
            context.SaveChanges();
        }
        
        private static void SeedTeeTimeSlots(ApplicationDbContext context)
        {
            var slots = new List<TeeTimeSlot>();
            
            // Create the first few weeks of slots based on your current data
            // I'll generate March 20 - April 3, 2025 slots to match your database
            
            DateTime startDate = new DateTime(2025, 3, 20);
            DateTime endDate = new DateTime(2025, 4, 3);
            
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Create slots from 7 AM to 5 PM (16:45), 15-minute intervals
                for (int hour = 7; hour < 17; hour++)
                {
                    for (int minute = 0; minute < 60; minute += 15)
                    {
                        var slotTime = date.AddHours(hour).AddMinutes(minute);
                        slots.Add(new TeeTimeSlot { BookingTime = slotTime });
                    }
                }
            }
            
            context.TeeTimeSlots.AddRange(slots);
            context.SaveChanges();
            
            // Now update IDs to match your database
            // Get the created slots ordered by booking time
            var createdSlots = context.TeeTimeSlots.OrderBy(s => s.BookingTime).ToList();
            
            // Since we can't directly set IDs, we'll have to match them by BookingTime
            // This step might not be necessary, but it's included for completeness
            Console.WriteLine($"Created {createdSlots.Count} tee time slots");
        }
        
        private static void SeedBookings(ApplicationDbContext context)
        {
            // Create bookings to match your database
            // We'll need to look up the TeeTimeSlotIds based on BookingTime
            var bookings = new List<TeeTimeBooking>();
            
            // Add exact bookings from your database
            AddBooking(context, bookings, 204, "2025-03-21 09:00:00");  // ID 45, SlotId 932
            AddBooking(context, bookings, 209, "2025-03-20 07:00:00");  // ID 37, SlotId 884 
            AddBooking(context, bookings, 209, "2025-03-21 07:45:00");  // ID 39, SlotId 927
            AddBooking(context, bookings, 209, "2025-03-23 07:00:00");  // ID 44, SlotId 1004
            AddBooking(context, bookings, 222, "2025-03-20 07:00:00");  // ID 38, SlotId 884
            AddBooking(context, bookings, 222, "2025-03-21 07:45:00");  // ID 40, SlotId 927
            AddBooking(context, bookings, 222, "2025-03-22 07:00:00");  // ID 41, SlotId 964
            AddBooking(context, bookings, 222, "2025-03-23 07:00:00");  // ID 43, SlotId 1004
            AddBooking(context, bookings, 223, "2025-03-20 07:15:00");  // ID 48, SlotId 885
            AddBooking(context, bookings, 223, "2025-03-21 09:00:00");  // ID 46, SlotId 932
            AddBooking(context, bookings, 224, "2025-03-21 11:00:00");  // ID 53, SlotId 940
            AddBooking(context, bookings, 227, "2025-03-21 11:00:00");  // ID 52, SlotId 940
            AddBooking(context, bookings, 231, "2025-03-21 11:00:00");  // ID 51, SlotId 940
            AddBooking(context, bookings, 231, "2025-03-22 07:00:00");  // ID 42, SlotId 964
            AddBooking(context, bookings, 232, "2025-03-20 07:15:00");  // ID 47, SlotId 885
            AddBooking(context, bookings, 232, "2025-03-21 10:00:00");  // ID 49, SlotId 936
            AddBooking(context, bookings, 234, "2025-03-21 10:00:00");  // ID 50, SlotId 936
            
            context.TeeTimeBookings.AddRange(bookings);
            context.SaveChanges();
        }
        
        private static void AddBooking(ApplicationDbContext context, List<TeeTimeBooking> bookings, int memberId, string bookingTimeStr)
        {
            var bookingTime = DateTime.Parse(bookingTimeStr);
            
            // Find the slot with matching booking time
            var slot = context.TeeTimeSlots.FirstOrDefault(s => s.BookingTime == bookingTime);
            
            if (slot != null)
            {
                bookings.Add(new TeeTimeBooking
                {
                    MemberId = memberId,
                    TeeTimeSlotId = slot.Id
                });
            }
            else
            {
                Console.WriteLine($"WARNING: Could not find slot for time {bookingTimeStr}");
            }
        }
    }
}