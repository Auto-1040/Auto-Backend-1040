using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using AutoMapper;

namespace Auto1040.Service
{
    public class OutputFormService : IOutputFormService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public OutputFormService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
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
    }
}
