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
    public class UnitOfWork : IUnitOfWork
    {

        private readonly Dictionary<Type,object> _repositories=new();


        private readonly Dbcontext _dbcontext;
        public UnitOfWork(Dbcontext dbcontext,ISessionRepository sessionRepository) { 
        
        
            _dbcontext=dbcontext;
            SessionRepository = sessionRepository;
        
        
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType= typeof(TEntity);
            if (_repositories.TryGetValue(EntityType,out var Repo) ) { return (IGenericRepository<TEntity>)Repo; }
            var NewRepo = new GenericRepository<TEntity>(_dbcontext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
           return _dbcontext.SaveChanges();
        }
    }
}
