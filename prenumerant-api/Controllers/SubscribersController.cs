using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
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

    [HttpGet("export/xml")]
    public async Task<IActionResult> ExportSubscribersToXml()
    {
        var subscribers = await _context.TblSubscribers
            .Select(s => new SubscriberXmlDto
            {
                SubscriberId = s.SubscriberId,
                Name = s.Name,
                SocialSecurtiyNr = s.SocialSecurityNr,
                PhoneNr = s.PhoneNr,
                DeliveryAddress = s.DeliveryAddress,
                Postcode = s.Postcode,
                City = s.City
            })
            .ToListAsync();
            
        var xmlDto = new SubscribersXmlDto { Subscribers = subscribers };

        var serializer = new XmlSerializer(typeof(SubscribersXmlDto));
        using var stream  = new MemoryStream();
        serializer.Serialize(stream, xmlDto);
        stream.Position = 0;

        return File(stream.ToArray(), "application/xml", "subscribers.xml");
    }

    [HttpPost("import/xml")]
    public async Task<IActionResult> ImportSubscribersFromXml(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Ingen fil vald");
        }

        SubscribersXmlDto? xmlDto;

        var serializer = new XmlSerializer(typeof(SubscribersXmlDto));

        using (var stream = file.OpenReadStream())
        {
            xmlDto = serializer.Deserialize(stream) as SubscribersXmlDto;
        }

        if (xmlDto == null || xmlDto.Subscribers.Count == 0)
        {
            return BadRequest("Filen innehpller inga prenumeranter");
        }

        foreach (var s in xmlDto.Subscribers)
        {
            var exists = await _context.TblSubscribers.AnyAsync(x => x.SubscriberId == s.SubscriberId);

            if (!exists)
            {
                _context.TblSubscribers.Add(new TblSubscriber
                {
                    SubscriberId = s.SubscriberId,
                    Name = s.Name,
                    SocialSecurityNr = s.SocialSecurtiyNr,
                    PhoneNr = s.PhoneNr,
                    DeliveryAddress = s.DeliveryAddress,
                    Postcode = s.Postcode,
                    City = s.City
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok("Import klar");
    }
}