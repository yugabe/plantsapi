using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Data;

namespace Plants.API.Models.PlantsViewModels
{
    public class FilterQueryModel
    {
        public bool? IsFrostProof { get; set; }
        public bool? IsFavorite { get; set; }
        public LightRequirements[] LightReq { get; set; }
        public Month[] PlantingTime { get; set; }
        public WaterRequirements? WaterReq { get; set; }
        public NutritionRequirements? NutritionReq { get; set; }
        //public Month[] PickingTime { get; set; }
        //public Month[] BloomingTime { get; set; }

        public int? CategoryId { get; set; } // MOD
    }
}
