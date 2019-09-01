﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Orange.ApiTokenValidation.API.Configuration
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            //operation.Deprecated = apiDescription.IsDeprecated();
            
            if (operation.Parameters == null)
            {
                return;
            }

            //// REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            //// REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            //foreach (var parameter in operation.Parameters.OfType< Swashbuckle.AspNetCore.Swagger.UnknownSwaggerDocument >())
            //{
            //    var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            //    if (parameter.Description == null)
            //    {
            //        parameter.Description = description.ModelMetadata?.Description;
            //    }

            //    //if (parameter.Default == null)
            //    //{
            //    //    parameter.Default = description.DefaultValue;
            //    //}

            //    //parameter.Required |= description.IsRequired;
            //}
        }
    }
}