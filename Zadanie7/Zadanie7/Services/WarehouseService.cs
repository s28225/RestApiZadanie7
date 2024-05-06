using Microsoft.AspNetCore.Components.Sections;
using Zadanie7.Models;

namespace Zadanie7.Services;

public class WarehouseService(IWarehouseRepository warehouseRepository) : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseProductRepository = warehouseRepository;

    public IEnumerable<WarehouseProduct> GetProducts()
    {
        return _warehouseProductRepository.GetAll();
    }

    public int AddWarehouseProduct(WarehouseProduct warehouseProduct)
    {
        int orderInWarehouse = _warehouseProductRepository.getIsOrderInProductWarehouse(warehouseProduct.IdOrder);
        if (orderInWarehouse==0)
        {
            return _warehouseProductRepository.AddWarehouseProduct(warehouseProduct);
        }
        else
        {
            throw new Exception("Already exists");
        }
    }

    public async Task<int> AddProductWarehouseWithProcedure(WarehouseProduct warehouseProduct)
    {
        return await _warehouseProductRepository.AddProductWarehouseWithProcedure(warehouseProduct);
    }

    public WarehouseProduct ConvertDtoToWarehouseProduct(WarehouseProductDTO dto)
    {
        List<Object> list = _warehouseProductRepository.getDataFromOrder(dto);
        decimal tempPrice = _warehouseProductRepository.getPriceFromProduct(dto.IdProduct);
        bool warehouse = _warehouseProductRepository.getIsWarehouseId(dto.IdWarehouse);
        if (list != null && dto.Amount > 0 && warehouse  && (DateTime)list[1]<dto.CreatedAt)
        {
            int tempIdOrder = (int)list[0];
            WarehouseProduct warehouseProduct = new WarehouseProduct
            {
                Id = -1,
                IdProduct = dto.IdProduct,
                IdWarehouse = dto.IdWarehouse,
                IdOrder = tempIdOrder,
                Amount = dto.Amount,
                Price = tempPrice * dto.Amount,
                CreatedAt = dto.CreatedAt,
            };
            return warehouseProduct;
        }
        else
        {
            throw new Exception("Not correct data");
        }
    }
}