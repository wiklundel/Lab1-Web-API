using System.Xml.Serialization;

[XmlRoot("Subscribers")]
public class SubscribersXmlDto
{
    [XmlElement("Subscriber")]
    public List<SubscriberXmlDto> Subscribers { get; set; } = new();
}

public class SubscriberXmlDto
{
    public int SubscriberId { get; set; }
    public string Name { get; set; } = "";
    public string SocialSecurtiyNr { get; set; } = "";
    public string PhoneNr { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
    public string Postcode { get; set; } = "";
    public string City { get; set; } = "";
}