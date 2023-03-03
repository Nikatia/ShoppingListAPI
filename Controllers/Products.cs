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

        [HttpGet]
        [Route("productExists/{productName}")]
        public ActionResult CheckProduct(string productName)
        {
            try
            {
                var exists = db.Products.Any(p => p.ProductName== productName);

                if (exists)
                {
                    var existingProduct = (from p in db.Products where p.ProductName == productName select p).FirstOrDefault();
                    return Ok(existingProduct.CategoryId);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult PostProduct([FromBody] Product product) 
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return Ok("New item has been added.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
