using GymManagmentBLL.ViewModels.MemberViewModels;
using GymManagmentBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Interfaces
{
    public interface ITrainerervice
    {

        //1
        IEnumerable<TrainerViewModel> GetAllTrainers();
        //2
        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        //3
        TrainerViewModel GetTrainerDetails(int TrainerId);
        //4
        TrainerToUpdateViewModel? GetMemberToUpdate(int TrainerID);
        //5
        bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel UpdatedTrainer);
        //6
        bool RemoveTrainer(int MemberId);
    }
}
