using System;
using Gsuke.ApiPlatform.Models;

namespace Gsuke.ApiPlatform.Repositories
{
    public interface IResourceRepository
    {
        IEnumerable<Resource> GetList();
        Resource Get(string url);
    }
}
