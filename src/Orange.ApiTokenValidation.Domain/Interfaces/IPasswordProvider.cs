namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    internal interface IPasswordProvider
    {
        /// <summary>
        /// Create byte array password
        /// </summary>
        byte[] GetNextBytePassword(int passwordLength);

        /// <summary>
        /// Create string password
        /// </summary>
        string GetNextStringPassword(int passwordLength);
    }
}
