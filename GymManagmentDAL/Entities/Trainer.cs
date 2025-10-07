using GymManagmentDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Trainer :GymUser
    {
        //hire date is equal to createdat of baseEntity
        public Specialties Specialties { get; set; }


        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
