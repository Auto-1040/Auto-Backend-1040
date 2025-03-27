using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;
using System.Text.Json;
using ThirdParty.Json.LitJson;

namespace Auto1040.Service
{
    public class OutputFormService : IOutputFormService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;
        private readonly IProcessingService _processingService;

        public OutputFormService(IRepositoryManager repositoryManager, IMapper mapper, IS3Service s3Service, IProcessingService processingService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _s3Service = s3Service;
            _processingService = processingService;
        }

        public Result<IEnumerable<OutputFormDto>> GetAllOutputForms()
        {
            var outputForms = _repositoryManager.OutputForms.GetList();
            var outputFormDtos = _mapper.Map<IEnumerable<OutputFormDto>>(outputForms);
            return Result<IEnumerable<OutputFormDto>>.Success(outputFormDtos);
        }

        public Result<OutputFormDto> GetOutputFormById(int id)
        {
            var outputForm = _repositoryManager.OutputForms.GetById(id);
            if (outputForm == null)
                return Result<OutputFormDto>.NotFound();

            var outputFormDto = _mapper.Map<OutputFormDto>(outputForm);
            return Result<OutputFormDto>.Success(outputFormDto);
        }

        public async Task<Result<OutputFormDto>> GenarateOutputFormAsync(int paySlipId)
        {
            // Generate filled file
            var paySlip = _repositoryManager.PaySlips.GetByIdWithUser(paySlipId);
            if (paySlip == null)
                return Result<OutputFormDto>.NotFound();
            var jsonData= ConvertUserToJson(paySlip.User, paySlip);
            var result = await _processingService.GenerateOutputFormAsync(jsonData);
            if (!result.IsSuccess)
                return Result<OutputFormDto>.Failure(result.ErrorMessage);

            // Upload the file to S3
            var fileName = Guid.NewGuid().ToString() + ".pdf"; // Generate a unique file name
            var fileUploadResult=await _s3Service.UploadFileAsync(fileName, result.Data.OpenReadStream());

            if (!fileUploadResult.IsSuccess)
            {
                return Result<OutputFormDto>.Failure(fileUploadResult.ErrorMessage);
            }

            // Store output form to db
            var outputForm = new OutputForm
            {
                UserId = paySlip.User.Id,
                S3Key = fileName,
                S3Url = fileUploadResult.Data,
                Year = paySlip.TaxYear
            };
            var savedOutputForm=_repositoryManager.OutputForms.Add(outputForm);
            _repositoryManager.Save();
            return Result<OutputFormDto>.Success(_mapper.Map<OutputFormDto>(savedOutputForm));

        }
        public Result<bool> AddOutputForm(OutputFormDto outputFormDto)
        {
            var outputForm = _mapper.Map<OutputForm>(outputFormDto);
            _repositoryManager.OutputForms.Add(outputForm);
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> UpdateOutputForm(int id, OutputFormDto outputFormDto)
        {
            var outputForm = _mapper.Map<OutputForm>(outputFormDto);
            if (outputForm == null)
                return Result<bool>.BadRequest("Cannot update output form to null reference");

            var existingOutputForm = _repositoryManager.OutputForms.GetById(id);
            if (existingOutputForm == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            _mapper.Map(outputFormDto, existingOutputForm);
            existingOutputForm.UpdatedAt = DateTime.UtcNow;

            var result = _repositoryManager.OutputForms.Update(id, existingOutputForm);
            if (result == null)
                return Result<bool>.Failure("Failed to update output form");

            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        public Result<bool> DeleteOutputForm(int id)
        {
            if (_repositoryManager.OutputForms.GetById(id) == null)
                return Result<bool>.NotFound($"Id {id} is not found");

            var result = _repositoryManager.OutputForms.Delete(id);
            if (!result)
                return Result<bool>.Failure("Failed to delete output form");

            _repositoryManager.Save();
            return Result<bool>.Success(result);
        }

        public Result<bool> SoftDelete(int id)
        {
            var outputForm = _repositoryManager.OutputForms.GetById(id);
            if (outputForm == null)
                return Result<bool>.NotFound();

            outputForm.IsDeleted = true;
            _repositoryManager.Save();
            return Result<bool>.Success(true);
        }

        private string ConvertUserToJson(User user, PaySlip paySlip)
        {
            if (user == null||paySlip==null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var userJson = new
            {
                first_name = user.UserDetails?.FirstName,
                last_name = user.UserDetails?.LastName,
                ssn = user.UserDetails?.Ssn,
                spouse_first_name = user.UserDetails?.SpouseFirstName,
                spouse_last_name = user.UserDetails?.SpouseLastName,
                spouse_ssn = user.UserDetails?.SpouseSsn,
                address = user.UserDetails?.HomeAddress,
                city = user.UserDetails?.City,
                foreign_country = user.UserDetails?.ForeignCountry,
                foreign_postal_code = user.UserDetails?.ForeignPostalCode,
                total_income = paySlip.TotalIncomeUSD
            };

            return JsonSerializer.Serialize(userJson);
        }
    }
}
