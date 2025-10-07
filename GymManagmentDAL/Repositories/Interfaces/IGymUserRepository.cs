using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IGymUserRepository
    {
        IEnumerable<GymUser> GetAll();
        GymUser? GetById(int id);
        int Add(GymUser gymUser);
        int Update(GymUser gymUser);
        int Delete(int id);
    }
}
