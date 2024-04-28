using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthService.Models;
using AuthService.Data;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public CategoriesController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Получение всех категорий
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _dbContext.Categories.Find(_ => true).ToListAsync();
            return Ok(categories);
        }

        // Получение категории по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await _dbContext.Categories.Find(c => c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // Создание категории
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            await _dbContext.Categories.InsertOneAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // Обновление категории
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category updatedCategory)
        {
            var updateResult = await _dbContext.Categories.ReplaceOneAsync(c => c.Id == id, updatedCategory);

            if (updateResult.ModifiedCount == 0)
            {
                return NotFound(new { message = "Category not found or no change made." });
            }

            updatedCategory.UpdatedAt = DateTime.UtcNow; // Ensure updatedAt is set properly
            return Ok(updatedCategory);
        }

        // Удаление категории
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            // Проверяем роль - admin
            if (userRole != "admin")
            {
                return Forbid(); 
            }
            var deleteResult = await _dbContext.Categories.DeleteOneAsync(c => c.Id == id);

            if (deleteResult.DeletedCount == 0)
            {
                return NotFound(new { message = "Category not found." });
            }

            return NoContent();
        }
    }
}
