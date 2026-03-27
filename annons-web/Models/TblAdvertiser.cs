using System;
using System.Collections.Generic;

namespace annons_web.Models;

public partial class TblAdvertiser
{
    public int AdvertiserId { get; set; }

    public string Name { get; set; } = null!;

    public int PhoneNr { get; set; }

    public int Postcode { get; set; }

    public string City { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public int? CorporateRegNr { get; set; }

    public string? InvoiceAddress { get; set; }

    public string AdvertiserType { get; set; } = null!;

    public virtual ICollection<TblAd> TblAds { get; set; } = new List<TblAd>();
}
