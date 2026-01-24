namespace Identity.Identity.Models;

using System;
using Sport.Common.Core;
using Microsoft.AspNetCore.Identity;

public class RoleClaim : IdentityRoleClaim<Guid>, IVersion
{
    public long Version { get; set; }
}