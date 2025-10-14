using GymManagmentDAL.Data.Contexts;
using GymManagmentDAL.Entities;
using GymManagmentDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly Dbcontext _dbcontext;

        public SessionRepository(Dbcontext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbcontext.Sessions.Include(s => s.Trainer)
                                      .Include(c => c.SessionCategory)
                                      .ToList();
        }

        public int GetCountOfBookedSlots(int sessionid)
        {
           return _dbcontext.MemberSessions.Count(x=>x.SessionId == sessionid);
        }

        public Session? GetSessionsWithTrainerAndCategory(int sessionid)
        {
            return _dbcontext.Sessions.Include(s => s.Trainer)
                                       .Include(c => c.SessionCategory)
                                       .FirstOrDefault(x=>x.Id==sessionid);
                                    
        }
    }
}
