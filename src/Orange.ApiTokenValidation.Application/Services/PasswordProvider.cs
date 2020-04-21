using System;
using System.Security.Cryptography;
using System.Text;
using EnsureThat;
using Orange.ApiTokenValidation.Application.Interfaces;

namespace Orange.ApiTokenValidation.Application.Services
{
    internal class PasswordProvider : IPasswordProvider
    {
        private readonly string _legalPasswordCharacters;
        private const string DefaultPasswordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public PasswordProvider()
           : this(DefaultPasswordCharacters)
        {
        }

        public PasswordProvider(string legalPasswordCharacters)
        {
            Ensure.That(legalPasswordCharacters.Length > 10, nameof(legalPasswordCharacters),
                        p => p.WithMessage("The legal password characters array must be more than 10."));

            _legalPasswordCharacters = legalPasswordCharacters;
        }

        /// <summary>
        /// Get next string password with specific length
        /// </summary>
        /// <param name="passwordLength">
        /// Password length
        /// </param>
        /// <returns>
        /// Password string
        /// </returns>
        public string GetNextStringPassword(int passwordLength)
        {
            var passwordCharacters = _legalPasswordCharacters;
            var stringBuilder = new StringBuilder();

            using (var cryptoServiceProvider = GetRandomNumberGenerator())
            {
                while (passwordLength-- > 0)
                    stringBuilder.Append(passwordCharacters[GetInt(cryptoServiceProvider, passwordCharacters.Length)]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get next password as byte array with specific length
        /// </summary>
        /// <param name="passwordLength">
        /// Password length
        /// </param>
        /// <returns>
        /// Password bytes
        /// </returns>
        public virtual byte[] GetNextBytePassword(int passwordLength)
        {
            return Encoding.UTF8.GetBytes(GetNextStringPassword(passwordLength));
        }

        private static int GetInt(RandomNumberGenerator rnd, int max)
        {
            var data = new byte[4];
            int num;
            do
            {
                rnd.GetBytes(data);
                num = BitConverter.ToInt32(data, 0) & int.MaxValue;
            }
            while (num >= max * (int.MaxValue / max));
            return num % max;
        }

        private static RandomNumberGenerator GetRandomNumberGenerator()
        {
            return RandomNumberGenerator.Create();
        }
    }
}