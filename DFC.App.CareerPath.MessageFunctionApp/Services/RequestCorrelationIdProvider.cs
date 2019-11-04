namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class RequestCorrelationIdProvider : ICorrelationIdProvider
    {

        public RequestCorrelationIdProvider()
        {

        }

        public string CorrelationId { get; set; }
    }
}
