using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Web.Swagger
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Any(x => x.Filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Any(x => x.Filter is IAllowAnonymousFilter);

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "UserName",
                In = ParameterLocation.Header,
                Description = "username",
                Required = false
            });

            if (isAuthorized && !allowAnonymous)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "access token",
                    Required = true
                });
            }
        }
    }
}