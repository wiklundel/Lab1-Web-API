namespace annons_web.Models;
public class CreateAdViewModel
{
    public TblAnnonsorer.AdvertiserTypeEnum AdvertiserType { get; set; }

    // För prenumerant
    public string? SubscriberId { get; set; }

    // Gemensamma uppgifter
    public string Name { get; set; } = "";
    public string PhoneNr { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
    public string Postcode { get; set; } = "";
    public string City { get; set; } = "";

    // Endast företag
    public string? CorporateRegNr { get; set; }
    public string? InvoiceAddress { get; set; }

    // Annons
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public int Price { get; set; }

    // Valutakurser
    public string SelectedCurrency { get; set; } = "SEK";
    public decimal ConvertedCurrency { get; set; }
}