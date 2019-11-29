using DFC.App.CareerPath.Data.Models;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IMappingService
    {
        CareerPathSegmentModel MapToSegmentModel(string message, long sequenceNumber);
    }
}