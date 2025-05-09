﻿using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.User;

public class UserDTO
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string FullName { get; set; }

    public bool IsActive { get; set; }
}
