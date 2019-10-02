using DFC.App.CareerPath.Data.Models.PatchModels;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard.Attributes;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Functions
{

    public static class PatchSegmentMarkup
    {
        private static readonly string ThisClassPath = typeof(PatchSegmentMarkup).FullName;

        [FunctionName("PatchSegmentMarkup")]
        public static async Task Run([ServiceBusTrigger("%patch-markup-segment-topic-name%", "%patch-markup-segment-subscription-name%", Connection = "service-bus-connection-string")]string serviceBusMessage,
                                            ILogger log,
                                            [Inject] SegmentClientOptions segmentClientOptions,
                                            [Inject] HttpClient httpClient)
        {
            var serviceBusModel = JsonConvert.DeserializeObject<CareerPathPatchMarkupServiceBusModel>(serviceBusMessage);

            log.LogInformation($"{ThisClassPath}: JobProfile Id: {serviceBusModel.JobProfileId}: Patching Markup segment");

            var segmentDataModel = await HttpClientService.GetByIdAsync(httpClient, segmentClientOptions, serviceBusModel.JobProfileId).ConfigureAwait(false);

            if (segmentDataModel == null || segmentDataModel.LastReviewed < serviceBusModel.Data?.LastReviewed)
            {
                var careerPathPatchMarkupSegmentModel = new CareerPathPatchMarkupSegmentModel
                {
                    DocumentId = serviceBusModel.JobProfileId,
                    SocLevelTwo = serviceBusModel.SocLevelTwo,
                    CanonicalName = serviceBusModel.CanonicalName,
                    Data = serviceBusModel.Data,
                };

                var result = await HttpClientService.PatchSegmentMarkupAsync(httpClient, segmentClientOptions, careerPathPatchMarkupSegmentModel).ConfigureAwait(false);

                if (result == HttpStatusCode.OK)
                {
                    log.LogInformation($"{ThisClassPath}: JobProfile Id: {serviceBusModel.JobProfileId}: Patched Markup segment");
                }
                else
                {
                    log.LogWarning($"{ThisClassPath}: JobProfile Id: {serviceBusModel.JobProfileId}: Segment not patched: Status: {result}");
                }
            }
            else
            {
                log.LogWarning($"{ThisClassPath}: JobProfile Id: {serviceBusModel.JobProfileId}: Service bus message out of date: {serviceBusModel.Data.LastReviewed}, stored: {segmentDataModel.LastReviewed}");
            }
        }
    }
}
