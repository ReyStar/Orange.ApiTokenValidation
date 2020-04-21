namespace Orange.ApiTokenValidation.Application.Interfaces
{
    public interface IMeasurer
    {
        void Heartbeat();
        void RequestMetric(string path, string method, int statusCode, string correlationId);
    }
}
