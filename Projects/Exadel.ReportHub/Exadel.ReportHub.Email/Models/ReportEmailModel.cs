using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Email.Models;

public class ReportEmailModel
{
    public string UserName { get; set; }

    public string ReportPeriod { get; set; }

    public bool IsSuccess { get; set; }
}
