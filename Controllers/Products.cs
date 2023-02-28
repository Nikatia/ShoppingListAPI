using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Products : ControllerBase
    {
        private shoppingListContext db = new shoppingListContext();

        [HttpGet]
        [Route("{catId}")]
        public ActionResult GetProducts(int catId)
        {
            try
            {
                var result = (from p in db.Products where p.CategoryId == catId select p).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
