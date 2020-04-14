using System;
using System.Collections.Generic;
using Dapper.Dommel.Resolvers;
using Newtonsoft.Json.Linq;

namespace Orange.ApiTokenValidation.Repositories.DapperConfig
{
    class DommelPropertyResolverForCustomTypes : DommelPropertyResolver
    {
        private HashSet<Type> _mappedTypes;

        protected override HashSet<Type> PrimitiveTypes
        {
            get { return _mappedTypes ??= new HashSet<Type>(base.PrimitiveTypes) {typeof(JObject)}; }
        }
    }
}