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
                LightReq = p.LightReq.ToStringFirstLower(),
                PlantingTime = p.PlantingTime.ToStringsFirstLower(),
                //PickingTime = p.PickingTimes.ToStringsFirstLower(),
                //BloomingTime = p.BloomingTimes.ToStringsFirstLower(),
                WaterReq = p.WaterReq.ToStringFirstLower(),
                NutritionReq = p.NutritionReq.ToStringFirstLower(),
                IsFavorite = currentUserId == null ? null : p.FavoritedByUsers?.Any(u => u.UserId == currentUserId)
            });
    }
}
