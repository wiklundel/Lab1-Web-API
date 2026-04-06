using System;
using System.Collections.Generic;

namespace prenumerant_api.Models;

public partial class TblSubscriber
{
    public int SubscriberId { get; set; }

    public string Name { get; set; } = null!;

    public string SocialSecurityNr { get; set; } = "";

    public string PhoneNr { get; set; } = "";

    public string Postcode { get; set; } = "";

    public string City { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;
}
