using System;

namespace Brewdocs.Services.Cache
{
    public class ActiveDocumentService : IActiveDocumentService
    {
        private Dictionary<string, ActiveDocument> _cache = new();

        public List<ActiveDocument> ActiveDocuments => _cache.Select(x => x.Value).ToList();

        public event Action ActiveDocumentsChanged;

        public Task AddAsync(string documentType, string key, ActiveDocument data)
        {
            var cacheKey = GetKey(documentType, key);
            if (!_cache.TryAdd(cacheKey, data))
            {
                _cache[cacheKey] = data;
            }

            ActiveDocumentsChanged?.Invoke();

            return Task.CompletedTask;
        }

        public Task DownloadAsync(string documentType, string key)
        {
            throw new NotImplementedException();
        }

        public Task<List<ActiveDocument>> GetAllAsync()
        {
            return Task.FromResult(_cache.Select(x => x.Value).ToList());
        }

        public Task<ActiveDocument> GetAsync(string documentType, string key)
        {
            var cacheKey = GetKey(documentType, key);
            if (_cache.TryGetValue(cacheKey, out ActiveDocument? document))
            {
                return Task.FromResult(document);
            }
            else
            {
                Console.WriteLine("Cache miss");
            }

            return Task.FromResult(document);
        }

        private static string GetKey(string documentType, string key)
        {
            return $"{documentType}:{key}";
        }

        public Task RemoveAsync(ActiveDocument data)
        {
            Console.WriteLine($"Key request: {GetKey(data.DocumentType, data.Name)}");
            PrintCache();
            _cache.Remove(GetKey(data.DocumentType, data.Name));
            ActiveDocumentsChanged.Invoke();
            return Task.CompletedTask;
        }

        private void PrintCache()
        {
            foreach (var kvp in _cache)
            {
                Console.WriteLine($"Key {kvp.Key} | Value {kvp.Value}");
            }
        }
    }
}

