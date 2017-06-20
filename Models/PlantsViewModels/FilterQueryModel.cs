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
        public bool? IsSaved { get; set; }
        public Level[] LightReqs { get; set; }
        public Month[] PlantingTime { get; set; }
        public Month[] PickingTime { get; set; }
        public Month[] BloomingTime { get; set; }
        public Level[] WaterLevel { get; set; } // MOD
        public Level[] NutritionLevel { get; set; } // MOD

        public int? CategoryId { get; set; } // MOD
    }
}
