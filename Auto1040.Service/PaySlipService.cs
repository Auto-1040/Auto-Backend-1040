using Amazon.Runtime.Internal;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auto1040.Service
{
    public class PaySlipService : IPaySlipService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;
        private readonly IProcessingService _processingService;
        private readonly IUserService _userService;

        public PaySlipService(IRepositoryManager repositoryManager, IMapper mapper, IS3Service s3Service, IProcessingService processingService,IUserService userService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _s3Service = s3Service;
            _processingService = processingService;
            _userService = userService;
        }

       

        public Result<IEnumerable<PaySlipDto>> GetPaySlipByUserId(int? userId,int authId)
        {
            if (authId != userId)
                return Result<IEnumerable<PaySlipDto>>.Forbid("You are not authorized to access this resource.");
            IEnumerable<PaySlip> paySlips;
            if (userId.HasValue)
            {
                paySlips = _repositoryManager.PaySlips.GetList().Where(p => p.UserId == userId && !p.IsDeleted);
            }
            else
            {
                if(_userService.IsUserAdmin(userId.Value))
                    return Result<IEnumerable<PaySlipDto>>.Forbid("You are not authorized to access this resource.");
                else
                    paySlips = _repositoryManager.PaySlips.GetList().Where(p => p.UserId == userId && !p.IsDeleted);
                paySlips = _repositoryManager.PaySlips.GetList().Where(p => !p.IsDeleted);
            }
            var paySlipDtos = _mapper.Map<IEnumerable<PaySlipDto>>(paySlips);
            return Result<IEnumerable<PaySlipDto>>.Success(paySlipDtos);
        }


        public Result<PaySlipDto> GetPaySlipById(int id)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(id);
            if (paySlip == null)
                return Result<PaySlipDto>.NotFound();
            if (paySlip.IsDeleted)
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
            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            _repositoryManager.PaySlips.Update(id, paySlip);
            var result = await CalcTotalIncomeAsync(paySlip);
            if (!result)
                return Result<bool>.Failure("Failed to calculate total income");
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> DeletePaySlip(int id)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(id);
            if (paySlip == null)
                return Result<bool>.NotFound();

            paySlip.IsDeleted = true;
            _repositoryManager.Save();

            return Result<bool>.Success(true);
        }
        
        
        public async Task<Result<PaySlipDto>> ProcessPaySlipFileAsync(IFormFile file, int userId)
        {
            // Upload the file to S3
            var fileName = Guid.NewGuid().ToString() + ".pdf"; // Generate a unique file name
            var fileUploadResult = await _s3Service.UploadFileAsync(fileName, file.OpenReadStream());
            if (!fileUploadResult.IsSuccess)
            {
                return Result<PaySlipDto>.Failure(fileUploadResult.ErrorMessage);
            }

            // Store paySlip to db
            var paySlip = new PaySlip
            {
                UserId = userId,
                S3Key = fileName,
                S3Url = fileUploadResult.Data
            };

            var savedPaySlip = _repositoryManager.PaySlips.Add(paySlip);
            _repositoryManager.Save();

            // Process the file with OCR
            var result = await _processingService.ExtractDataOcrAsync(S3Service.BucketName, paySlip.S3Key);
            if (!result.IsSuccess)
                return Result<PaySlipDto>.Failure(result.ErrorMessage);
            var paySlipFields = result.Data;

            // Store the extracted fields to the db
            await CalcTotalIncomeAsync(paySlipFields);
            _repositoryManager.PaySlips.Update(savedPaySlip.Id, paySlipFields);
            _repositoryManager.Save();
            return Result<PaySlipDto>.Success(_mapper.Map<PaySlipDto>(savedPaySlip));
        }

        public bool IsPaySlipOwner(int paySlipId,int authId)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(paySlipId);
            if (paySlip == null)
                return false;
            return paySlip.UserId == authId;
        }


        private async Task<bool> CalcTotalIncomeAsync(PaySlip paySlip)
        {
            try
            {
                paySlip.TotalIncomeILS = (paySlip.Field158_172 ?? 0) +
                                  (paySlip.Field218_219 ?? 0) * 0.075m +
                                  (paySlip.Field248_249 ?? 0) +
                                  (paySlip.Field36 ?? 0);
                paySlip.ExchangeRate = await GetYearlyAvgExchangeRateAsync(paySlip.TaxYear);
                paySlip.TotalIncomeUSD = paySlip.TotalIncomeILS / paySlip.ExchangeRate;
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private async Task<decimal> GetYearlyAvgExchangeRateAsync(int year)
        {
            var response = await _processingService.GetExchangeRateAsync(year);
            if (!response.IsSuccess)
                throw new Exception($"Failed to get exchange rate from API: {response.ErrorMessage}");
            return response.Data;
        }

       


    }
}
