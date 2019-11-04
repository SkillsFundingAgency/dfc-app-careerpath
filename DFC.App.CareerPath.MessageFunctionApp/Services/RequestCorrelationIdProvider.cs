namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class RequestCorrelationIdProvider : ICorrelationIdProvider
    {
        public string CorrelationId { get; set; }
    }
}
