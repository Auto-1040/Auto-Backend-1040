using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaySlipController : ControllerBase
    {
        private readonly IPaySlipService _paySlipService;
        private readonly IMapper _mapper;


        public PaySlipController(IPaySlipService paySlipService, IMapper mapper)
        {
            _paySlipService = paySlipService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PaySlipDto>> GetAll()
        {
            var result = _paySlipService.GetAllPaySlips();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public ActionResult<PaySlipDto> Get(int id)
        {
            var result = _paySlipService.GetPaySlipById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PaySlipDto>> Upload([FromForm] FileDto fileDto)
        {
            if (fileDto == null ||fileDto.File==null|| fileDto.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            var userId = GetUserId();
            // Call the service to process the file and extract the necessary data
            var result = await _paySlipService.ProcessPaySlipFileAsync(fileDto.File, userId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Add([FromBody] PaySlipPostModel paySlip)
        {
            if (paySlip == null)
                return BadRequest("Pay slip data is required.");

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            var result = await _paySlipService.AddPaySlipAsync(paySlipDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] PaySlipPostModel paySlip)
        {
            if (paySlip == null)
                return BadRequest("Pay slip data is required.");

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            var result =await _paySlipService.UpdatePaySlipAsync(id, paySlipDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var result = _paySlipService.DeletePaySlip(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("User ID claim is missing.");
            }
            return int.Parse(userIdClaim);
        }
    }
}
