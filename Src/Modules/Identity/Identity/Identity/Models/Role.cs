namespace Identity.Identity.Models;

using System;
using Microsoft.AspNetCore.Identity;
using Sport.Common.Core;

public class Role : IdentityRole<Guid>, IVersion
{
    public long Version { get; set; }
}