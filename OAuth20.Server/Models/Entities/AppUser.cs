/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace OAuth20.Server.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public IEnumerable<UserClaim> UserClaims = new List<UserClaim>();

        public string Password { get; set; }
    }
}
