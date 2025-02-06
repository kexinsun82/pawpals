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

        // GET: api/Connection/FindConnection/{id}
        [HttpGet("FindConnection/{id}")]
        public async Task<ActionResult<ConnectionDTO>> GetConnection(int id)
        {
            var connection = await _context.Connections
                .Include(c => c.Follower)
                .Include(c => c.Following)
                .FirstOrDefaultAsync(c => c.ConnectionId == id);

            if (connection == null)
            {
                return NotFound();
            }

            var connectionDto = new ConnectionDTO
            {
                ConnectionId = connection.ConnectionId,
                FollowerId = connection.FollowerId,
                FollowingId = connection.FollowingId
            };

            return Ok(connectionDto);
        }

        // GET: api/Connection/ListConnections
        [HttpGet("ListConnections")]
        public async Task<ActionResult<IEnumerable<ConnectionDTO>>> GetConnections()
        {
            var connections = await _context.Connections
                .Include(c => c.Follower)
                .Include(c => c.Following)
                .Select(c => new ConnectionDTO
                {
                    FollowerId = c.Follower.MemberId,
                    FollowingId = c.Following.MemberId
                })
                .ToListAsync();

            return Ok(connections);
        }

        // POST: api/Connection/AddConnection
        [HttpPost("AddConnection")]
        public async Task<ActionResult<ConnectionDTO>> PostConnection(ConnectionDTO connectionDto)
        {
            var newConnection = new Connection
            {
                FollowerId = connectionDto.FollowerId,
                FollowingId = connectionDto.FollowingId
            };

            _context.Connections.Add(newConnection);
            await _context.SaveChangesAsync();

            var createdDto = new ConnectionDTO
            {
                ConnectionId = newConnection.ConnectionId, // 由数据库生成
                FollowerId = newConnection.FollowerId,
                FollowingId = newConnection.FollowingId
            };

            return CreatedAtAction(nameof(GetConnection), new { id = createdDto.ConnectionId }, createdDto);
        }

        [HttpPut("UpdateConnection/{id}")]
        public async Task<IActionResult> PutConnection(int id, ConnectionDTO connectionDto)
        {
            if (id != connectionDto.ConnectionId)
            {
                return BadRequest("Connection ID does not match");
            }

            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            connection.FollowerId = connectionDto.FollowerId;
            connection.FollowingId = connectionDto.FollowingId;

            _context.Entry(connection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Connection/DeleteConnection/5
        [HttpDelete("DeleteConnection/{id}")]
        public async Task<IActionResult> DeleteConnection(int id)
        {
            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConnectionExists(int id)
        {
            return _context.Connections.Any(e => e.ConnectionId == id);
        }
    }
}