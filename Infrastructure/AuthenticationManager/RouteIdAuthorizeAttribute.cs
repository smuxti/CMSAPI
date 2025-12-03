using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationManager
{
    public class RouteIdAuthorizeAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _routeId;

        public RouteIdAuthorizeAttribute(string routeId)
        {
            _routeId = routeId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var userClaims = context.HttpContext.User.Claims;

            var roleClaims = userClaims.FirstOrDefault(c => c.Type == "RoleTypeCode")?.Value; 
            if (roleClaims == "1")
            {
                return;
            }

            var routeClaims = userClaims
                .Where(c => c.Type == "RouteId")
                .Select(c => c.Value)
                .ToList();

            if (!routeClaims.Contains(_routeId))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
