using System;
using System.Collections.Generic;

namespace prenumerant_api.Models;

public partial class TblSubscriber
{
    public int SubscriberNr { get; set; }

    public string Name { get; set; } = null!;

    public int SocialSecurityNr { get; set; }

    public int PhoneNr { get; set; }

    public int Postcode { get; set; }

    public string City { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;
}
