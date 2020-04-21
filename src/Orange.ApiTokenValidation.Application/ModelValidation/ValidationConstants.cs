namespace Orange.ApiTokenValidation.Application.ModelValidation
{
    public class ValidationConstants
    {
        public const int MinPasswordLength = 64;
        public const int MaxPasswordLength = 255;
        public const long MinTtlValue = 60;
        public const long MaxTtlValue = 3600;
        public const int MinParticipantLength = 10;
        public const int MaxParticipantLength = 255;
    }
}
