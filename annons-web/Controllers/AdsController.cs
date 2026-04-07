using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using annons_web.Models;
using System.Text.Json;
using annons_web.Services;
using Microsoft.AspNetCore.Components.Web;

namespace annons_web.Controllers;

public class AdsController : Controller
{
    private readonly AdsDbContext _context;
    private readonly IHttpClientFactory _httpClient;
    private readonly CurrencyService _currencyService;

    public AdsController(AdsDbContext context, IHttpClientFactory httpClient, CurrencyService currencyService)
    {
        _context = context;
        _httpClient = httpClient;
        _currencyService = currencyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string selectedCurrency = "SEK")
    {
        var ads = _context.TblAds
            .Include(a => a.AdAdvertiser)
            .ToList();

        var result = new List<AdListItemViewModel>();

        foreach (var ad in ads)
        {
            var convertedPrice = await _currencyService.ConvertFromSekAsync(ad.AdPrice, selectedCurrency);
            var convertedFee = await _currencyService.ConvertFromSekAsync(ad.AdFee, selectedCurrency);

            result.Add(new AdListItemViewModel
            {
                Title = ad.AdTitle,
                Content = ad.AdContent,
                OriginalPrice = ad.AdPrice,
                DisplayPrice = convertedPrice ?? ad.AdPrice,
                OriginalFee = ad.AdFee,
                DisplayFee = convertedFee ?? ad.AdFee,
                Currency = selectedCurrency,
                SellerName = ad.AdAdvertiser?.Name ?? "",
                City = ad.AdAdvertiser?.City ?? "",
                AdvertiserType = ad.AdAdvertiser?.AdvertiserType.ToString() ?? ""
            });
        }

        ViewBag.SelectedCurrency = selectedCurrency;
        return View(result);
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
        
        Console.WriteLine($"AdvertiserType: {model.AdvertiserType}");

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

    [HttpPost]
    public async Task<IActionResult> ImportXml(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ImportError"] = "Ingen fil vald.";
            return RedirectToAction("Index");
        }

        var client = _httpClient.CreateClient();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

        var response = await client.PostAsync(
            "http://localhost:5021/api/subscribers/import/xml",
            content
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            TempData["ImportError"] = $"Import misslyckades: {error}";
            return RedirectToAction("Index");
        }

        TempData["ImportSuccess"] = "Import lyckades.";
        return RedirectToAction("Index");
    }
}
