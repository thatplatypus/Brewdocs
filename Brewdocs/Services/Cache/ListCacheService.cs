using System;
using Blazored.LocalStorage;

namespace Brewdocs.Services.Cache
{
    public class ListCacheService : ILRUCacheService
    {
        private readonly ILocalStorageService _localStorageService;
        private List<RecentDocument> _cache = new();
        private readonly int _capacity = 25;
        private readonly string _cacheKey = "__cache";

        public ListCacheService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<List<RecentDocument>> GetAllAsync()
        {
            if (_cache.Count == 0 && (await _localStorageService.ContainKeyAsync(_cacheKey)))
            {
                var cacheJson = await _localStorageService.GetItemAsStringAsync(_cacheKey);
                _cache = System.Text.Json.JsonSerializer.Deserialize<List<RecentDocument>>(cacheJson ?? "") ?? new List<RecentDocument>();
            }

            return _cache;
        }

        public async Task<RecentDocument> GetAsync(string key)
        {
            if (_cache.Count == 0 && (await _localStorageService.ContainKeyAsync(_cacheKey)))
            {
                var cacheJson = await _localStorageService.GetItemAsStringAsync(_cacheKey);
                _cache = System.Text.Json.JsonSerializer.Deserialize<List<RecentDocument>>(cacheJson ?? "") ?? new List<RecentDocument>();
            }

            Console.WriteLine(key);

            var document = _cache.FirstOrDefault(x => x.Document.Name == key);
            if (document != null)
            {
                _cache.Remove(document); // Remove the document from its current position
                _cache.Insert(0, document); // Add it back to the start of the list
                var data = System.Text.Json.JsonSerializer.Serialize(_cache);
                await _localStorageService.SetItemAsStringAsync(_cacheKey, data ?? "");
            }

            Console.WriteLine(document?.Document?.Name);

            return document;
        }

        public async Task SaveAsync(string key, RecentDocument data)
        {
            var existingDocument = _cache.FirstOrDefault(x => x.Document.Name == key);
            if (existingDocument != null)
            {
                _cache.Remove(existingDocument); // Remove the existing document
            }
            else if (_cache.Count >= _capacity)
            {
                _cache.RemoveAt(_cache.Count - 1); // Remove the oldest document if the cache is full
            }

            _cache.Insert(0, data); // Add the new document to the start of the list

            var json = System.Text.Json.JsonSerializer.Serialize(_cache);
            await _localStorageService.SetItemAsStringAsync(_cacheKey, json ?? "");
        }
    }
}

