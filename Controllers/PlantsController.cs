using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;

        public PlantsController(ApplicationDbContext dbContext, CurrentUserInfo currentUserInfo, IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _currentUserInfo = currentUserInfo;
            _env = env;
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
        [ProducesResponseType(200, Type = typeof(PlantViewModel))]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            var result = _dbContext.Plants.ToViewModels(_currentUserInfo?.Id).FirstOrDefault(p => p.Id == id);
            if (result == null)
                return NotFound($"The plant with Id {id} was not found.");
            return Ok(result);
        }

        [HttpGet("api/categories")]
        public IEnumerable<CategoryViewModel> GetByCategory([FromQuery]FilterQueryModel filter)
        {
            var categoryNumbers = _dbContext.Plants.FilterBy(filter, _currentUserInfo?.Id)
                .Where(c => c.CategoryId != null)
                .Select(c => c.CategoryId)
                .ToList();

            return _dbContext.Categories.ToList().Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, Plants = categoryNumbers.Count(n => n == c.Id) });
        }

        [HttpPost("api/plants")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlant([FromBody]CreateUpdatePlantViewModel model)
        {
            var entry = _dbContext.Add(new Plant
            {
                //BloomingTimes = model.BloomingTime.ToFlags(),
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                IsFrostProof = model.IsFrostProof,
                LightReq = model.LightReq,
                Name = model.Name,
                NutritionReq = model.NutritionLevel,
                //PickingTimes = model.PickingTime?.ToFlags(),
                PlantingTime = model.PlantingTime?.ToFlags(),
                Price = model.Price,
                WaterReq = model.WaterLevel
            });
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(_env.IsDevelopment() ? ex.ToString() : ex.GetType().ToString());
            }
            return CreatedAtAction(nameof(Get), new { id = entry.Entity.Id }, entry.Entity);
        }

        [HttpPost("api/plants/favorite/{plantId}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult Favorite(int plantId)
        {
            if (_currentUserInfo?.Id == null || _dbContext.Users.All(u => u.Id != _currentUserInfo.Id))
                return Unauthorized();

            if (_dbContext.Plants.All(p => p.Id != plantId))
                return NotFound($"The plant with Id {plantId} was not found.");

            var favorited = _dbContext.FavoritePlants.FirstOrDefault(fp => fp.UserId == _currentUserInfo.Id && fp.PlantId == plantId);
            if (favorited != null)
                _dbContext.FavoritePlants.Remove(favorited);
            else
                _dbContext.FavoritePlants.Add(new FavoritePlant { UserId = _currentUserInfo.Id, PlantId = plantId });

            _dbContext.SaveChanges();
            return Ok(favorited == null);
        }

        [HttpPost("api/categories/{categoryId}/plants/{plantId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult AddPlantToCategory(int plantId, int categoryId)
        {
            var plant = _dbContext.Plants.Find(plantId);
            if (plant == null)
                return NotFound($"The plant with Id {plantId} was not found.");

            if (_dbContext.Categories.All(c => c.Id != categoryId))
                return NotFound($"The category with Id {categoryId} was not found.");

            plant.CategoryId = categoryId;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("api/category")]
        [ProducesResponseType(401)]
        public IActionResult CreateCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                return BadRequest("CategoryName cannot be empty.");

            var entry = _dbContext.Categories.Add(new Category { Name = categoryName });
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(_env.IsDevelopment() ? ex.ToString() : ex.GetType().ToString());
            }

            return CreatedAtAction(nameof(GetByCategory), new { categoryId = entry.Entity.Id }, entry.Entity);
        }

        [HttpDelete("api/categories/{categoryId}/plants/{plantId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RemovePlantFromCategory(int plantId, int categoryId)
        {
            var plant = _dbContext.Plants.Find(plantId);
            if (plant == null)
                return NotFound($"The plant with Id {plantId} was not found.");

            if (plant.CategoryId != categoryId)
                return BadRequest($"The plant with Id {plantId} was not in the category {categoryId}.");

            plant.CategoryId = null;

            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("api/category/{id}")]
        [ProducesResponseType(404)]
        public IActionResult RemoveCategory(int id)
        {
            var category = _dbContext.Find<Category>(id);
            if (category == null)
                return NotFound($"The category with Id {id} was not found.");

            _dbContext.Remove(category);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("api/plants/{id}")]
        [ProducesResponseType(404)]
        public IActionResult RemovePlant(int id)
        {
            var plant = _dbContext.Find<Plant>(id);
            if (plant == null)
                return NotFound($"The plant with Id {id} was not found.");

            _dbContext.Remove(plant);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}