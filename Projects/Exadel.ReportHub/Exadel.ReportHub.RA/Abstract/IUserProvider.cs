using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.RA.Abstract;

public interface IUserProvider
{
    Guid GetUserID();
}
