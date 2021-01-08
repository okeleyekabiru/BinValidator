using BinList_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinList_api.Services
{
    public interface IBinValidator
    {
        Task<BaseResponse<BinResponse>> VerifyCard(BinRequest request);
    }
}
