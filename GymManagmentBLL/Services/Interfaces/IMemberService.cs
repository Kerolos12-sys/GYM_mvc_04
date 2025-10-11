using GymManagmentBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Interfaces
{
    public interface IMemberService
    {
        //1
        IEnumerable<MemberViewModel> GetAllMembers();
        //2
        bool CreateMember(CreatMemberViewModel creatMember);

        //3
        MemberViewModel GetMemberDetails(int MemberId);

        //4
        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);

        //5
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberID);

        //6
        bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel UpdatedMember);

        //7
        bool RemoveMember(int MemberId);

    }
}
