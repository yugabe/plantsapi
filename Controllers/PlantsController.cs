using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plants.API.Data;
using Plants.API.Models;
using Plants.API.Models.PlantsViewModels;

namespace Plants.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CurrentUserInfo _currentUserInfo;

        public PlantsController(ApplicationDbContext dbContext, CurrentUserInfo currentUserInfo)
        {
            _dbContext = dbContext;
            _currentUserInfo = currentUserInfo;
        }

        [HttpGet("api/plants")]
        public IEnumerable<PlantViewModel> Get([FromQuery]FilterQueryModel filter)
        {
            return _dbContext.Plants.Include(p => p.FavoritedByUsers)
                .FilterBy(filter, _currentUserInfo?.Id)
                .ToList()
                .ToViewModels(_currentUserInfo?.Id);
        }

        [HttpGet("api/plants/{id}")]
        public PlantViewModel Get(int id)
        {
            return _dbContext.Plants.ToViewModels(_currentUserInfo?.Id).FirstOrDefault(p => p.Id == id);
        }

        [HttpGet("api/categories")]
        public IEnumerable<CategoryViewModel> GetByCategory([FromQuery]FilterQueryModel filter)
        {
            if (new object[] { filter?.BloomingTime, filter?.CategoryId, filter?.IsFrostProof, filter?.IsSaved, filter?.LightReqs, filter?.NutritionLevel, filter?.PickingTime, filter?.PlantingTime, filter?.WaterLevel }?.Any(p => p != null) == true)
                return _dbContext.Plants
                    .FilterBy(filter, _currentUserInfo?.Id)
                    .SelectMany(p => p.PlantCategories)
                    .GroupBy(pc => pc.Category)
                    .Select(g => new CategoryViewModel { Id = g.Key.Id, Name = g.Key.Name, Plants = g.Count() });

            return _dbContext.Categories.Include(c => c.PlantCategories).ToList().Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, Plants = c.PlantCategories.Count() });
        }

        [HttpPost("api/plants")]
        public IActionResult CreatePlant([FromBody]CreateUpdatePlantViewModel model)
        {
            var entry = _dbContext.Add(new Plant
            {
                BloomingTimes = model.BloomingTime.ToFlags(),
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                IsFrostProof = model.IsFrostProof,
                LightReqs = model.LightReqs,
                Name = model.Name,
                NutritionLevel = model.NutritionLevel,
                PickingTimes = model.PickingTime?.ToFlags(),
                PlantingTimes = model.PlantingTime?.ToFlags(),
                Price = model.Price,
                WaterLevel = model.WaterLevel
            });
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = entry.Entity.Id }, entry.Entity);
        }

        [HttpPost("api/plants/favorite/{plantId}")]
        public bool Favorite(int plantId)
        {
            var favorited = _dbContext.FavoritePlants.FirstOrDefault(fp => fp.UserId == _currentUserInfo.Id && fp.PlantId == plantId);
            if (favorited != null)
                _dbContext.FavoritePlants.Remove(favorited);
            else
                _dbContext.FavoritePlants.Add(new FavoritePlant { UserId = _currentUserInfo.Id, PlantId = plantId });
            _dbContext.SaveChanges();
            return favorited == null;
        }

        [HttpPost("api/categories/{categoryId}/plants/{plantId}")]
        public IActionResult AddPlantToCategory(int plantId, int categoryId)
        {
            _dbContext.Add(new PlantCategory { PlantId = plantId, CategoryId = categoryId });
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("api/category")]
        public IActionResult AddCategory(string categoryName)
        {
            var entry = _dbContext.Categories.Add(new Category { Name = categoryName });
            _dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetByCategory), new { categoryId = entry.Entity.Id }, entry.Entity);
        }

        [HttpDelete("api/categories/{categoryId}/plants/{plantId}")]
        public IActionResult RemovePlantFromCategory(int plantId, int categoryId)
        {
            _dbContext.RemoveRange(_dbContext.PlantCategories.Where(pc => pc.CategoryId == categoryId && pc.PlantId == plantId));
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("api/category/{id}")]
        public IActionResult RemoveCategory(int id)
        {
            _dbContext.RemoveRange(_dbContext.Categories.Where(c => c.Id == id));
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("api/plants/{id}")]
        public IActionResult RemovePlant(int id)
        {
            _dbContext.RemoveRange(_dbContext.Plants.Where(c => c.Id == id));
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}