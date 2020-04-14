namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    interface ITokenDbContextFactory
    {
        TokenDbContext CreateDbContext();
    }
}
