using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prenumerant_api.Models;

namespace prenumerant_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscribersController : ControllerBase
{
    private readonly SubscribersDbContext _context;

    public SubscribersController(SubscribersDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TblSubscriber>>> GetSubscribers()
    {
        return await _context.TblSubscribers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TblSubscriber>> GetSubscriber(long id)
    {
        var subscriber = await _context.TblSubscribers
            .FirstOrDefaultAsync(s => s.SubscriberId == id);

        if (subscriber == null)
        {
            return NotFound(new {message = "Subscriber not found"});
        }

        return subscriber;
    }

    [HttpPost]
    public async Task<ActionResult<TblSubscriber>> CreateSubscriber(TblSubscriber subscriber)
    {
        _context.TblSubscribers.Add(subscriber);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSubscriber),
        new { id = subscriber.SubscriberId },
        subscriber);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscriber(long id, TblSubscriber subscriber)
    {
        if (id != subscriber.SubscriberId)
        {
            return BadRequest();
        }

        _context.Entry(subscriber).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TblSubscribers.Any(e => e.SubscriberId == id))
                return NotFound();
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscriber(long id)
    {
        var subscriber = await _context.TblSubscribers.FindAsync(id);

        if (subscriber == null)
            return NotFound();

        _context.TblSubscribers.Remove(subscriber);
        await _context.SaveChangesAsync();

        return NoContent();
    }   
}