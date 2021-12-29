using System;
using Gsuke.ApiPlatform.Repositories;

namespace Gsuke.ApiPlatform.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        public string GetResourceList()
        {
            return "Hello";
        }
    }
}
