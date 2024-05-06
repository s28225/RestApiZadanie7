using System.Data;
using System.Data.SqlClient;
using Zadanie7.Models;

namespace Zadanie7.Services;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public int AddWarehouseProduct(WarehouseProduct warehouseProduct)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Product_Warehouse (IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt)" +
                          " Values (@IdProduct, @IdWarehouse, @IdOrder, @Amount, @Price, @CreatedAt)";
        cmd.Parameters.AddWithValue("@IdProduct", warehouseProduct.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", warehouseProduct.IdWarehouse);
        cmd.Parameters.AddWithValue("@IdOrder", warehouseProduct.IdOrder);
        cmd.Parameters.AddWithValue("@Amount", warehouseProduct.Amount);
        cmd.Parameters.AddWithValue("@Price", warehouseProduct.Price);
        cmd.Parameters.AddWithValue("@CreatedAt", warehouseProduct.CreatedAt); 
        cmd.ExecuteNonQuery();
        updateOrder(warehouseProduct.IdOrder,warehouseProduct.CreatedAt);
        
        return getIsOrderInProductWarehouse(warehouseProduct.IdOrder);
    }
    public void updateOrder(int orderID, DateTime time)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE \"Order\" SET FulfilledAt = @CreatedAt WHERE IdOrder=@IdOrder";
        cmd.Parameters.AddWithValue("@CreatedAt", time);
        cmd.Parameters.AddWithValue("@IdOrder", orderID);
        var dr = cmd.ExecuteNonQuery();
    }

    public IEnumerable<WarehouseProduct> GetAll()
    {
        var warehouseProducts = new List<WarehouseProduct>();
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "Select IdProductWarehouse, IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt from Product_Warehouse";
        var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var grade = new WarehouseProduct
            {
                Id = (int)dr["IdProductWarehouse"],
                IdProduct = (int)dr["IdProduct"],
                IdWarehouse = (int)dr["IdWarehouse"],
                IdOrder =(int)dr["IdOrder"],
                Amount = (int)dr["Amount"],
                Price = dr.GetDecimal(dr.GetOrdinal("Price")),
                CreatedAt = (DateTime)dr["CreatedAt"]
            };
            warehouseProducts.Add(grade);
        }

        return warehouseProducts;
    }

    public List<Object> getDataFromOrder(WarehouseProductDTO dto)
    {
        var list = new List<Object>();
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "Select IdOrder, CreatedAt from \"Order\" where IdProduct = @idProduct and Amount = @amount and FulfilledAt = null";
        cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        cmd.Parameters.AddWithValue("@Amount", dto.Amount);

        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        var grade = new 
        {
            IdOrder =(int)dr["IdOrder"],
            CreatedAt = (DateTime)dr["CreatedAt"],
        };
        list.Add(grade.IdOrder);
        list.Add(grade.CreatedAt);

        return list;
    }

    public decimal getPriceFromProduct(int product)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "Select Price from Product where IdProduct = @idProduct";
        cmd.Parameters.AddWithValue("@IdProduct", product);

        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return 0;
        var grade = dr.GetDecimal(dr.GetOrdinal("Price")); 
        

        return grade;
    }

    public bool getIsWarehouseId(int warehouseId)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "Select IdWarehouse from Warehouse where IdWarehouse = @idWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", warehouseId);

        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return false;
        
        var grade = (int)dr["IdWarehouse"];

        return grade>0?true:false;
    }

    public int getIsOrderInProductWarehouse(int orderId)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand();
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "Select IdOrder from Product_Warehouse where IdOrder = @idOrder";
        cmd.Parameters.AddWithValue("@IdOrder", orderId);

        var dr = cmd.ExecuteReader();
        if (!dr.Read()) return 0;
        
        var grade = (int)dr["IdOrder"];

        return grade;
    }
    public async Task<int> AddProductWarehouseWithProcedure(WarehouseProduct warehouseProduct)
    {
        int returnValue = -1;
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var cmd = new SqlCommand("AddProductToWarehouse", con)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@idProduct", warehouseProduct.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", warehouseProduct.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", warehouseProduct.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", warehouseProduct.CreatedAt);
        await con.OpenAsync();
        using (var sqlDataReader = await cmd.ExecuteReaderAsync())
        {
            while (await sqlDataReader.ReadAsync())
            {
                returnValue = (int)sqlDataReader["NewId"];
            }
        }

        return returnValue;
    }
    
}