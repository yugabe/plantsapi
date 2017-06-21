using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plants.API.Data;

namespace Plants.Importer
{
    public static class Importer
    {
        public static IEnumerable<Plant> Import(string connectionString, DirectoryInfo baseDirectory)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(connectionString);
            var options = builder.Options;
            using (var ctx = new ApplicationDbContext(options))
            {
                foreach (var categoryModel in JsonConvert.DeserializeObject<CategoryModel[]>(File.ReadAllText(baseDirectory.EnumerateFiles("categories.json").First().FullName)))
                {
                    var plantsFile = baseDirectory.EnumerateFiles($"plants_cat_{categoryModel.Id}.json").FirstOrDefault();
                    ctx.Categories.Add(new Category
                    {
                        Name = categoryModel.Name,
                        Plants = plantsFile?.Exists == true ? JsonConvert.DeserializeObject<PlantModel[]>(File.ReadAllText(plantsFile.FullName)).Select(p => new Plant
                        {
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            IsFrostProof = p.IsFrostProof,
                            LightReq = p.LightReq,
                            Name = p.Name,
                            NutritionReq = p.NutritionReq,
                            PlantingTime = p.PlantingTime.ToFlags(),
                            Price = p.Price,
                            WaterReq = p.WaterReq
                        }).ToList() : null
                    });
                }
                ctx.SaveChanges();
                return ctx.Plants.ToList();
            }
        }
    }
}