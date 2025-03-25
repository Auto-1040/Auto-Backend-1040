using Amazon.Runtime.Internal;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;
using System.Threading.Tasks;

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

        public async Task<Result<bool>> AddPaySlipAsync(PaySlipDto paySlipDto)
        {
            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            await CalcTotalIncomeAsync(paySlip);
            _repositoryManager.PaySlips.Add(paySlip);
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdatePaySlipAsync(int id, PaySlipDto paySlipDto)
        {
            var existingPaySlip = _repositoryManager.PaySlips.GetById(id);
            if (existingPaySlip == null)
                return Result<bool>.NotFound();
            var paySlip=_mapper.Map<PaySlip>(paySlipDto);
            _repositoryManager.PaySlips.Update(id, paySlip);
            await CalcTotalIncomeAsync(paySlip);
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

        public async Task CalcTotalIncomeAsync(PaySlip paySlip)
        {
            paySlip.TotalIncomeILS = (paySlip.F158_172 ?? 0) +
                              (paySlip.F218_219 ?? 0) * 0.075m +
                              (paySlip.F248_249 ?? 0) +
                              (paySlip.F36 ?? 0);

            paySlip.ExchangeRate =await GetExchangeRateAsync();
            paySlip.TotalIncomeUSD = paySlip.TotalIncomeILS * paySlip.ExchangeRate;
        }
        public static async Task<decimal> GetExchangeRateAsync()//to do
        {
            return 4;
        }

        
    }
}
