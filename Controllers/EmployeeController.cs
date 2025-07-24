using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using placement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // For session

namespace placement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Employee")] // uncomment when using roles
    public class EmployeeController : ControllerBase
    {
        private readonly TamsdbContext _db;

        public EmployeeController(TamsdbContext db)
        {
            _db = db;
        }

        // ✅ Helper to get logged-in UserId from session
        private string GetUserId()
        {
            return HttpContext.Session.GetString("UserId");
        }

        // ✅ GET: api/employee/tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var tasks = await _db.Tasks
                .Where(t => t.AssignedTo == int.Parse(userId))
                .ToListAsync();

            return Ok(tasks);
        }

        // ✅ POST: api/employee/tasks/{taskId}/start
        [HttpPost("tasks/{taskId}/start")]
        public async Task<IActionResult> StartTask(int taskId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var log = new TaskLog
            {
                TaskId = taskId,
                EmpId = int.Parse(userId),
                StartTime = DateTime.UtcNow,
                BreakTime = TimeOnly.MinValue, // zero break
                CompleteTime = null
            };

            _db.TaskLogs.Add(log);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Task started successfully",
                logId = log.Lid,
                startTime = log.StartTime
            });
        }

        // ✅ POST: api/employee/tasks/{logId}/addbreak?minutes=10
        [HttpPost("tasks/{logId}/addbreak")]
        public async Task<IActionResult> AddBreak(int logId, [FromQuery] int minutes)
        {
            var log = await _db.TaskLogs.FindAsync(logId);
            if (log == null) return NotFound(new { message = "Log not found" });

            // Get current break time
            var currentBreak = log.BreakTime.HasValue
                ? log.BreakTime.Value.ToTimeSpan()
                : TimeSpan.Zero;

            // Add new minutes
            var updatedBreak = currentBreak + TimeSpan.FromMinutes(minutes);

            // Save back as TimeOnly
            log.BreakTime = TimeOnly.FromTimeSpan(updatedBreak);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Break time added",
                totalBreak = updatedBreak.TotalMinutes
            });
        }

        // ✅ POST: api/employee/tasks/{logId}/end
        [HttpPost("tasks/{logId}/end")]
        public async Task<IActionResult> EndTask(int logId)
        {
            var log = await _db.TaskLogs.FindAsync(logId);
            if (log == null) return NotFound(new { message = "Log not found" });

            log.EndTime = DateTime.UtcNow;

            // Calculate worked duration minus breaks
            var totalDuration = log.EndTime.Value - log.StartTime.Value;
            var breakDuration = log.BreakTime.HasValue ? log.BreakTime.Value.ToTimeSpan() : TimeSpan.Zero;
            var workedDuration = totalDuration - breakDuration;

            // Store workedDuration in CompleteTime (as TimeOnly)
            log.CompleteTime = TimeOnly.FromTimeSpan(workedDuration);

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Task ended successfully",
                totalWorkedMinutes = workedDuration.TotalMinutes,
                totalBreakMinutes = breakDuration.TotalMinutes
            });
        }

        // ✅ POST: api/employee/queries
        [HttpPost("queries")]
        public async Task<IActionResult> RaiseQuery([FromBody] Query model)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            model.RaisedBy = int.Parse(userId);
            model.Status = "Open";
            model.CreatedAt = DateTime.UtcNow;

            _db.Queries.Add(model);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Query raised successfully",
                queryId = model.Qid,
                model.Subject,
                model.Description,
                model.Status
            });
        }
    }
}
