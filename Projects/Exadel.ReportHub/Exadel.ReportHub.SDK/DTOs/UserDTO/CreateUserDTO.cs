using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.SDK.DTOs.UserDTO;

public class CreateUserDTO
{
    public string Email { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }
}
