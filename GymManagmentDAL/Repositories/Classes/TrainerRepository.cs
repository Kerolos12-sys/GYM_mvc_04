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
    public class TrainerRepository : ITrainerRepository
    {
        private readonly Dbcontext  _context;

        public TrainerRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<Trainer> GetAll()
        {
            return _context.Trainers
                           .ToList();
        }

        public Trainer? GetById(int id)
        {
            return _context.Trainers
                           .FirstOrDefault(t => t.Id == id);
        }

        public int Add(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            return _context.SaveChanges();
        }

        public int Update(Trainer trainer)
        {
            _context.Trainers.Update(trainer);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                return _context.SaveChanges();
            }
            return 0;
        }
    }

}
