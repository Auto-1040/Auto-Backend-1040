using Auto1040.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace Auto1040.Core.Services
{
    public interface IPaySlipService
    {
        Result<IEnumerable<PaySlipDto>> GetPaySlipByUserId(int? userId,int authId);
        Result<PaySlipDto> GetPaySlipById(int id);
        Task<Result<bool>> AddPaySlipAsync(PaySlipDto paySlipDto);
        Task<Result<bool>> UpdatePaySlipAsync(int id, PaySlipDto paySlipDto);
        Result<bool> DeletePaySlip(int id);
        Task<Result<PaySlipDto>> ProcessPaySlipFileAsync(IFormFile file, int userId);
        bool IsPaySlipOwner(int paySlipId, int authId);

    }
}
