using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class ProcessingService : IProcessingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProcessingService> _logger;
    private readonly PythonServiceSettings _pythonSettings;

    public ProcessingService(HttpClient httpClient, IOptions<PythonServiceSettings> pythonSettings, ILogger<ProcessingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _pythonSettings = pythonSettings.Value;
    }

    public async Task<Result<PaySlip>> ExtractDataOcrAsync(string bucketName,string fileKey)
    {
        try
        {
            var requestData = new { bucket_name=bucketName,file_key=fileKey };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_pythonSettings.BaseUrl}/payslip-data", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to extract payslip data. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorMessage);
                return Result<PaySlip>.Failure($"Error extracting payslip data: {errorMessage}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var payslipData = JsonSerializer.Deserialize<PaySlip>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return payslipData != null 
                ? Result<PaySlip>.Success(payslipData) 
                : Result<PaySlip>.Failure("Failed to parse payslip data.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while extracting payslip data.");
            return Result<PaySlip>.Failure("Unexpected error occurred while processing payslip data.");
        }
    }

    public async Task<Result<decimal>> GetExchangeRateAsync(int year)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_pythonSettings.BaseUrl}/exchange-rate-israel?year={year}");
            if (!response.IsSuccessStatusCode)
            {
                return Result<decimal>.Failure($"Failed to retrieve exchange rate: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);

            if (jsonDoc.RootElement.TryGetProperty("exchange_rate", out var rateElement) && rateElement.TryGetDecimal(out var exchangeRate))
            {
                return Result<decimal>.Success(exchangeRate);
            }

            return Result<decimal>.Failure("Exchange rate not found in response.");
        }
        catch (Exception ex)
        {
            return Result<decimal>.Failure($"Error fetching exchange rate: {ex.Message}");
        }
    }

}
