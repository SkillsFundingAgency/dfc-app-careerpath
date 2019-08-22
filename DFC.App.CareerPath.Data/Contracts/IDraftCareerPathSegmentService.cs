using DFC.App.CareerPath.Data.Models;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface IDraftCareerPathSegmentService
    {
        Task<CareerPathSegmentModel> GetSitefinityData(string canonicalName);
    }
}
