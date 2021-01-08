using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BinList_api.Services
{
    public class BinRequest
    {
        [Required]
        [MinLength(6)]
        public string CardNumber { get; set; }
    }
}
