using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Models.PlantsViewModels;

namespace Plants.API.Data
{
    public static class PlantsFilterExtensions
    {
        public static IQueryable<Plant> FilterBy(this IQueryable<Plant> plants, FilterQueryModel filter, string currentUserId)
        {
            if (filter == null)
                return plants;

            if (filter.BloomingTime?.Any() == true)
            {
                var flags = filter.BloomingTime.ToFlags();
                if (flags != MonthFlags.None)
                    plants = plants.Where(p => (p.BloomingTimes & flags) != 0);
            }

            if (filter.CategoryId != null)
                plants = plants.Where(p => p.PlantCategories.Any(c => c.CategoryId == filter.CategoryId));

            if (filter.IsFrostProof != null)
                plants = plants.Where(p => p.IsFrostProof == filter.IsFrostProof);

            if (filter.IsSaved != null && currentUserId != null)
                plants = filter.IsSaved == true ? plants.Where(p => p.FavoritedByUsers.Any(pu => pu.UserId == currentUserId)) : plants.Where(p => p.FavoritedByUsers.All(pu => pu.UserId != currentUserId));

            if (filter.LightReqs?.Any() == true)
                plants = plants.Where(p => filter.LightReqs.Contains(p.LightReqs));

            if (filter.NutritionLevel?.Any() == true)
                plants = plants.Where(p => filter.NutritionLevel.Contains(p.NutritionLevel));

            if (filter.PickingTime?.Any() == true)
            {
                var flags = filter.PickingTime.ToFlags();
                if (flags != MonthFlags.None)
                    plants = plants.Where(p => p.PickingTimes == null || (p.PickingTimes & flags) != 0);
            }

            if (filter.PlantingTime?.Any() == true)
            {
                var flags = filter.PlantingTime.ToFlags();
                if (flags != MonthFlags.None)
                    plants = plants.Where(p => p.PlantingTimes == null || (p.PlantingTimes & flags) != 0);
            }

            if (filter.WaterLevel?.Any() == true)
                plants = plants.Where(p => filter.WaterLevel.Contains(p.WaterLevel));

            return plants;
        }
    }
}
