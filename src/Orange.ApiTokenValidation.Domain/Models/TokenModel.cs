namespace Orange.ApiTokenValidation.Domain.Models
{
    public class TokenModel
    {
        public TokenModel()
        {

        }

        public TokenModel(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
