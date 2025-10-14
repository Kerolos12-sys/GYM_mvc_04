using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface ISessionRepository :IGenericRepository<Session>
    {

        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        int GetCountOfBookedSlots(int sessionid);

        Session? GetSessionsWithTrainerAndCategory(int sessionid);
    }
}
