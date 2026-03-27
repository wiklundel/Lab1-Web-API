using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using annons_web.Models;

namespace annons_web.Controllers;

public class AdsController : Controller
{
    private readonly AdsDbContext _context;

    public AdsController(AdsDbContext _context)
    {
        this._context = _context;
    }

    public async Task<IActionResult> Index()
    {
        var ads = await _context.TblAds
            .Include(a => a.AdAdvertiser)
            .ToListAsync();

        return View(ads);
    }
}