// MemberService.cs

using System.ComponentModel.DataAnnotations;
using GolfClubManagerAPI.Data;
using GolfClubManagerAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GolfClubManagerAPI.Services
{
    public class MemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new member
        public async Task<Member> AddMemberAsync(Member member)
        {
            // Backend Validation: Ensure Name, Email, Gender, and Handicap are valid
            if (string.IsNullOrWhiteSpace(member.Name))
                throw new ArgumentException("Member name is required.");

            if (string.IsNullOrWhiteSpace(member.Email) || !new EmailAddressAttribute().IsValid(member.Email))
                throw new ArgumentException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(member.Gender) || (member.Gender != "Male" && member.Gender != "Female" && member.Gender != "Other"))
                throw new ArgumentException("Invalid gender. Must be Male, Female, or Other.");

            if (member.Handicap < 0 || member.Handicap > 54)
                throw new ArgumentException("Handicap must be between 0 and 54.");

            //  Generate MembershipNumber (auto-increment logic)
            var lastMember = await _context.Members
                .OrderByDescending(m => m.MembershipNumber)
                .FirstOrDefaultAsync();

            var newMembershipNumber = lastMember != null ? lastMember.MembershipNumber + 1 : 1;
            member.MembershipNumber = newMembershipNumber;

            //  Save valid member to the database
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
    
            return member;
        }

        
        // Get a member by their ID
        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        
        // Get all members
        public async Task<List<MemberDTO>> GetAllMembersAsync(string? gender, string? handicapRange, string? sortBy, string? order)
        {
            var query = _context.Members.AsQueryable();

            // 🏷️ Filter by Gender
            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(m => m.Gender == gender);
            }

            // 🎯 Filter by Handicap Range
            if (!string.IsNullOrWhiteSpace(handicapRange))
            {
                switch (handicapRange)
                {
                    case "Below 10":
                        query = query.Where(m => m.Handicap < 10);
                        break;
                    case "11-20":
                        query = query.Where(m => m.Handicap >= 11 && m.Handicap <= 20);
                        break;
                    case "Above 20":
                        query = query.Where(m => m.Handicap > 20);
                        break;
                }
            }

            //  Sorting Logic
            bool descending = order == "desc";
            query = sortBy switch
            {
                "Name" => descending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
                "Handicap" => descending ? query.OrderByDescending(m => m.Handicap) : query.OrderBy(m => m.Handicap),
                _ => query.OrderBy(m => m.Name) // Default: Sort by Name (asc)
            };

            return await query.Select(m => new MemberDTO
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Gender = m.Gender,
                Handicap = m.Handicap
            }).ToListAsync();
        }

    
    }
    
}