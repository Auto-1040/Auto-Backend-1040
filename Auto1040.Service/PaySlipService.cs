﻿using Amazon.Runtime.Internal;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using Auto1040.Core.Shared;
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
        private readonly IEmailService _emailService;

        public PaySlipService(IRepositoryManager repositoryManager, IMapper mapper, IEmailService emailService, IS3Service s3Service, IProcessingService processingService, IUserService userService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _s3Service = s3Service;
            _processingService = processingService;
            _userService = userService;
            _emailService = emailService;
        }



        public Result<IEnumerable<PaySlipDto>> GetPaySlipByUserId(int? userId, int authId)
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
                if (_userService.IsUserAdmin(userId.Value))
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


        public Result<bool> AddPaySlip(PaySlipDto paySlipDto)
        {
            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            CalcTotalIncome(paySlip);
            _repositoryManager.PaySlips.Add(paySlip);
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdatePaySlip(int id, PaySlipDto paySlipDto)
        {
            var existingPaySlip = _repositoryManager.PaySlips.GetById(id);
            if (existingPaySlip == null)
                return Result<bool>.NotFound();
            var paySlip = _mapper.Map<PaySlip>(paySlipDto);
            _repositoryManager.PaySlips.Update(id, paySlip);
            var result = CalcTotalIncome(paySlip);
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
            var user = _repositoryManager.Users.GetById(userId);

            if (user == null)
            {
                return Result<PaySlipDto>.NotFound("User not found");

            }

            // Upload the file to S3
            var fileName = Guid.NewGuid().ToString() + ".pdf"; // Generate a unique file name
            var fileUploadResult = await _s3Service.UploadFileAsync(fileName, file.OpenReadStream());
            if (!fileUploadResult.IsSuccess)
            {
                _ = Task.Run(() => SendFailureEmailAsync(user.Email, user.UserName));
                return Result<PaySlipDto>.Failure(fileUploadResult.ErrorMessage);
            }

            // Call the Python service 
            var year = DateTime.Now.Year - 1; // Default to the previous year
            Result<(PaySlip payslipData, decimal exchangeRate)> result;
            try
            {
                 result = await _processingService.ProcessPayslipAsync(S3Service.BucketName, fileName, year);
            }
            catch (HttpRequestException ex) when ((int?)((ex as dynamic)?.StatusCode) == 512)
            {
                //retry
                try
                {
                    result = await _processingService.ProcessPayslipAsync(S3Service.BucketName, fileName, year);
                }
                catch (Exception retryEx)
                {
                    result = Result<(PaySlip payslipData, decimal exchangeRate)>.BadRequest(retryEx.Message);
                }
            }

            if (!result.IsSuccess)
            {
                _ = Task.Run(() => SendFailureEmailAsync(user.Email, user.UserName));
                return Result<PaySlipDto>.Failure(result.ErrorMessage);
            }

            var (payslipData, exchangeRate) = result.Data;

            // Update payslip data with exchange rate
            payslipData.ExchangeRate = exchangeRate;
            payslipData.TotalIncomeUSD = payslipData.TotalIncomeILS / exchangeRate;
            payslipData.UserId = userId;
            payslipData.S3Key = fileName;
            payslipData.S3Url = fileUploadResult.Data;
            payslipData.CreatedAt = DateTime.Now;
            payslipData.UpdatedAt = DateTime.Now;
            CalcTotalIncome(payslipData);

            // Store the payslip in the database
            var savedPaySlip = _repositoryManager.PaySlips.Add(payslipData);
            _repositoryManager.Save();

            _ = Task.Run(() => SendSuccessEmailAsync(user.Email, user.UserName));
            return Result<PaySlipDto>.Success(_mapper.Map<PaySlipDto>(savedPaySlip));
        }


        public bool IsPaySlipOwner(int paySlipId, int authId)
        {
            var paySlip = _repositoryManager.PaySlips.GetById(paySlipId);
            if (paySlip == null)
                return false;
            return paySlip.UserId == authId;
        }


        private bool CalcTotalIncome(PaySlip paySlip)
        {
            try
            {
                paySlip.TotalIncomeILS = (paySlip.Field158_172 ?? 0) +
                                  (paySlip.Field218_219 ?? 0) * 0.075m +
                                  (paySlip.Field248_249 ?? 0) +
                                  (paySlip.Field36 ?? 0);
                paySlip.TotalIncomeUSD = paySlip.TotalIncomeILS / paySlip.ExchangeRate;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task SendSuccessEmailAsync(string userEmail, string userName)
        {
            var subject = "Your Form 106 Was Successfully Processed";
            var body = $@"
                        Dear {userName},

                        We’re happy to inform you that your Form 106 has been successfully uploaded and analyzed by our system.

                        The extracted data has been automatically added to your draft Form 1040, which you can now download in your personal dashboard.

                        If you have any questions or would like to consult with a tax advisor, feel free to reach out — we’re here to help.

                        Best regards,
                        The EZ1040 Team";

            var request = new EmailRequest
            {
                To = userEmail,
                Subject = subject,
                Body = body
            };

            await _emailService.SendEmailAsync(request);
        }

        private async Task SendFailureEmailAsync(string userEmail, string userName)
        {
            var subject = "Issue Processing Your Form 106";
            var body = $@"
                    Dear {userName},

                    Unfortunately, we were unable to complete the upload or analysis of your Form 106.

                    This may be due to a low-quality file or an unsupported format. Please double-check the file and try uploading it again.
                    If the issue persists, don’t hesitate to contact us for assistance.

                    Best regards,
                    The EZ1040 Team";

            var request = new EmailRequest
            {
                To = userEmail,
                Subject = subject,
                Body = body
            };

            await _emailService.SendEmailAsync(request);
        }





    }
}
