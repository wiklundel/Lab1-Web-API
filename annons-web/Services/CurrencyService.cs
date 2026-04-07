using System.Text.Json;

namespace annons_web.Services;

public class CurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal?> ConvertFromSekAsync(decimal sekAmount, string targetCurrency)
    {
        if (targetCurrency == "SEK")
        {
            return sekAmount;
        }

        var response = await _httpClient.GetAsync(
            $"https://api.frankfurter.dev/v2/rates?base=SEK&quotes={targetCurrency}");

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();

        var data = JsonSerializer.Deserialize<List<ExchangeRateDto>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        
        if (data == null || data.Count == 0)
        {
            return null;
        }

        return sekAmount * data[0].Rate;
    }
}