namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface IMeasurer
    {
        void Heartbeat();
        void RequestMetric(string path, string method, int statusCode, string correlationId);
    }
}
