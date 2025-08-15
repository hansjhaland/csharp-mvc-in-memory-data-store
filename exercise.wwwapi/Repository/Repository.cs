using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace exercise.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DataContext _db;
        public Repository(DataContext db) {  _db = db; }

        public async Task<Product> AddAsync(Product entity)
        {
            await _db.Products.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var target = await _db.Products.FindAsync(id);
            if (target is null) return null;
            _db.Products.Remove(target);
            await _db.SaveChangesAsync();
            return target;
        }

        public async Task<List<Product>> GetAsync(string category)
        {
            if (category != "")
            {
                return await _db.Products.Where(entity => entity.Category.ToLower() == category.ToLower()).ToListAsync();
            }
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<Product> UpdateAsync(int id, Product entity)
        {
            var target = await _db.Products.FindAsync(id);
            target.Name = entity.Name;
            target.Category = entity.Category;
            target.Price = entity.Price;

            await _db.SaveChangesAsync();

            return target;
        }
    }
}
