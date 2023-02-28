using System;
using System.Collections.Generic;

namespace ShoppingListAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? PictureLink { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ListedItem> ListedItems { get; } = new List<ListedItem>();

    public virtual ICollection<Purchase> Purchases { get; } = new List<Purchase>();
}
