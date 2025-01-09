using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Models;

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GameContext _context;

        public UsersController(GameContext context)
        {
            _context = context;
        }

        [HttpGet("session/{sessionId}")]
        public ActionResult<User> GetUser(string sessionId)
        {
            var user = _context.Users.FirstOrDefault(u => u.SessionId == sessionId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("session/{sessionId}")]
        public async Task<IActionResult> PutUser(string sessionId, User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if (existingUser == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            existingUser.NickName = user.NickName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while updating the user." });
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}