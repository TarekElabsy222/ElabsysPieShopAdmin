
using ElabsysPieShopAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace ElabsysPieShopAdmin.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            bool categoryWithSameNameExist = await _context.Categories.AnyAsync(c => c.Name == category.Name);
            if (categoryWithSameNameExist)
            {
                throw new Exception("A category with same name already exists");
            }
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var piesInCategory = _context.Pies.Any(p => p.CategoryId == id);

            if (piesInCategory)
            {
                throw new Exception("Pies exist in this category. Delete all pies in this category before deleting the category.");
            }

            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (categoryToDelete != null)
            {
                _context.Categories.Remove(categoryToDelete);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The category to delete can't be found.");
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
            =>  await _context.Categories.AsNoTracking().OrderBy(c=>c.CategoryId).ToListAsync();

        public async Task<Category?> GetCategoryByIdAsync(int id)
           => await _context.Categories.AsNoTracking().Include(p=>p.Pies).FirstOrDefaultAsync(c=>c.CategoryId == id);

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            bool categoryWithSameNameExist = await _context.Categories.AnyAsync(c => c.Name == category.Name && c.CategoryId != category.CategoryId);
            if (categoryWithSameNameExist)
            {
                throw new Exception("A category with same name already exists");
            }
            var categoryUpdated = await _context.Categories.FirstOrDefaultAsync(c=>c.CategoryId == category.CategoryId);
            if (categoryUpdated != null)
            {
                categoryUpdated.Name = category.Name ;
                categoryUpdated.Description =  category.Description;
                _context.Categories.Update(categoryUpdated);
                return await _context.SaveChangesAsync();
            }else
            {
                throw new Exception($"category to update Can't be found");
            }

        }
        public async Task<int> UpdateCategoryNamesAsync(List<Category> categories)
        {
            foreach (var category in categories)
            {
                var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

                if (categoryToUpdate != null)
                {
                    categoryToUpdate.Name = category.Name;

                    _context.Categories.Update(categoryToUpdate);
                }
            }

            return await _context.SaveChangesAsync();
        }
    }
}
