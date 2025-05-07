using System.Threading.Tasks;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Shared;
using Microsoft.AspNetCore.Http;

public interface IProcessingService
{
    Task<Result<(PaySlip payslipData, decimal exchangeRate)>> ProcessPayslipAsync(string bucketName, string fileKey, int year);
    Task<Result<decimal>> GetExchangeRateAsync(int year);
    Task<Result<Stream>> GenerateOutputFormAsync(string jsonData);

}
