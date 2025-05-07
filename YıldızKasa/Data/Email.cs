using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetEnv;

public class EmailValidationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey; 
    private readonly string _apiUrl;

    public EmailValidationService(){
        _httpClient = new HttpClient();
        Env.TraversePath().Load();
        _apiKey = Env.GetString("api_key");
        _apiUrl = Env.GetString("api_url");
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
    try
    {
        var requestUrl = $"{_apiUrl}?api_key={_apiKey}&email={email}";
        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmailValidationResponse>(jsonResponse);
            return result.is_valid_format.value;
        }
        else
        {
            Console.WriteLine("Hata: " + response.ReasonPhrase);
            return false;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Bir hata oluştu: {ex.Message}");
        return false;
    }
}
}

public class EmailValidationResponse
{
    public string email { get; set; }
    public string autocorrect { get; set; }
    public string deliverability { get; set; }
    public string quality_score { get; set; }
    public ValidationResult is_valid_format { get; set; }
    public ValidationResult is_free_email { get; set; }
    public ValidationResult is_disposable_email { get; set; }
    public ValidationResult is_role_email { get; set; }
    public ValidationResult is_catchall_email { get; set; }
    public ValidationResult is_mx_found { get; set; }
    public ValidationResult is_smtp_valid { get; set; }
}

public class ValidationResult
{
    public bool value { get; set; }
    public string text { get; set; }
}