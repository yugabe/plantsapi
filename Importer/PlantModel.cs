using System;
using System.Collections.Generic;
using System.Text;
using Plants.API.Data;

namespace Plants.Importer
{
    public class PlantModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsFrostProof { get; set; }
        public LightRequirements LightReq { get; set; }
        public string Description { get; set; }
        public Month[] PlantingTime { get; set; }
        public WaterRequirements WaterReq { get; set; }
        public NutritionRequirements NutritionReq { get; set; }
        public string ImageUrl { get; set; }
    }
}
