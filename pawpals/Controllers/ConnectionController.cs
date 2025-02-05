using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pawpals.Data;
using pawpals.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/Connection/ListConnections
        [HttpGet("ListConnections")]
        public async Task<ActionResult<IEnumerable<Connection>>> GetConnections()
        {
            return await _context.Connections.ToListAsync();
        }

        // POST: api/Connection/AddConnection
        [HttpPost("AddConnection")]
        public async Task<ActionResult<Connection>> PostConnection(Connection connection)
        {
            _context.Connections.Add(connection);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConnection", new { id = connection.ConnectionId }, connection);
        }

        // PUT: api/Connection/UpdateConnection/5
        [HttpPut("UpdateConnection/{id}")]
        public async Task<IActionResult> PutConnection(int id, Connection connection)
        {
            if (id != connection.ConnectionId)
            {
                return BadRequest();
            }

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