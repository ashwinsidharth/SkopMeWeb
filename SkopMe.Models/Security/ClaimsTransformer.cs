﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Security;
using Thinktecture.IdentityModel;

namespace SkopMe.Core.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        /// <summary>
        /// Authentication method
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="incomingPrincipal"></param>
        /// <returns></returns>
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }
            
            return CreatePrincipal(incomingPrincipal);
        }

        /// <summary>
        /// Create Claims 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        private ClaimsPrincipal CreatePrincipal(ClaimsPrincipal principal)
        {
            var userName = principal.Identity.Name;
            
            //Get roles
            string[] roles = Roles.GetRolesForUser(userName);

            //initialize claims
            List<Claim> claims = new List<Claim>();
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //add name claim
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.GivenName, userName));

            //create claims
            return Principal.Create("Application",                    
                    claims.ToArray());
            
        }
    }
}