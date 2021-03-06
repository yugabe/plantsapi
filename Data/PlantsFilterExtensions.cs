﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plants.API.Models.PlantsViewModels;

namespace Plants.API.Data
{
    public static class PlantsFilterExtensions
    {
        public static IEnumerable<Plant> FilterBy(this IEnumerable<Plant> plants, FilterQueryModel filter, string currentUserId)
        {
            if (filter == null)
                return plants;

            //if (filter.BloomingTime?.Any() == true)
            //{
            //    var flags = filter.BloomingTime.ToFlags();
            //    if (flags != MonthFlags.None)
            //        plants = plants.Where(p => (p.BloomingTimes & flags) != 0);
            //}

            if (filter.CategoryId != null)
                plants = plants.Where(p => p.CategoryId == filter.CategoryId);

            if (filter.IsFrostProof != null)
                plants = plants.Where(p => p.IsFrostProof == filter.IsFrostProof);

            if (filter.IsFavorite != null && currentUserId != null)
                plants = filter.IsFavorite == true ? plants.Where(p => p.FavoritedByUsers.Any(pu => pu.UserId == currentUserId)) : plants.Where(p => p.FavoritedByUsers.All(pu => pu.UserId != currentUserId));

            if (filter.LightReq?.Any() == true)
                plants = plants.Where(p => filter.LightReq.Contains(p.LightReq));

            if (filter.NutritionReq.HasValue )
                plants = plants.Where(p => p.NutritionReq <= filter.NutritionReq);

            //if (filter.PickingTime?.Any() == true)
            //{
            //    var flags = filter.PickingTime.ToFlags();
            //    if (flags != MonthFlags.None)
            //        plants = plants.Where(p => p.PickingTimes == null || (p.PickingTimes & flags) != 0);
            //}

            if (filter.PlantingTime?.Any() == true)
            {
                var flags = filter.PlantingTime.ToFlags();
                if (flags != MonthFlags.None)
                    plants = plants.Where(p => p.PlantingTime == null || (p.PlantingTime & flags) != 0);
            }

            if (filter.WaterReq.HasValue)
                plants = plants.Where(p => p.WaterReq <= filter.WaterReq);

            return plants;
        }
    }
}
