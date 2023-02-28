using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private shoppingListContext db = new shoppingListContext();

        [HttpGet]
        [Route("")]
        public ActionResult GetAllCategories()
        {
            try
            {
                var categories = (from c in db.Categories select c).ToList();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
