using System;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface ICosmosRepository<T>
        where T : IDataModel
    {
    }
}
