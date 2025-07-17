using Amazon.Library.Models;
using eCommerce.API.Database;
using eCommerce.Library.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.API.EC
{
    public class InventoryEC
    {
        public InventoryEC() { }

        public async Task<IEnumerable<ProductDTO>> Get()
        {
            return await Task.Run(() =>
                Filebase.Current.Products.Take(100).Select(p => new ProductDTO(p))
            );
        }

        public async Task<IEnumerable<ProductDTO>> Search(string? query)
        {
            return await Task.Run(() =>
                Filebase.Current.Products.Where(p =>
                    (p?.Name != null && p.Name.ToUpper().Contains(query?.ToUpper() ?? string.Empty))
                    ||
                    (p?.Description != null && p.Description.ToUpper().Contains(query?.ToUpper() ?? string.Empty)))
                .Take(100).Select(p => new ProductDTO(p))
            );
        }

        public async Task<ProductDTO?> Delete(int id) // 2 SHOULD HIT ON CLIENT// HITS ON SWAGGER
        {
            var deletedProduct = await Task.Run(() => Filebase.Current.Delete(id));

            if (deletedProduct != null)
            {
                return new ProductDTO(deletedProduct);
            }
            else
            {
                return null;
            }
        }

        public async Task<ProductDTO> AddOrUpdate(ProductDTO p)// 2 HITS WHEN ADDED ON CLIENT 
        {
            return await Task.Run(() =>
            {
                var product = new Product(p);
                var savedProduct = Filebase.Current.AddOrUpdate(product);
                return new ProductDTO(savedProduct);
            });
        }
    }
}
