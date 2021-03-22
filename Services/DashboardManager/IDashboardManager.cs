using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Services.DashboardManager
{
    public interface IDashboardManager
    {
        Task<string> GetDenumireClient(string id);
        Task<IList> GetProfitPierdere(string id, string an);

    }
}
