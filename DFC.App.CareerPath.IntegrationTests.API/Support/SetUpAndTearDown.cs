using DFC.App.CareerPath.Tests.Common.AzureServiceBusSupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Enums;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    public class SetUpAndTearDown
    {
        internal JobProfileContentType JobProfile { get; private set; }

        internal CommonAction CommonAction { get; } = new CommonAction();

        internal Topic Topic { get; private set; }

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            this.CommonAction.InitialiseAppSettings();
            this.Topic = new Topic(Settings.ServiceBusConfig.ConnectionString);
            this.JobProfile = this.CommonAction.GenerateJobProfileContentType();
            this.JobProfile.CareerPathAndProgression = "This is the original career path content";
            byte[] jobProfileMessageBody = this.CommonAction.ConvertObjectToByteArray(this.JobProfile);
            Message jobProfileMessage = this.CommonAction.CreateServiceBusMessage(this.JobProfile.JobProfileId, jobProfileMessageBody, ActionType.Published, CType.JobProfile);
            await this.CommonAction.SendMessage(this.Topic, jobProfileMessage).ConfigureAwait(true);
            await Task.Delay(5000).ConfigureAwait(true);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await this.CommonAction.DeleteJobProfile(this.Topic, this.JobProfile).ConfigureAwait(true);
        }
    }
}
