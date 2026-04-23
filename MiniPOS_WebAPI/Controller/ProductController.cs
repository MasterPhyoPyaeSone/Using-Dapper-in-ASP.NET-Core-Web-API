using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotNetTrainingBatch2.DapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // IConfiguration ကို Inject လုပ်ပြီး Connection String ကို ယူသုံးပါမယ်။
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
                {
                    // Query ရေးတဲ့အခါ Column တွေကို အတိအကျ ခေါ်ခြင်းက အမှားနည်းပါတယ်
                    string sql = "SELECT ProductId, ProductCode, ProductName, Price, DeleteFlag FROM Tbl_Product WHERE DeleteFlag = 0";

                    var list = db.Query<ProductModel>(sql).ToList();

                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                // Error ဘာကြောင့်တက်လဲဆိုတာ အတိအကျ သိနိုင်ဖို့
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                // QueryFirstOrDefault က record တစ်ကြောင်းပဲယူချင်တဲ့အခါသုံးပါတယ်။ Data မရှိရင် null ပြန်ပေးပါတယ်။
                // Parameter ကို anonymous object အနေနဲ့ လွယ်လွယ်ကူကူ ထည့်လို့ရပါတယ်။
                var item = db.QueryFirstOrDefault<ProductModel>("select * from Tbl_Product where ProductId = @ProductId;", new
                {
                    ProductId = id
                });

                if (item is null)
                {
                    return NotFound();
                }
                return Ok(item);
            }
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductRequestModel requestModel)
        {
            string query = @"INSERT INTO [dbo].[Tbl_Product]
           ([ProductCode]
           ,[ProductName]
           ,[Price]
           ,[DeleteFlag])
     VALUES
           (@ProductCode
           ,@ProductName
           ,@Price
           ,0)";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                // Execute method က INSERT, UPDATE, DELETE query တွေအတွက်သုံးပြီး affected rows အရေအတွက်ကို return ပြန်ပေးပါတယ်။
                int result = db.Execute(query, requestModel);
                return Ok(result > 0 ? "Saving Successful." : "Saving Failed.");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateProduct(int id, ProductRequestModel requestModel)
        {
            string query = @"UPDATE [dbo].[Tbl_Product]
   SET [ProductCode] = @ProductCode
      ,[ProductName] = @ProductName
      ,[Price] = @Price
 WHERE ProductId = @ProductId";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                // Anonymous object အသစ်တစ်ခုထဲမှာ requestModel နဲ့ id ကိုပေါင်းပြီး parameter အဖြစ် ထည့်ပေးလိုက်ပါတယ်။
                int result = db.Execute(query, new
                {
                    requestModel.ProductCode,
                    requestModel.ProductName,
                    requestModel.Price,
                    ProductId = id
                });
                return Ok(result > 0 ? "Updating Successful." : "Updating Failed.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            string query = @"UPDATE [dbo].[Tbl_Product]
   SET DeleteFlag = 1
 WHERE ProductId = @ProductId";
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                int result = db.Execute(query, new { ProductId = id });
                return Ok(result > 0 ? "Deleting Successful." : "Deleting Failed.");
            }
        }
    }
}