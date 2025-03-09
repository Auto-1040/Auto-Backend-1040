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

        [HttpPost]
        public ActionResult<bool> Add([FromBody] PaySlipPostModel paySlip)
        {
            if (paySlip == null)
                return BadRequest("Pay slip data is required.");

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            var result = _paySlipService.AddPaySlip(paySlipDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> Update(int id, [FromBody] PaySlipPostModel paySlip)
        {
            if (paySlip == null)
                return BadRequest("Pay slip data is required.");

            var paySlipDto = _mapper.Map<PaySlipDto>(paySlip);
            var result = _paySlipService.UpdatePaySlip(id, paySlipDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var result = _paySlipService.DeletePaySlip(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.ErrorMessage);

            return NoContent();
        }
    }
}
