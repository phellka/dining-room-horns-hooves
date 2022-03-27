using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiningRoomContracts.ViewModels;

namespace DiningRoomContracts.BusinessLogicsContracts
{
    public interface IReportLogic
    {
        List<ReportLunchesPCView> GetLunchesPCView(DateTime after, DateTime before);
    }
}
