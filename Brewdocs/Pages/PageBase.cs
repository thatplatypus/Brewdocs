using System;
using System.IO;
using Brewdocs.Services;
using Brewdocs.Services.Cache;
using Brewdocs.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Brewdocs.Pages
{
    public abstract class PageBase : ComponentBase, IDisposable
    {
        [Inject]
        public ThemeService ThemeService { get; set; } = default!;

        [Inject]
        public ILRUCacheService CacheService { get; set; } = default!;

        [Inject]
        public IActiveDocumentService ActiveDocumentService { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        public abstract string Identifier { get; }

        public abstract string ThemeColor { get; }

        protected virtual Func<IBrowserFile, Task<bool>> ValidateFileAsync { get; } = (file) => Task.FromResult(true);

        protected override void OnInitialized()
        {
            if (!string.IsNullOrWhiteSpace(ThemeColor))
                ChangeTheme(ThemeColor);

            ThemeService.ColorModeChanged += StateHasChanged;

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrWhiteSpace(ThemeColor))
                ChangeTheme(ThemeColor);

            base.OnParametersSet();
        }

        public void ChangeTheme(string color)
        {
            ThemeService.ChangeTheme(color);
        }

        public async Task AddAndCacheDocumentAsync(string key, string path, ActiveDocument data)
        {
            await ActiveDocumentService.AddAsync(Identifier, key, data);
            await CacheService.SaveAsync(BuildKey(key), new RecentDocument
            {
                Document = data,
                Path = path
            });
        }

        public async Task SaveToCacheAsync(string key, RecentDocument data)
        {
            await CacheService.SaveAsync(BuildKey(key), data);
        }

        public async Task AddActiveDocumentAsync(string key, ActiveDocument data)
        {
            await ActiveDocumentService.AddAsync(data.DocumentType, key, data);
            await CacheService.SaveAsync($"{data.DocumentType}:{key}", new RecentDocument
            {
                Document = data,
                Path = ""
            });
        }

        private async Task ActivateCachedDocument(RecentDocument document, string key)
        {
            await ActiveDocumentService.AddAsync(document.Document.DocumentType, key, document.Document);
        }

        public async Task<ActiveDocument> GetDocumentAsync(string key)
        {
            Console.WriteLine(Identifier);
            Console.WriteLine(key);

            var document = await ActiveDocumentService.GetAsync(Identifier, key);

            if (document?.Name != null)
            {
                document.LastAccessed = DateTime.Now;
                return document;
            }

            Console.WriteLine("Retrying from local storage cache");

            var cachedDocument = await CacheService.GetAsync(key);
            if (cachedDocument != null)
            {
                await ActivateCachedDocument(cachedDocument, key);
                cachedDocument.Document.LastAccessed = DateTime.Now;
                return cachedDocument.Document;
            }

            ActiveDocument? none = default!;

            return none;
        }

        protected async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            foreach (var file in args.GetMultipleFiles())
            {
                var document = await ProcessFileAsync(file, Identifier);

                if (args.FileCount == 1)
                {
                    NavigationManager.NavigateTo($"{Identifier}/{document?.ShortName}");
                }
            }
        }

        protected async Task<ActiveDocument?> ProcessFileAsync(IBrowserFile file, string documentType)
        {
            Console.WriteLine($"Processing file {file.Name} for document type {documentType}");
            if (!await ValidateFileAsync(file))
            {
                // Handle invalid file
                return default;
            }

            var contentBytes = new byte[file.Size];
            using var stream = file.OpenReadStream();
            await stream.ReadAsync(contentBytes);

            var document = new ActiveDocument()
            {
                DocumentType = documentType,
                Name = file.Name,
                Data = contentBytes,
            };

            await AddActiveDocumentAsync(file.Name, document);

            return document;
        }

        private string BuildKey(string key)
        {
            return $"{Identifier}:{key}";
        }

        public void Dispose()
        {
            ThemeService.ColorModeChanged -= StateHasChanged;
        }
    }
}

