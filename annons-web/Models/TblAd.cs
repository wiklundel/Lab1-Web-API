using System;
using System.Collections.Generic;

namespace annons_web.Models;

public partial class TblAd
{
    public int AdId { get; set; }

    public string AdTitle { get; set; } = null!;

    public string AdContent { get; set; } = null!;

    public decimal AdPrice { get; set; }

    public decimal AdFee { get; set; }

    public int AdAdvertiserId { get; set; }

    public virtual TblAdvertiser AdAdvertiser { get; set; } = null!;
}
