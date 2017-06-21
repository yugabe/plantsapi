using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public class Plant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFrostProof { get; set; }
        public Level LightReq { get; set; }
        public MonthFlags? PlantingTime { get; set; }
        //public MonthFlags? PickingTimes { get; set; }
        //public MonthFlags BloomingTimes { get; set; }
        public Level WaterReq { get; set; }
        public Level NutritionReq { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<FavoritePlant> FavoritedByUsers { get; set; }
    }
}
