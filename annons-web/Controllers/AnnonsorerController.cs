using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using annons_web.Models;

namespace annons_web.Controllers;

public class AnnonsorerController : Controller
{
    private readonly AdsDbContext _context;

    public AnnonsorerController(AdsDbContext _context)
    {
        this._context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TblAnnonsorer>>> GetAdvertisers()
    {
        return await _context.TblAnnonsorer.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TblAnnonsorer>> GetAdvertiser(long id)
    {
        var advertiser = await _context.TblAnnonsorer.FindAsync(id);

        if (advertiser == null)
            return NotFound();
        
        return advertiser;
    }

    [HttpPost]
    public async Task<ActionResult<TblAnnonsorer>> Create(CreateAdViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if(model.AdvertiserType == TblAnnonsorer.AdvertiserTypeEnum.Subscriber)
        {
            // hämta data från API med prenumerationsnummer
            
        }
        else
        {
            // Företag
        }

        var annonsor = new TblAnnonsorer
        {
            Name = model.Name,
            PhoneNr = model.PhoneNr,
            DeliveryAddress = model.DeliveryAddress,
            Postcode = model.Postcode,
            City = model.City,
            AdvertiserType = model.AdvertiserType,
            CorporateRegNr = model.CorporateRegNr,
            InvoiceAddress = model.InvoiceAddress
        };

        _context.TblAnnonsorer.Add(annonsor);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdvertiser(long id, TblAnnonsorer advertiser)
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
            if (!_context.TblAnnonsorer.Any(e => e.AdvertiserId == id))
                return NotFound();
            
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdvertiser(long id)
    {
        var advertiser = await _context.TblAnnonsorer.FindAsync(id);

        if (advertiser == null)
            return NotFound();

        _context.TblAnnonsorer.Remove(advertiser);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}