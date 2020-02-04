using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface IJobProfileSegmentRefreshService<TModel>
    {
        Task SendMessageAsync(TModel model);

        Task SendMessageListAsync(IList<TModel> models);
    }
}