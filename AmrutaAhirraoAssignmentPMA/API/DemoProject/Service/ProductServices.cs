using DemoProject.Context;
using DemoProject.Interface;
using DemoProject.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Service
{
    public class ProductServices : IProductServices
    {
        private readonly ProductDbContext _dbContext;

        public ProductServices(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public async Task AddProduct(Products product)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Products ON");

                    _dbContext.Products.Add(product);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Products OFF");

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                Console.WriteLine(ex.Message);
            }
        }


        public async Task<bool> UpdateProduct(int id, Products product)
        {

            if (product == null)
            {
                return false;
            }
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
