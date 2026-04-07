namespace annons_web.Models;
public class AdListItemViewModel
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public decimal OriginalPrice { get; set; }
    public decimal DisplayPrice { get; set; }
    public decimal OriginalFee { get; set; }
    public decimal DisplayFee { get; set; }
    public string Currency { get; set; } = "SEK";
    public string SellerName { get; set; } = "";
    public string City { get; set; } = "";
    public string AdvertiserType { get; set; } = "";
}