using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Orange.ApiTokenValidation.API.Attributes
{
    public class VersionRoute : RouteAttribute
    {
        public VersionRoute(string prefix) : base($"v{{version:apiVersion}}/{prefix}")
        {
        }
    }
}
