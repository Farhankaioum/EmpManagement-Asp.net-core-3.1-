using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmpManagement.Security
{
    public class CanEditOnlOtherAdminRolesAndClaimsHandler :
                AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor contextAccessor;

        public CanEditOnlOtherAdminRolesAndClaimsHandler(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ManageAdminRolesAndClaimsRequirement requirement)
        {
            //var authFilterContext = context.Resource as AuthorizationFilterContext;
            var a = contextAccessor.HttpContext.Request.Query["userId"].ToString();
            //if (authFilterContext == null)
            //{
            //    return Task.CompletedTask;
            //}
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //var value = authFilterContext.HttpContext.Request.Query["userId"].FirstOrDefault();
            //string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];
                

            if (context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Edit Role"
                                    ) && a.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
