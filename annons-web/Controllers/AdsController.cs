using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using annons_web.Models;
using System.Text.Json;

namespace annons_web.Controllers;

public class AdsController : Controller
{
    private readonly AdsDbContext _context;
    private readonly IHttpClientFactory _httpClient;

    public AdsController(AdsDbContext context, IHttpClientFactory httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var ads = _context.TblAds
            .Include(a => a.AdAdvertiser)
            .ToList();

        return View(ads);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var vm = new CreateAdViewModel();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> GetSubscriber(CreateAdViewModel model)
    {
        if (model.SubscriberId == null)
        {
            ModelState.AddModelError("SubscriberId", "Ange Prenumerationsnummer.");
            return View("Create", model);
        }

        var client = _httpClient.CreateClient();

        var response = await client.GetAsync(
            $"http://localhost:5021/api/subscribers/{model.SubscriberId}");
        
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("SubscriberId", "Prenumeranten hittades inte");
            return View("Create", model);
        }

        var json = await response.Content.ReadAsStringAsync();

        var subscriber = JsonSerializer.Deserialize<SubscriberDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (subscriber == null)
        {
            ModelState.AddModelError("", "Kunde inte läsa prenumerantdata");
            return View("Create", model);
        }

        model.Name = subscriber.Name;
        model.PhoneNr = subscriber.PhoneNr;
        model.DeliveryAddress = subscriber.DeliveryAddress;
        model.Postcode = subscriber.Postcode;
        model.City = subscriber.City;
        model.AdvertiserType = TblAnnonsorer.AdvertiserTypeEnum.Subscriber;

        return View("Create", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAdViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var advertiser = MapToAdvertiser(model);

        _context.TblAnnonsorer.Add(advertiser);

        var ad = new TblAd
        {
            AdTitle = model.Title,
            AdContent = model.Content,
            AdPrice = model.Price,
            AdFee = model.AdvertiserType == TblAnnonsorer.AdvertiserTypeEnum.Subscriber ? 0 : 40,
                AdAdvertiser = advertiser
        };

        _context.TblAds.Add(ad);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    private TblAnnonsorer MapToAdvertiser(CreateAdViewModel model)
{
    return new TblAnnonsorer
    {
        Name = model.Name,
        PhoneNr = model.PhoneNr,
        DeliveryAddress = model.DeliveryAddress,
        Postcode = model.Postcode,
        City = model.City,
        CorporateRegNr = model.CorporateRegNr,
        InvoiceAddress = model.InvoiceAddress,
        AdvertiserType = model.AdvertiserType
    };
}
}
