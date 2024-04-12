using System;
namespace Brewdocs.Services.Cache
{
    public interface ILRUCacheService
    {
        Task SaveAsync(string key, RecentDocument data);

        Task<RecentDocument> GetAsync(string key);

        Task<List<RecentDocument>> GetAllAsync();

        Task ClearCacheAsync();
    }
}

