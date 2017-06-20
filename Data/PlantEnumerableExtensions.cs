using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Models.PlantsViewModels;

namespace Plants.API.Data
{
    public static class PlantEnumerableExtensions
    {
        public static IEnumerable<PlantViewModel> ToViewModels(this IEnumerable<Plant> plants, string currentUserId)
            => plants?.Select(p => new PlantViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                IsFrostProof = p.IsFrostProof,
                LightReqs = p.LightReqs.ToStringFirstLower(),
                PlantingTime = p.PlantingTimes.ToStringsFirstLower(),
                PickingTime = p.PickingTimes.ToStringsFirstLower(),
                BloomingTime = p.BloomingTimes.ToStringsFirstLower(),
                WaterLevel = p.WaterLevel.ToStringFirstLower(),
                NutritionLevel = p.NutritionLevel.ToStringFirstLower(),
                IsSaved = currentUserId == null ? null : p.FavoritedByUsers?.Any(u => u.UserId == currentUserId)
            });
    }
}
