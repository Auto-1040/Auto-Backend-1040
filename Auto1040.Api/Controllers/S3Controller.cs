using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Auto1040.Api.Controllers
{
    [ApiController]
    [Route("api/s3")]
    public class S3Controller : ControllerBase
    {
        private readonly IS3Service _s3Service;

        public S3Controller(IS3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpGet("presigned-url/upload")]
        public async Task<IActionResult> GetPreSignedURLForPost([FromQuery] string fileName)
        {
            var result = await _s3Service.GeneratePreSignedURLForPostAsync(fileName);
            if (result.IsSuccess)
            {
                var url = result.Data;
                return Ok(new { url });
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("presigned-url/download")]
        public async Task<IActionResult> GetPreSignedURLForGet([FromQuery] string fileName)
        {
            var result = await _s3Service.GeneratePreSignedURLForGetAsync(fileName);
            if (result.IsSuccess)
            {
                var url = result.Data;
                return Ok(new { url });
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
