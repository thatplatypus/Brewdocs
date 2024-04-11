namespace Brewdocs.Services.Cache
{
    public interface IActiveDocumentService
    {
        List<ActiveDocument> ActiveDocuments { get; }

        Task AddAsync(string documentType, string key, ActiveDocument data);

        Task RemoveAsync(ActiveDocument data);

        Task DownloadAsync(string documentType, string key);

        Task<ActiveDocument> GetAsync(string documentType, string key);

        Task<List<ActiveDocument>> GetAllAsync();

        public event Action ActiveDocumentsChanged;
    }
}

