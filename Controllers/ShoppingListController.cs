using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListAPI.Models;

namespace ShoppingListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private shoppingListContext db = new shoppingListContext();

        [HttpGet]
        [Route("")]
        public ActionResult GetShoppingList()
        {
            try
            {
                var result = (from sl in db.ListedItems
                              join p in db.Products
                              on sl.ProductId equals p.ProductId
                              select new { sl.ListedItemId, sl.ProductId, p.ProductName, sl.Amount}).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteListedItem(int id)
        {
            ListedItem item = db.ListedItems.Find(id);

            try
            {
                if (item != null)
                {
                    db.ListedItems.Remove(item);
                    db.SaveChanges();
                    return Ok("Product " + id + ": has been removed from the shopping list.");
                }
                else
                {
                    return NotFound("Product with ID " + id + " does not exist.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        //POST /api/products
        [HttpPost]
        [Route("")]
        public ActionResult AddToShoppingList([FromBody] ListedItem item)
        {
            try
            {
                db.ListedItems.Add(item);
                db.SaveChanges();
                return Ok("New item has been added.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("purchased/{id}")]
        public ActionResult PurchasedListedItem(int id)
        {
            ListedItem item = db.ListedItems.Find(id);

            try
            {
                if (item != null)
                {
                    Purchase purchase = new Purchase();
                    purchase.ProductId = item.ProductId; 
                    purchase.Amount = item.Amount; 
                    purchase.Date = DateTime.Now;
                    db.Purchases.Add(purchase);

                    db.ListedItems.Remove(item);
                    db.SaveChanges();
                    return Ok("Product " + id + ":  has been removed from the shopping list and added to history of purchases.");
                }
                else
                {
                    return NotFound("Product with ID " + id + " does not exist.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
