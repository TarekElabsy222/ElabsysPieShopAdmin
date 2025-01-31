namespace ElabsysPieShopAdmin.Models.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<Category?> GetCategoryByIdAsync(int id);
        public Task<int> AddCategoryAsync(Category category);
        public Task<int> UpdateCategoryAsync(Category category);
        public Task<int> DeleteCategoryAsync(int id);
        Task<int> UpdateCategoryNamesAsync(List<Category> categories);

    }
}
