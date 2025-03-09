using Auto1040.Core.DTOs;

namespace Auto1040.Core.Services
{
    public interface IOutputFormService
    {
        Result<IEnumerable<OutputFormDto>> GetAllOutputForms();
        Result<OutputFormDto> GetOutputFormById(int id);
        Result<bool> AddOutputForm(OutputFormDto outputFormDto);
        Result<bool> UpdateOutputForm(int id, OutputFormDto outputFormDto);
        Result<bool> DeleteOutputForm(int id);
        Result<bool> SoftDelete(int id);
    }
}
