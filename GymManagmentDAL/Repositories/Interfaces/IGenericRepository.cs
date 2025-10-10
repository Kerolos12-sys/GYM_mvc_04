using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {

        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>? condition=null);
        int Add  (TEntity entity);
        int Update (TEntity entity);
        int Delete (TEntity entity);



    }
}
