using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Data;

namespace Plants.API.Models.PlantsViewModels
{
    public class CreateUpdatePlantViewModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsFrostProof { get; set; }
        public string ImageUrl { get; set; }
        public Level LightReq { get; set; }
        public Month[] PlantingTime { get; set; }
        //public Month[] PickingTime { get; set; }
        //public Month[] BloomingTime { get; set; }
        public string Description { get; set; }
        public Level WaterLevel { get; set; }
        public Level NutritionLevel { get; set; }
    }
}
