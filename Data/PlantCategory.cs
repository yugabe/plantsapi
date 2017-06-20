using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public class PlantCategory
    {
        public Plant Plant { get; set; }
        public int PlantId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
