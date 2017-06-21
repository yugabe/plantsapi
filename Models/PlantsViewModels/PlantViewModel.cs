using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Data;

namespace Plants.API.Models.PlantsViewModels
{
    public class PlantViewModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public bool IsFrostProof { get; set; }
        public string LightReq { get; set; }
        public string[] PlantingTime { get; set; }
        public string Description { get; set; }
        public string WaterReq { get; set; }
        public string NutritionReq { get; set; }
        public bool? IsFavorite { get; set; }
        public string ImageUrl { get; set; }
        //public string[] PickingTime { get; set; }
        //public string[] BloomingTime { get; set; }
    }
}
