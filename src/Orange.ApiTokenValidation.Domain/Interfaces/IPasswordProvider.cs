namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    internal interface IPasswordProvider
    {
        byte[] GetNextBytePassword(int passwordLength);
        string GetNextStringPassword(int passwordLength);
    }
}
