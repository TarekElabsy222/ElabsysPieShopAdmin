using ElabsysPieShopAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElabsysPieShopAdmin.ViewModels
{
    public class PieAddViewModel
    {
        public Pie Pie { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; } = default!;
    }
}
