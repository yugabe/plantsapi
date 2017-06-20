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
        public Level LightReqs { get; set; }
        public MonthFlags? PlantingTimes { get; set; }
        public MonthFlags? PickingTimes { get; set; }
        public MonthFlags BloomingTimes { get; set; }
        public Level WaterLevel { get; set; }
        public Level NutritionLevel { get; set; }
        public ICollection<PlantCategory> PlantCategories { get; set; }
        public ICollection<FavoritePlant> FavoritedByUsers { get; set; }
    }
}
