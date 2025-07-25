using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using placement.Models;

namespace placement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryResponseController : ControllerBase
    {
        private readonly TamsdbContext _db;

        public QueryResponseController(TamsdbContext db)
        {
            _db = db;
        }

        // ✅ GET: api/QueryResponse/by-query/5
        // Get all responses for a specific query
        [HttpGet("by-query/{queryId}")]
        public async Task<IActionResult> GetResponsesForQuery(int queryId)
        {
            var responses = await _db.QueryResponses
                .Include(r => r.RespondedByNavigation) // if you want user info
                .Where(r => r.QueryId == queryId)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

            return Ok(responses);
        }

        // ✅ POST: api/QueryResponse/add
        // Add a new response to a query
        [HttpPost("add")]
        public async Task<IActionResult> AddResponse([FromBody] QueryResponse model)
        {
            if (model.QueryId == null || string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest(new { statusMessage = "QueryId and Message are required." });
            }

            model.CreatedAt = DateTime.UtcNow;
            _db.QueryResponses.Add(model);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                statusMessage = "Response added successfully",
                responseId = model.Qrid,
                responseMessage = model.Message,
                createdAt = model.CreatedAt
            });
        }


        // ✅ GET: api/QueryResponse/5
        // Optional: get a single response
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResponse(int id)
        {
            var response = await _db.QueryResponses.FindAsync(id);
            if (response == null)
            {
                return NotFound(new { message = "Response not found" });
            }
            return Ok(response);
        }

        // ✅ DELETE: api/QueryResponse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            var response = await _db.QueryResponses.FindAsync(id);
            if (response == null)
            {
                return NotFound(new { message = "Response not found" });
            }
            _db.QueryResponses.Remove(response);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Response deleted" });
        }
    }
}
