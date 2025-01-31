
using ElabsysPieShopAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace ElabsysPieShopAdmin.Models.Repositories
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext _context;

        public PieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddPieAsync(Pie pie)
        {
             _context.Pies.Add(pie);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeletePieAsync(int id)
        {
            var pieToDelete = await _context.Pies.FirstOrDefaultAsync(p=>p.PieId == id);
            if (pieToDelete != null)
            {
                _context.Pies.Remove(pieToDelete);
                return await _context.SaveChangesAsync();
            }else
            {
                throw new ArgumentException($"The pie to delete can't be found");
            }
            

        }

        public async Task<IEnumerable<Pie>> GetAllPiesAsync()
         => await _context.Pies.OrderBy(p=>p.PieId).ToListAsync();

        public async Task<Pie?> GetPieByIdAsync(int id)
        => await _context.Pies.Include(i => i.Ingredients).Include(c => c.Category).OrderBy(p => p.PieId).FirstOrDefaultAsync(p=>p.PieId == id);

        public async Task<int> GetAllPiesCountAsync()
        {
            IQueryable<Pie> allPies = from p in _context.Pies
                                      select p;
            var count = await allPies.CountAsync();
            return count;
        }

        public async Task<IEnumerable<Pie>> GetPiesPagedAsync(int? pageNumber, int pageSize)
        {
            IQueryable<Pie> pies = from p in _context.Pies
                                   select p;

            pageNumber ??= 1;

            pies = pies.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            return await pies.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pie>> GetPiesSortedAndPagedAsync(string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<Pie> allPies = from p in _context.Pies
                                      select p;
            IQueryable<Pie> pies;

            switch (sortBy)
            {
                case "name_desc":
                    pies = allPies.OrderByDescending(p => p.Name);
                    break;
                case "name":
                    pies = allPies.OrderBy(p => p.Name);
                    break;
                case "id_desc":
                    pies = allPies.OrderByDescending(p => p.PieId);
                    break;
                case "id":
                    pies = allPies.OrderBy(p => p.PieId);
                    break;
                case "price_desc":
                    pies = allPies.OrderByDescending(p => p.Price);
                    break;
                case "price":
                    pies = allPies.OrderBy(p => p.Price);
                    break;
                default:
                    pies = allPies.OrderBy(p => p.PieId);
                    break;
            }

            pageNumber ??= 1;

            pies = pies.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);

            return await pies.AsNoTracking().ToListAsync(); ;
        }

        public async Task<int> UpdatePieAsync(Pie pie)
        {
            var pieToUpdate = await _context.Pies.FirstOrDefaultAsync(p=>p.PieId==pie.PieId);
            if (pieToUpdate != null)
            {
                pieToUpdate.CategoryId = pie.CategoryId;
                pieToUpdate.ShortDescription = pie.ShortDescription;
                pieToUpdate.LongDescription = pie.LongDescription;
                pieToUpdate.Price = pie.Price;
                pieToUpdate.AllergyInformation = pie.AllergyInformation;
                pieToUpdate.ImageThumbnailUrl = pie.ImageThumbnailUrl;
                pieToUpdate.ImageUrl = pie.ImageUrl;
                pieToUpdate.InStock = pie.InStock;
                pieToUpdate.IsPieOfTheWeek = pie.IsPieOfTheWeek;
                pieToUpdate.Name = pie.Name;
                _context.Pies.Update(pieToUpdate);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"The pie to update can't be found.");
            }
        }
        public async Task<IEnumerable<Pie>> SearchPies(string searchQuery, int? categoryId)
        {
            var pies = from p in _context.Pies
                       select p;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                pies = pies.Where(s => s.Name.Contains(searchQuery) || s.ShortDescription.Contains(searchQuery) || s.LongDescription.Contains(searchQuery));
            }

            if (categoryId != null)
            {
                pies = pies.Where(s => s.CategoryId == categoryId);
            }

            return await pies.ToListAsync();
        }
    }
}
