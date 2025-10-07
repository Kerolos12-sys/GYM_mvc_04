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
    public class PlanRepository : IPlanRepository
    {
        private readonly Dbcontext _context;

        public PlanRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _context.Plans
                           .ToList();
        }

        public Plan? GetById(int id)
        {
            return _context.Plans
                           .FirstOrDefault(p => p.Id == id);
        }

        public int Add(Plan plan)
        {
            _context.Plans.Add(plan);
            return _context.SaveChanges();
        }

        public int Update(Plan plan)
        {
            _context.Plans.Update(plan);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var plan = _context.Plans.Find(id);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
                return _context.SaveChanges();
            }
            return 0;
        }
    }

}
