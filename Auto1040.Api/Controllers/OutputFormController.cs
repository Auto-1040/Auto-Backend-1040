using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OutputFormController : ControllerBase
    {
        private readonly IOutputFormService _outputFormService;
        private readonly IMapper _mapper;

        public OutputFormController(IOutputFormService outputFormService, IMapper mapper)
        {
            _outputFormService = outputFormService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public ActionResult<IEnumerable<OutputFormDto>> GetAll()
        {
            var result = _outputFormService.GetAllOutputForms();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<OutputFormDto> Get(int id)
        {
            var result = _outputFormService.GetOutputFormById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<bool> Add([FromBody] OutputFormPostModel outputForm)
        {
            if (outputForm == null)
                return BadRequest("Output form data is required.");

            var outputFormDto = _mapper.Map<OutputFormDto>(outputForm);
            var result = _outputFormService.AddOutputForm(outputFormDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<bool> Update(int id, [FromBody] OutputFormPostModel outputForm)
        {
            if (outputForm == null)
                return BadRequest("Output form data is required.");

            var outputFormDto = _mapper.Map<OutputFormDto>(outputForm);
            var result = _outputFormService.UpdateOutputForm(id, outputFormDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<bool> Delete(int id)
        {
            var result = _outputFormService.DeleteOutputForm(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpPatch("{id}/soft-delete")]
        [Authorize]
        public ActionResult<bool> SoftDelete(int id)
        {
            var result = _outputFormService.SoftDelete(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }
    }
}
