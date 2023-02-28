using System;
using System.Collections.Generic;

namespace ShoppingListAPI.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public DateTime Date { get; set; }

    public int Amount { get; set; }

    public virtual Product? Product { get; set; } = null!;
}
