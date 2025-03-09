using Auto1040.Core.DTOs;

namespace Auto1040.Core.Services
{
    public interface IPaySlipService
    {
        Result<IEnumerable<PaySlipDto>> GetAllPaySlips();
        Result<PaySlipDto> GetPaySlipById(int id);
        Result<bool> AddPaySlip(PaySlipDto paySlipDto);
        Result<bool> UpdatePaySlip(int id, PaySlipDto paySlipDto);
        Result<bool> DeletePaySlip(int id);
    }
}
