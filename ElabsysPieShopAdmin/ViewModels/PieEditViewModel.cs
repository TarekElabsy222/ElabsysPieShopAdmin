using ElabsysPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElabsysPieShopAdmin.ViewModels
{
    public class PieEditViewModel
    {
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;
        public Pie Pie { get; set; }
    }
}
