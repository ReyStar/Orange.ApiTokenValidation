using Microsoft.AspNetCore.Mvc;
using Orange.ApiTokenValidation.API.Controllers;

namespace Orange.ApiTokenValidation.API.Attributes
{
    /// <summary>
    /// Api version attribute, use only definition api versions
    /// </summary>
    public class OrangeApiVersionAttribute : ApiVersionAttribute
    {
        public OrangeApiVersionAttribute(ApiVersions version) 
            : base(new ApiVersion((int) version, 0))
        {

        }
    }
}
