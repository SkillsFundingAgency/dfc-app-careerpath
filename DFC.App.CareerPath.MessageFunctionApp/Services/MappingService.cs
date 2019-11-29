using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using Newtonsoft.Json;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public class MappingService : IMappingService
    {
        private readonly IMapper mapper;

        public MappingService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public CareerPathSegmentModel MapToSegmentModel(string message, long sequenceNumber)
        {
            var fullJobProfileMessage = JsonConvert.DeserializeObject<JobProfileMessage>(message);
            var fullJobProfile = mapper.Map<CareerPathSegmentModel>(fullJobProfileMessage);
            fullJobProfile.SequenceNumber = sequenceNumber;

            return fullJobProfile;
        }
    }
}