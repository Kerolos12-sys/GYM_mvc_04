using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymManagmentBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Services.Interfaces
{
    public interface ISessionService
    {

        IEnumerable<SessionViewModel> GetAllSession();

        SessionViewModel? GetSessionById(int sessionId);

        bool CreateSession(CreateSessionViewModel createdSession);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId);

        bool RemoveSession(int sessionId);

    }
}
