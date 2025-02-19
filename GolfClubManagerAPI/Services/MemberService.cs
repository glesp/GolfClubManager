// MemberService.cs
using GolfClubManagerAPI.Models;
using GolfClubManagerAPI.Data;
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
            // You can add validation here if necessary
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
        public async Task<List<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }
    
    }
    
}