﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support
{
    internal class ResourceManager
    {
        internal static T GetResource<T>(string resourceName)
        {
            string content = GetResourceContent(resourceName);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private static string GetResourceContent(string resourceName)
        {
            DirectoryInfo resourcesDirectory = Directory.CreateDirectory(Environment.CurrentDirectory).GetDirectories("Resource")[0];
            FileInfo[] files = resourcesDirectory.GetFiles();
            FileInfo selectedResource = null;

            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                if (files[fileIndex].Name.StartsWith(resourceName, StringComparison.CurrentCultureIgnoreCase))
                {
                    selectedResource = files[fileIndex];
                    break;
                }
            }

            if (selectedResource == null)
            {
                throw new Exception($"No resource with the name {resourceName} was found");
            }

            using (StreamReader streamReader = new StreamReader(selectedResource.FullName))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}