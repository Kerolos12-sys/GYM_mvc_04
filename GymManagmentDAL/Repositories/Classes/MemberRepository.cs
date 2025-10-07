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
    internal class MemberRepository : IMemberRepository
    {
        private readonly Dbcontext _context ;

        public MemberRepository(Dbcontext context) { 
        
        _context=context;
        
        }

       
        public IEnumerable<Member> GetAll()
        {
            return _context.Members.ToList();
        }

        public Member? GetById(int id)
        {
            return _context.Members.Find(id);
        }

        public int Add(Member member)
        {
            _context.Members.Add(member);
            return _context.SaveChanges();
        }

        public int Update(Member member)
        {
            _context.Members.Update(member);
            return _context.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
