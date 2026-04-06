using System;
using System.Collections.Generic;

namespace annons_web.Models;

public partial class TblAnnonsorer
{
    public enum AdvertiserTypeEnum
    {
        Subscriber,
        Company
    }
    public int AdvertiserId { get; set; }

    public string Name { get; set; } = "";

    public string PhoneNr { get; set; } = "";

    public string Postcode { get; set; } = "";

    public string City { get; set; } = "";

    public string DeliveryAddress { get; set; } = "";

    public string? CorporateRegNr { get; set; }

    public string? InvoiceAddress { get; set; }

    public AdvertiserTypeEnum AdvertiserType { get; set; }

    public virtual ICollection<TblAd> TblAds { get; set; } = new List<TblAd>();
}
