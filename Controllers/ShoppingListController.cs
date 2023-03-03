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
        [Route("delete/{id}")]
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

        //POST /api/shoppingList
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

        [HttpPut]
        [Route("edit/{id}")]
        public ActionResult EditShoppingListItem(int id, [FromBody] ListedItem item)
        {
            try
            {
                ListedItem listedItem = db.ListedItems.Find(id);
                if (listedItem != null)
                {
                    listedItem.ListedItemId = id;
                    listedItem.ProductId = item.ProductId;
                    listedItem.Amount = item.Amount;

                    db.SaveChanges();
                    return Ok("Changes for item with ID " + id + " have been saved");
                }
                else
                {
                    return NotFound("Item with ID " + id + " does not exist.");
                }
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
                    return NotFound("Product with ID " + id + " does not exist in the Shopping List.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("")]
        public ActionResult DeleteAllListedItems()
        {
            try
            {
                db.ListedItems.RemoveRange(db.ListedItems);
                db.SaveChanges();
                return Ok("All items in shopping list have been removed");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("check/{id}")]
        public ActionResult CheckIfProductIsOnList(int id)
        {
            var result = (from sl in db.ListedItems where sl.ProductId == id select new { sl.ListedItemId, sl.ProductId, sl.Amount }).ToList();
            if (result.Count == 0) 
            {
                return Ok("Does not exist");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
