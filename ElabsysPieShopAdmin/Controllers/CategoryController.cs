using ElabsysPieShopAdmin.Models;
using ElabsysPieShopAdmin.Models.Repositories;
using ElabsysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ElabsysPieShopAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepositories;

        public CategoryController(ICategoryRepository categoryRepositories)
        {
            _categoryRepositories = categoryRepositories;
        }

        public async Task<IActionResult> Index()
        {
            CategoryListViewModel model = new CategoryListViewModel()
            {
                Categories = (await _categoryRepositories.GetAllCategoriesAsync()).ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var result = await _categoryRepositories.GetCategoryByIdAsync(id.Value);
            return View(result);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([Bind("Name,Description,DateAdded")]Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryRepositories.AddCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }
            }catch (Exception ex)
            {
                ModelState.AddModelError("",$"Adding The Category Failed, Please try again: {ex.Message}");
            }
            return View(category);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var selectCategory = await _categoryRepositories.GetCategoryByIdAsync(id);
            return View(selectCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryRepositories.UpdateCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }
            }catch (Exception ex)
            {
                ModelState.AddModelError("", $"Adding The Category Failed, Please try again: {ex.Message}"); 
            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var selectedCategory = await _categoryRepositories.GetCategoryByIdAsync(id);

            return View(selectedCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? CategoryId)
        {
            if (CategoryId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the category failed, invalid ID!";
                return View();
            }

            try
            {
                await _categoryRepositories.DeleteCategoryAsync(CategoryId.Value);
                TempData["CategoryDeleted"] = "Category deleted successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the category failed, please try again! Error: {ex.Message}";
            }

            var selectedCategory = await _categoryRepositories.GetCategoryByIdAsync(CategoryId.Value);
            return View(selectedCategory);
        }
        public async Task<IActionResult> AllEdit()
        {
            List<CategoryAllEditViewModel> categoryAllEditViewModels = new List<CategoryAllEditViewModel>();

            var allCategories = await _categoryRepositories.GetAllCategoriesAsync();
            foreach (var category in allCategories)
            {
                categoryAllEditViewModels.Add(new CategoryAllEditViewModel
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                });
            }

            return View(categoryAllEditViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> AllEdit(List<CategoryAllEditViewModel> categoryAllEditViewModels)
        {
            List<Category> categories = new List<Category>();

            foreach (var categoryAllEditViewModel in categoryAllEditViewModels)
            {
                categories.Add(new Category() { CategoryId = categoryAllEditViewModel.CategoryId, Name = categoryAllEditViewModel.Name });
            }

            await _categoryRepositories.UpdateCategoryNamesAsync(categories);

            return RedirectToAction(nameof(Index));
        }

    }

    }
