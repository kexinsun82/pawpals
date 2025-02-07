using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pawpals.Data;
using pawpals.Models;
using Microsoft.EntityFrameworkCore;
using pawpals.Models.DTOs;

namespace pawpals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConnectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new follow relationship between two members.
        /// </summary>
        /// <example>
        /// POST api/Connection/Follow/1/2 -> "Followed successfully"
        /// ------
        /// Error: Already following
        /// POST api/Connection/Follow/1/2 -> "You are already following this user."
        /// </example>
        /// <returns>
        /// Success message if follow is successful. Error message if already following.
        /// </returns>
        [HttpPost("NewFollow/{memberId}/{followingId}")]
        public async Task<ActionResult> FollowUser(int memberId, int followingId)
        {
            // 确保用户不会关注自己
            if (memberId == followingId)
            {
                return BadRequest("You cannot follow yourself.");
            }

            // 检查是否已经关注过
            var existingConnection = await _context.Connections
                .FirstOrDefaultAsync(c => c.FollowerId == memberId && c.FollowingId == followingId);

            if (existingConnection != null)
            {
                return Conflict("You are already following this user.");
            }

            // 创建新关注关系
            var newConnection = new Connection
            {
                FollowerId = memberId,
                FollowingId = followingId
            };

            _context.Connections.Add(newConnection);
            await _context.SaveChangesAsync();

            return Ok("Followed successfully");
        }

        
        // [HttpGet("CheckFollowStatus/{memberId}/{followingId}")]
        // public async Task<ActionResult<bool>> CheckFollowStatus(int memberId, int followingId)
        // {
        //     var existingConnection = await _context.Connections
        //         .AnyAsync(c => c.FollowerId == memberId && c.FollowingId == followingId);

        //     return Ok(existingConnection);
        // }

        /// <summary>
        /// Deletes a existing connection between two members.
        /// </summary>
        /// <example>
        /// POST api/Connection/Unfollow/1/2 -> "Unfollowed successfully"
        /// ------
        /// Error: Connection not found
        /// POST api/Connection/Unfollow/1/2 -> "Connection not found."
        /// </example>
        /// <returns>
        /// Success message if follow is successful. Error message if already following.
        /// </returns>
        [HttpDelete("Unfollow/{memberId}/{followingId}")]
        public async Task<IActionResult> UnfollowUser(int memberId, int followingId)
        {
            var connection = await _context.Connections
                .FirstOrDefaultAsync(c => c.FollowerId == memberId && c.FollowingId == followingId);

            if (connection == null)
            {
                return NotFound("Connection not found.");
            }

            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();

            return Ok("Unfollowed successfully");
            // return NoContent();
        }

        
    }
}