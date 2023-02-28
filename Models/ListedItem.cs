using System;
using System.Collections.Generic;

namespace ShoppingListAPI.Models;

public partial class ListedItem
{
    public int ListedItemId { get; set; }

    public int ProductId { get; set; }

    public int Amount { get; set; }

    public virtual Product? Product { get; set; } = null!;
}
