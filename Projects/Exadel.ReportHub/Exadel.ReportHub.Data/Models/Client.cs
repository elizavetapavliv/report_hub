using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Data.Models;

public class Client : IDocument
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IList<Guid> CustomerIds { get; set; }
}
