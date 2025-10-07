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
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly Dbcontext _context;

        public HealthRecordRepository(Dbcontext context)
        {
            _context = context;
        }

        public IEnumerable<HealthRecord> GetAll()
        {
            return _context.HealthRecords.ToList();
        }

        public HealthRecord? GetById(int id)
        {
            return _context.HealthRecords.Find(id);
        }

        public int Add(HealthRecord healthRecord)
        {
            _context.HealthRecords.Add(healthRecord);
            return _context.SaveChanges();
        }

        public int Update(HealthRecord healthRecord)
        {
            _context.HealthRecords.Update(healthRecord);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var record = _context.HealthRecords.Find(id);
            if (record != null)
            {
                _context.HealthRecords.Remove(record);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
