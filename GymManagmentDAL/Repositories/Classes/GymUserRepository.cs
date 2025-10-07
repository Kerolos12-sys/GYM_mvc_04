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
    public class GymUserRepository : IGymUserRepository
    {
        private readonly Dbcontext _context;

        public GymUserRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<GymUser> GetAll()
        {
            return _context.GymUsers.ToList();
        }

        public GymUser? GetById(int id)
        {
            return _context.GymUsers.Find(id);
        }

        public int Add(GymUser gymUser)
        {
            _context.GymUsers.Add(gymUser);
            return _context.SaveChanges();
        }

        public int Update(GymUser gymUser)
        {
            _context.GymUsers.Update(gymUser);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var gymUser = _context.GymUsers.Find(id);
            if (gymUser != null)
            {
                _context.GymUsers.Remove(gymUser);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
