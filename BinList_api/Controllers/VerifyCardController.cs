using BinList_api.Models;
using BinList_api.Services;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BinList_api.Controllers
{
    [Route("api/v1/verify")]
    [ApiController]
    public class VerifyCardController : ControllerBase
    {
        private readonly IBinValidator _binValidator;
        private readonly ILogger<VerifyCardController> _logger;

        public VerifyCardController(IBinValidator binValidator,ILogger<VerifyCardController> logger)
        {
           _binValidator = binValidator;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("bin")]
        
        public async Task<ActionResult> VerifyCardBin([FromBody]BinRequest request)
        {
            BaseResponse<BinResponse> response = null;
            try
            {
                response = await _binValidator.VerifyCard(request);
                if (response == default)
                    throw new HttpRequestException();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"an error occured when verifying card {ex.Message } {ex.InnerException}");
                return StatusCode(StatusCodes.Status500InternalServerError,new BaseResponse<BinRequest>
                {
                    Message = $"an error occured when verifying card {ex.Message } {ex.InnerException}",
                    Code = "500",
                    Data = null,
                    isSuccessful = false
                });
            }
            return Ok(response);
        }
    }
}
