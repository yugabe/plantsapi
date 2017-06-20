using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Models
{
    public class ErrorModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
