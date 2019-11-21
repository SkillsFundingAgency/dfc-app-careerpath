using DFC.App.CareerPath.Common.Contracts;

namespace DFC.App.CareerPath.Common.Services
{
    public class InMemoryCorrelationIdProvider : ICorrelationIdProvider
    {
        public string CorrelationId { get; set; }
    }
}
