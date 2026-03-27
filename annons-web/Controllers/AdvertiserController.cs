using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using annons_web.Models;

namespace annons_web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvertiserController : Controller
{
    private readonly AdsDbContext _context;

    public AdvertiserController(AdsDbContext _context)
    {
        this._context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TblAdvertiser>>> GetAdvertisers()
    {
        return await _context.TblAdvertisers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TblAdvertiser>> GetAdvertiser(long id)
    {
        var advertiser = await _context.TblAdvertisers.FindAsync(id);

        if (advertiser == null)
            return NotFound();
        
        return advertiser;
    }

    [HttpPost]
    public async Task<ActionResult<TblAdvertiser>> CreateAdvertiser(TblAdvertiser advertiser)
    {
        _context.TblAdvertisers.Add(advertiser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAdvertiser),
        new { id = advertiser.AdvertiserId},
        advertiser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdvertiser(long id, TblAdvertiser advertiser)
    {
        if (id != advertiser.AdvertiserId)
            return BadRequest();

        _context.Entry(advertiser).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TblAdvertisers.Any(e => e.AdvertiserId == id))
                return NotFound();
            
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdvertiser(long id)
    {
        var advertiser = await _context.TblAdvertisers.FindAsync(id);

        if (advertiser == null)
            return NotFound();

        _context.TblAdvertisers.Remove(advertiser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}