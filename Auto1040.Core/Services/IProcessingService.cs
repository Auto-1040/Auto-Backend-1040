using System.Threading.Tasks;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Shared;
using Microsoft.AspNetCore.Http;

public interface IProcessingService
{
    /// <summary>
    /// Extracts payslip data from a given S3 file URL using the Python service.
    /// </summary>
    /// <param name="fileUrl">The S3 URL of the payslip file.</param>
    /// <returns>A Result object containing the extracted payslip data or an error message.</returns>
    Task<Result<PaySlip>> ExtractDataOcrAsync(string bucketName,string fileKey);
    /// <summary>
    /// Retrieves the IRS exchange rate for Israel for a given year.
    /// </summary>
    /// <param name="year">The tax year.</param>
    /// <returns>A Result object containing the exchange rate or an error message.</returns>
    Task<Result<decimal>> GetExchangeRateAsync(int year);
    Task<Result<IFormFile>> GenerateOutputFormAsync(string jsonData);

}
