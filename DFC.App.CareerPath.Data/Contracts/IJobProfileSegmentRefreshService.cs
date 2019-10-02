using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface IJobProfileSegmentRefreshService<TModel>
    {
        Task SendMessageAsync(TModel model);
    }
}