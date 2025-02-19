// MemberController.cs

using GolfClubManagerAPI.Data;
using GolfClubManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GolfClubManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        // POST: api/member
        [HttpPost]
        public async Task<ActionResult<Member>> AddMember(Member member)
        {
            var addedMember = await _memberService.AddMemberAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = addedMember.Id }, addedMember);
        }

        // GET: api/member/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMemberById(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            return Ok(members);
        }
    }
}