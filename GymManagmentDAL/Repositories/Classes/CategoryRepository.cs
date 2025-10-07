using GymManagmentDAL.Data.Contexts;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Dbcontext _context;

        public CategoryRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                           .ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories
                           .FirstOrDefault(c => c.Id == id);
        }

        public int Add(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges();
        }

        public int Update(Category category)
        {
            _context.Categories.Update(category);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                return _context.SaveChanges();
            }
            return 0;
        }
    }

}
