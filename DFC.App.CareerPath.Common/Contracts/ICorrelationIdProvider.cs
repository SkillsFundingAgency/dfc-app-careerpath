namespace DFC.App.CareerPath.Common.Contracts
{
    public interface ICorrelationIdProvider
    {
        string CorrelationId { get; set; }
    }
}