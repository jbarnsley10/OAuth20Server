/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;

namespace OAuth20.Server.Models
{
    public class ClientStore
    {
        public IEnumerable<Client> Clients = new[]
        {
            new Client
            {
                ClientName = "localOAuth2",
                ClientId = "21ac06dd-7306-4f7d-ab1e-e168814f2c12",
                ClientSecret = "AabKL~juBGtf6k9f47gHBde5hfs87kjPQj32hdgh",
                AllowedScopes = new[]{ "openid", "profile", "21ac06dd-7306-4f7d-ab1e-e168814f2c12" },
                GrantType = GrantTypes.Code,
                IsActive = true,
                ClientUri = "https://localhost:7275",
                RedirectUri = "https://localhost:7275/signin-oidc",
                UsePkce = true,
            }
        };
    }
}
