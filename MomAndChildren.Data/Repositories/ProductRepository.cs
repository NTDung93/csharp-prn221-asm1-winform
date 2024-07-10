using Microsoft.EntityFrameworkCore;
using MomAndChildren.Data.Base;
using MomAndChildren.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndChildren.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository() { }
        public ProductRepository(Net1710_221_3_MomAndChildrenContext context) => _context = context;
        public async Task<List<Product>> getProductsAsync()
        {
            //List<Product> products = _context.Products.Include(x => x.Brand).Include(x=>x.Category).ToList();
            //return products;
            return await _context.Products.Include(x => x.Brand).Include(x => x.Category).ToListAsync();
        }
        public Product getProductByIdAsync(int id)
        {
            Product product = _context.Products.Include(x => x.Brand).Include(x => x.Category).FirstOrDefault(x => x.ProductId == id);
            return product;
        }
    }
}
