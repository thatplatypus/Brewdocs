using System;
using System.Xml.Linq;
using Blazored.LocalStorage;

namespace Brewdocs.Services.Cache
{

    public class LRUCacheService : ILRUCacheService
    {
        private readonly ILocalStorageService _localStorageService;
        private Dictionary<string, Node> _cache = new Dictionary<string, Node>();
        private Node _head, _tail;
        private int _capacity = 25;
        private string _cacheKey = "__cache";

        public LRUCacheService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<List<RecentDocument>> GetAllAsync()
        {
            if (_cache.Count == 0 && (await _localStorageService.ContainKeyAsync(_cacheKey)))
            {
                var cacheJson = await _localStorageService.GetItemAsStringAsync(_cacheKey);

                _cache = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Node>>(cacheJson ?? "") ?? new();
            }

            return _cache.Select(x => x.Value.Value).ToList();
        }

        public async Task<RecentDocument> GetAsync(string key)
        {
            if (_cache.Count == 0 && (await _localStorageService.ContainKeyAsync(_cacheKey)))
            {
                var cacheJson = await _localStorageService.GetItemAsStringAsync(_cacheKey);

                _cache = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Node>>(cacheJson ?? "") ?? new();
            }

            Console.WriteLine(key);
            foreach (var n in _cache.Values)
            {
                Console.WriteLine($"Key {n.Key}, Value {n.Value.Document.Name}");
            }
            if (_cache.TryGetValue(key, out Node? value))
            {
                Node node = value;
                Console.WriteLine(node.Value.Document.Name);
                Remove(node);
                Add(node);
                var data = System.Text.Json.JsonSerializer.Serialize(_cache);
                await _localStorageService.SetItemAsStringAsync(_cacheKey, data ?? "");
                return node.Value;
            }

            return null;
        }

        public async Task SaveAsync(string key, RecentDocument data)
        {
            if (_cache.ContainsKey(key))
            {
                Node node = _cache[key];
                node.Value = data;
                Remove(node);
                Add(node);
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    _cache.Remove(_tail.Key);
                    Remove(_tail);
                }
                Node node = new Node() { Key = key, Value = data };
                Add(node);
                _cache.Add(key, node);
            }

            var json = System.Text.Json.JsonSerializer.Serialize(_cache);
            await _localStorageService.SetItemAsStringAsync(_cacheKey, json ?? "");
        }

        private void Add(Node node)
        {
            node.Next = _head;
            node.Prev = null;
            if (_head != null)
                _head.Prev = node;
            _head = node;
            if (_tail == null)
                _tail = _head;
        }

        private void Remove(Node node)
        {
            if (node.Prev != null)
                node.Prev.Next = node.Next;
            else
                _head = node.Next;
            if (node.Next != null)
                node.Next.Prev = node.Prev;
            else
                _tail = node.Prev;
        }
    }

    internal class Node
    {
        public string Key { get; set; }
        public RecentDocument Value { get; set; }
        public Node Next { get; set; }
        public Node Prev { get; set; }
    }
}

