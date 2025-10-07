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
    public class SessionRepository : ISessionRepository
    {
        private readonly Dbcontext _context;

        public SessionRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<Session> GetAll()
        {
            return _context.Sessions
                           .ToList();
        }

        public Session? GetById(int id)
        {
            return _context.Sessions
                           .FirstOrDefault(s => s.Id == id);
        }

        public int Add(Session session)
        {
            _context.Sessions.Add(session);
            return _context.SaveChanges();
        }

        public int Update(Session session)
        {
            _context.Sessions.Update(session);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                return _context.SaveChanges();
            }
            return 0;
        }
    }

}
