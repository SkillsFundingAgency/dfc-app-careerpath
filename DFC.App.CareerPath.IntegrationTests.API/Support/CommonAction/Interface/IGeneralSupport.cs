﻿using Newtonsoft.Json;
using System;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IGeneralSupport
    {
        string RandomString(int length);

        byte[] ConvertObjectToByteArray(object obj);

        T GetResource<T>(string resourceName);
    }
}
