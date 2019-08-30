using System.Data;

namespace Orange.ApiTokenValidation.Repositories
{
    internal interface IDataSource
    {
        IDbConnection Connection { get; }
    }
}
