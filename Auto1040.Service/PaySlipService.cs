using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;

namespace Auto1040.Service
{
    public class PaySlipService : IPaySlipService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public PaySlipService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public Result<IEnumerable<PaySlipDto>> GetAllPaySlips()
        {
            var paySlips = _repositoryManager.PaySlips.GetList();
            var paySlipDtos = _mapper.Map<IEnumerable<PaySlipDto>>(paySlips);
            return Result<IEnumerable<PaySlipDto>>.Success(paySlipDtos);
        }

        public Result<PaySlipDto> GetPaySlipById(int id)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(id);
            if (paySlip == null)
                return Result<PaySlipDto>.NotFound();

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            return Result<PaySlipDto>.Success(paySlipDto);
        }

        public Result<bool> AddPaySlip(PaySlipDto paySlipDto)
        {
            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            _repositoryManager.PaySlips.Add(paySlip);
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdatePaySlip(int id, PaySlipDto paySlipDto)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(id);
            if (paySlip == null)
                return Result<bool>.NotFound();

            _mapper.Map(paySlipDto, paySlip);
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> DeletePaySlip(int id)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(id);
            if (paySlip == null)
                return Result<bool>.NotFound();

            var result = _repositoryManager.PaySlips.Delete(id);
            if (!result)
                return Result<bool>.Failure("Failed to delete user details");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }
    }
}
