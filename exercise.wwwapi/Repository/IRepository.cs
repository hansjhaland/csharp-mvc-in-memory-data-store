using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IRepository
    {
        Task<List<Product>> GetAsync(string category);
        Task<Product> AddAsync(Product model);
        Task<Product> GetByIdAsync(int id);
        Task<Product> DeleteAsync(int id);
        Task<Product> UpdateAsync(int id, Product model);
    }
}
