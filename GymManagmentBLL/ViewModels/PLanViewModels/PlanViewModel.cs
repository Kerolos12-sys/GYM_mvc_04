using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.PLanViewModels
{
    public class PlanViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }=null!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        

       
    }
}
