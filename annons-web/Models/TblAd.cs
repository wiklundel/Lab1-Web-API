using System;
using System.Collections.Generic;

namespace annons_web.Models;

public partial class TblAd
{
    public int AdId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Fee { get; set; }

    public int AdvertiserId { get; set; }

    public virtual TblAdvertiser Advertiser { get; set; } = null!;
}
