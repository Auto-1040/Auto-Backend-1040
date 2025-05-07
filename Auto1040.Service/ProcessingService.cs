using System;
using System.Buffers.Text;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThirdParty.Json.LitJson;

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

    public async Task<Result<(PaySlip payslipData, decimal exchangeRate)>> ProcessPayslipAsync(string bucketName, string fileKey, int year)
    {
        try
        {
            var requestData = new
            {
                bucket_name = bucketName,
                file_key = fileKey,
                year = year
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            _logger.LogInformation("Processing payslip with data: {JsonData} service url: {BaseUrl}/process-payslip", jsonContent, _pythonSettings.BaseUrl);

            var response = await _httpClient.PostAsync($"{_pythonSettings.BaseUrl}/process-payslip", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to process payslip. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorMessage);
                return Result<(PaySlip, decimal)>.Failure($"Error processing payslip: {errorMessage}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(jsonResponse);

            // Extract payslip data
            var payslipData = result.RootElement.TryGetProperty("payslip_data", out var payslipElement) && payslipElement.ValueKind != JsonValueKind.Null
                ? JsonSerializer.Deserialize<PaySlip>(payslipElement.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                : null;

            // Extract exchange rate
            var exchangeRate = result.RootElement.TryGetProperty("exchange_rate", out var exchangeRateElement) &&
                               exchangeRateElement.TryGetProperty("rate", out var rateElement) &&
                               rateElement.TryGetDecimal(out var rate)
                ? rate
                : throw new Exception("Exchange rate not found in response.");

            if (payslipData == null)
            {
                return Result<(PaySlip, decimal)>.Failure("Failed to parse payslip data.");
            }

            return Result<(PaySlip, decimal)>.Success((payslipData, exchangeRate));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing payslip.");
            return Result<(PaySlip, decimal)>.Failure("Unexpected error occurred while processing payslip.");
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
    public async Task<Result<Stream>> GenerateOutputFormAsync(string jsonData)
    {
        try
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_pythonSettings.BaseUrl}/form-1040", content);


            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to generate form 1040. Status: {StatusCode}, Error: {Error}", response.StatusCode, errorMessage);
                return Result<Stream>.Failure($"Error generating form 1040: {errorMessage}");
            }

            var stream = await response.Content.ReadAsStreamAsync();

            return Result<Stream>.Success(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating form 1040.");
            return Result<Stream>.Failure("Unexpected error occurred while generating form 1040.");
        }
    }


}
