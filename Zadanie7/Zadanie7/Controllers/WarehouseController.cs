using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models;
using Zadanie7.Services;

namespace Zadanie7.Controllers;

[Route("/warehouse")]
[ApiController]
public class WarehouseController(IWarehouseService warehouseService): ControllerBase
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    [HttpGet]
    public IActionResult GetProducts()
    {
        var warehouseProducts = _warehouseService.GetProducts();
        return Ok(warehouseProducts);
    }
    [HttpPost]
    public IActionResult AddWarehouseProduct(WarehouseProductDTO warehouseProduct)
    {
        var createdWarehouseProduct = _warehouseService.ConvertDtoToWarehouseProduct(warehouseProduct); 
        _warehouseService.AddWarehouseProduct(createdWarehouseProduct);
     
        return StatusCode(StatusCodes.Status201Created);
    }
    [HttpPost("/withProcedure")]
    public IActionResult AddWarehouseProductWithProduct(WarehouseProductDTO warehouseProduct)
    {
        var createdWarehouseProduct = _warehouseService.ConvertDtoToWarehouseProduct(warehouseProduct); 
        _warehouseService.AddProductWarehouseWithProcedure(createdWarehouseProduct);
        return StatusCode(StatusCodes.Status201Created);
    }
    
}