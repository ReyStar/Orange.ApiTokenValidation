using System;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework.Models
{
    class VersionInfo
    {
        public long Version { get; set; }

        public DateTime AppliedOn { get; set; }

        public string Description { get; set; }
    }
}
