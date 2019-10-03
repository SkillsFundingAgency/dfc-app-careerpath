using System;

namespace DFC.App.CareerPath.Data.Models.PatchModels
{
    public class CareerPathPatchSegmentModel
    {
        public string CanonicalName { get; set; }

        public string SocLevelTwo { get; set; }

        public CareerPathSegmentDataModel Data { get; set; }
    }
}
