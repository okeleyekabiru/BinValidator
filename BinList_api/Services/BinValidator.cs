using BinList_api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BinList_api.Services
{
    public class BinValidator:IBinValidator
    {
        private readonly HttpClient _client;
        private readonly ILogger<BinValidator> _logger;
        private readonly BinConfig _binConfig;
        const string SUCCESS_MESSAGE = "Card  bin successfully verified";
        const string NOT_FOUND_MESSAGE = "Card record cannot be found";
        const string THROUTLING_MESSAGE = "ten request can only be process within one minutes, please try again after some minute";

        public BinValidator(HttpClient client,IOptions<BinConfig> binConfig,ILogger<BinValidator> logger)
        {
            _client = client;
            _logger = logger;
            _binConfig = binConfig.Value;
        }

        public async Task<BaseResponse<BinResponse>> VerifyCard(BinRequest request)
        {
            var responseObject = default(BinResponse);
            var url = $"{_binConfig.Url}{request.CardNumber}";
            var response = await _client.GetAsync(new Uri(url),HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation(responseString);
               responseObject = JsonConvert.DeserializeObject<BinResponse>(responseString);
               
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogDebug($"{response.ReasonPhrase}");
                return new BaseResponse<BinResponse>
                {
                    Code = "404",
                    Data = null,
                    isSuccessful = false,
                    Message = NOT_FOUND_MESSAGE
                };
            }
            else if(response.StatusCode ==HttpStatusCode.TooManyRequests)
            {
                _logger.LogDebug($"{response.ReasonPhrase}");
                return new BaseResponse<BinResponse>
                {
                    Code = "429",
                    Data = null,
                    isSuccessful = false,
                    Message = THROUTLING_MESSAGE
                };
            }

            return new BaseResponse<BinResponse>
            {
                Code = "200",
                Data = responseObject,
                isSuccessful = true,
                Message = SUCCESS_MESSAGE
            };
        }
    }
}
