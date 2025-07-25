using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using placement.Models;

namespace placement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly TamsdbContext _db;

        public QueryController(TamsdbContext db)
        {
            _db = db;
        }

        private string GetUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllQueries()
        {
            var queries = await _db.Queries
                                   .Include(q => q.Task)
                                   .Include(q => q.RaisedByNavigation)
                                   .ToListAsync();
            return Ok(queries);
        }

        // ✅ GET: api/query/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQueryById(int id)
        {
            var query = await _db.Queries
                                 .Include(q => q.QueryResponses)
                                 .FirstOrDefaultAsync(q => q.Qid == id);

            if (query == null)
                return NotFound(new { message = "Query not found" });

            return Ok(query);
        }


        //[HttpPost("add")]
        //public async Task<IActionResult> CreateQuery([FromForm] QueryDto model)
        //{

        //    var userId = HttpContext.Session.GetString("UserId");

        //    // ✅ Use TryParse for safety
        //    if (!int.TryParse(userId, out int parsedUserId) || parsedUserId == 0)
        //        return Unauthorized(new { message = "User not logged in or invalid user ID" });

        //    string? savedFileName = null;

        //    if (model.Attachment != null)
        //    {
        //        string imageName = new string(Path.GetFileNameWithoutExtension(model.Attachment.FileName)
        //            .Take(10).ToArray()).Replace(' ', '-');
        //        imageName += "-" + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(model.Attachment.FileName);

        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        //        Directory.CreateDirectory(uploadsFolder);

        //        var filePath = Path.Combine(uploadsFolder, imageName);
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await model.Attachment.CopyToAsync(stream);
        //        }
        //        savedFileName = imageName;
        //    }

        //    var query = new Query
        //    {
        //        TaskId = model.TaskId,
        //        RaisedBy = parsedUserId,
        //        Subject = model.Subject,
        //        Description = model.Description,
        //        Attachement = savedFileName,
        //        Status = "Open",
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _db.Queries.Add(query);
        //    await _db.SaveChangesAsync();

        //    return Ok(new { message = "Query created", query });
        //}

        [HttpPost("add")]
        public async Task<IActionResult> CreateQuery([FromForm] QueryDto model)
        {
            if (model.UserId == null || model.UserId <= 0)
                return BadRequest(new { message = "Invalid User ID provided" });

            string? savedFileName = null;

            if (model.Attachment != null)
            {
                string imageName = new string(Path.GetFileNameWithoutExtension(model.Attachment.FileName)
                    .Take(10).ToArray()).Replace(' ', '-');
                imageName += "-" + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(model.Attachment.FileName);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, imageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Attachment.CopyToAsync(stream);
                }
                savedFileName = imageName;
            }

            var query = new Query
            {
                TaskId = model.TaskId,
                RaisedBy = model.UserId.Value,
                Subject = model.Subject,
                Description = model.Description,
                Attachement = savedFileName,
                Status = "Open",
                CreatedAt = DateTime.UtcNow
            };

            _db.Queries.Add(query);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Query created", query });
        }



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateQuery(int id, [FromForm] QueryDto model)
        {
            var query = await _db.Queries.FindAsync(id);
            if (query == null) return NotFound(new { message = "Query not found" });

            query.Subject = model.Subject ?? query.Subject;
            query.Description = model.Description ?? query.Description;

            if (model.Attachment != null)
            {
               
                DeleteFile(query.Attachement);

                // save new attachment
                string imageName = new string(Path.GetFileNameWithoutExtension(model.Attachment.FileName)
                    .Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + "-" + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(model.Attachment.FileName);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, imageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Attachment.CopyToAsync(stream);
                }
                query.Attachement = imageName;
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Query updated", query });
        }

        
        [NonAction]
        public void DeleteFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        
    }
}
