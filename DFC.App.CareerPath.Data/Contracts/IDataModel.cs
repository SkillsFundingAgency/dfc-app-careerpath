using System;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface IDataModel
    {
        Guid DocumentId { get; set; }

        string Etag { get; set; }

        string PartitionKey { get; }
    }
}
