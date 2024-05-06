using Zadanie7.Models;

namespace Zadanie7.Services;

public interface IWarehouseService
{
    public IEnumerable<WarehouseProduct> GetProducts();
    public int AddWarehouseProduct(WarehouseProduct warehouseProduct);

    public WarehouseProduct ConvertDtoToWarehouseProduct(WarehouseProductDTO dto);
    public Task<int> AddProductWarehouseWithProcedure(WarehouseProduct warehouseProduct);

}