using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using placement.Models;

namespace placement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TamsdbContext _context;

        public TasksController(TamsdbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<placement.Models.Task>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<placement.Models.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

      

         // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<placement.Models.Task>> PostTask(placement.Models.Task task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Tid }, task);
        }

        // GET: api/Tasks/assigned-to/3
        [HttpGet("assigned-to/{userId}")]
        public async Task<ActionResult<IEnumerable<placement.Models.Task>>> GetTasksAssignedTo(int userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedTo == userId)
                .ToListAsync();
        }

        // GET: api/Tasks/assigned-by/1
        [HttpGet("assigned-by/{userId}")]
        public async Task<ActionResult<IEnumerable<placement.Models.Task>>> GetTasksAssignedBy(int userId)
        {
            return await _context.Tasks
                .Where(t => t.AssignedBy == userId)
                .ToListAsync();
        }

        // POST: api/Tasks/assign
        [HttpPost("assign")]
        public async Task<ActionResult<placement.Models.Task>> AssignTask([FromBody] placement.Models.Task task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Tid }, task);
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Tid == id);
        }
    }
}
