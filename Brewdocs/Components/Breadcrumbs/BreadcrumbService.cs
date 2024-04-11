using System;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Brewdocs.Components.Breadcrumbs
{
    public class BreadcrumbService
    {
        private readonly NavigationManager _navigationManager;

        public BreadcrumbService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += HandleLocationChanged;

            HandleLocationChanged(this, new LocationChangedEventArgs(_navigationManager.Uri, false));
        }

        public event Action? OnChange;

        public List<BreadcrumbItem> Breadcrumbs { get; private set; } = new List<BreadcrumbItem>();

        private static readonly char[] separator = new[] { '/' };

        private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            // Split the URL into segments and create a breadcrumb for each segment
            var segments = new Uri(e.Location).AbsolutePath.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            Breadcrumbs = new List<BreadcrumbItem>
    {
        new BreadcrumbItem { Text = "Home", Href = "/", IsCurrentPage = segments.Length == 0 }
    };

            Breadcrumbs.AddRange(segments.Select((segment, index) => new BreadcrumbItem
            {
                Text = segment,
                Href = "/" + string.Join("/", segments.Take(index + 1)),
                IsCurrentPage = index == segments.Length - 1
            }));

            OnChange?.Invoke();
        }
    }
}

