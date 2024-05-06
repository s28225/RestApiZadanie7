using Zadanie7.Models;

namespace Zadanie7.Services;

public interface IWarehouseRepository
{
    int AddWarehouseProduct(WarehouseProduct warehouseProduct);
    IEnumerable<WarehouseProduct> GetAll();
    public List<Object> getDataFromOrder(WarehouseProductDTO dto);
    public decimal getPriceFromProduct(int product);
    public bool getIsWarehouseId(int warehouseId);

    public int getIsOrderInProductWarehouse(int oreder);
    public void updateOrder(int order, DateTime time);
    public Task<int> AddProductWarehouseWithProcedure(WarehouseProduct warehouseProduct);

}