using GymManagmentDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    [NotMapped]
    public class GymUser : BaseEntity
    {
        public string Name { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
    }

    [Owned]
    public class Address 
    { 
    
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!; 
    
    
    
    
    }
}
