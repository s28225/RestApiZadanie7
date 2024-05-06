using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Zadanie7.Models;

public class WarehouseProduct
{
    public int Id { get; set; }
    [Required] public int IdProduct { get; set; }
    [Required] public int IdWarehouse { get; set; }
    public int IdOrder { get; set; }
    [Required] public int Amount { get; set; }
    public decimal Price { get; set; }
    [Required] public DateTime CreatedAt { get; set; }

    public WarehouseProduct(int id, int idProduct, int idWarehouse, int idOrder, int amount, decimal price, DateTime createdAt)
    {
        Id = id;
        IdProduct = idProduct;
        IdWarehouse = idWarehouse;
        IdOrder = idOrder;
        Amount = amount;
        Price = price;
        CreatedAt = createdAt;
    }

    public WarehouseProduct()
    {
    }

}