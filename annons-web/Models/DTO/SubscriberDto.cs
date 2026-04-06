namespace annons_web.Models;

public class SubscriberDto
{
    public int SubscriberId { get; set; }
    public string Name { get; set; } = "";
    public string PhoneNr { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
    public string Postcode { get; set; } = "";
    public string City { get; set; } = "";
}