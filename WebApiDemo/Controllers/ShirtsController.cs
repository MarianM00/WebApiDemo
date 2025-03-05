using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Data;
using WebApiDemo.Filters.ActionFilters;
using WebApiDemo.Filters.ExceptionFilters;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShirtsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ShirtsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetShirts()
        {
            return Ok(_dbContext.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult GetShirtById(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtAttribute))]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            _dbContext.Shirts.Add(shirt);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetShirtById), new { id = shirt.ShirtId }, shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionFilterAttribute))]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Price = shirt.Price;

            _dbContext.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;
            _dbContext.Shirts.Remove(shirtToDelete);
            _dbContext.SaveChanges();
            return Ok(shirtToDelete);
        }
    }
}