using Auto1040.Api.PostModels;
using Auto1040.Core.DTOs;
using Auto1040.Core.Entities;
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
        public ActionResult<IEnumerable<PaySlipDto>> GetAll(int? userId)
        {
            var authId = GetUserId();
            var result = _paySlipService.GetPaySlipByUserId(userId,authId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public ActionResult<PaySlipDto> Get(int id)
        {
            var userId = GetUserId();
            if (!_paySlipService.IsPaySlipOwner(id, userId))
                return Forbid("You are not authorized to access this resource.");

            var result = _paySlipService.GetPaySlipById(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] PaySlipPostModel paySlip)
        {
            var userId = GetUserId();
            if (!_paySlipService.IsPaySlipOwner(id, userId))
                return Forbid("You are not authorized to access this resource.");

            if (paySlip == null)
                return BadRequest("Pay slip data is required.");

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            var result =  _paySlipService.UpdatePaySlip(id, paySlipDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var userId = GetUserId();
            if (!_paySlipService.IsPaySlipOwner(id, userId))
                return Forbid("You are not authorized to access this resource.");

            var result = _paySlipService.DeletePaySlip(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PaySlipDto>> Upload([FromForm] FileDto fileDto)
        {
            if (fileDto == null || fileDto.File == null || fileDto.File.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            var userId = GetUserId();
            var result = await _paySlipService.ProcessPaySlipFileAsync(fileDto.File, userId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }

            return Ok(result.Data);
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
