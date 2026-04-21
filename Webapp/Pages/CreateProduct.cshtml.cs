using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Webapp.DTO;
using OnlineShopServer.Services.Interface;

namespace Webapp.Pages
{
    public class CreateProductModel : PageModel
    {
        private readonly IAdminProductService _adminProductService;
        private readonly ICategoryService _categoryService;

        public CreateProductModel(IAdminProductService adminProductService, ICategoryService categoryService)
        {
            _adminProductService = adminProductService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ProductCreateDTO Product { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Suppliers { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("RoleName") != "Admin")
                return RedirectToPage("/Index");

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("RoleName") != "Admin")
                return RedirectToPage("/Index");

            var result = await _adminProductService.CreateProductAsync(Product);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
                await LoadDropdownsAsync();
                return Page();
            }

            return RedirectToPage("/ManageProduct");
        }

        private async Task LoadDropdownsAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            Categories = categories.Select(x => new SelectListItem
            {
                Value = x.CategoryId.ToString(),
                Text = x.CategoryName
            }).ToList();

            var suppliers = await _adminProductService.GetSuppliersAsync();
            Suppliers = suppliers.Select(x => new SelectListItem
            {
                Value = x.SupplierId.ToString(),
                Text = x.SupplierName
            }).ToList();
        }
    }
}
