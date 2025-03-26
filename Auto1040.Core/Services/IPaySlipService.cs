using Auto1040.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace Auto1040.Core.Services
{
    public interface IPaySlipService
    {
        Result<IEnumerable<PaySlipDto>> GetAllPaySlips();
        Result<PaySlipDto> GetPaySlipById(int id);
        Task<Result<PaySlipDto>> ProcessPaySlipFileAsync(IFormFile file, int userId);
        Task<Result<bool>> AddPaySlipAsync(PaySlipDto paySlipDto);
        Task<Result<bool>> UpdatePaySlipAsync(int id, PaySlipDto paySlipDto);
        Result<bool> DeletePaySlip(int id);
    }
}
