using Amazon.Library.Models;
using eCommerce.API.EC;
using eCommerce.Library.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IEnumerable<ProductDTO>> Get()
        {
            return await new InventoryEC().Get();
        }

        [HttpPost("Search")]
        public async Task<IEnumerable<ProductDTO>> Get(Query query)
        {
            return await new InventoryEC().Search(query.QueryString);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Received DELETE request for product ID: {id}");

            var deletedProduct = await new InventoryEC().Delete(id);

            if (deletedProduct != null)
            {
                _logger.LogInformation($"Product ID {id} deleted successfully.");
                return Ok(deletedProduct);
            }
            else
            {
                _logger.LogWarning($"Product ID {id} not found.");
                return NotFound($"Product with ID {id} not found.");
            }
        }

        [HttpPost()]
        public async Task<ProductDTO> AddOrUpdate([FromBody] ProductDTO p) //  1 HITS WHEN ADDED ON CLIENT
        {
            return await new InventoryEC().AddOrUpdate(p);
        }
    }
}
