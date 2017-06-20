using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Models;

namespace Plants.API.Data
{
    public class FavoritePlant
    {
        public Plant Plant { get; set; }
        public int PlantId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
