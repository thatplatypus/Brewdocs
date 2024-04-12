using System;
namespace Brewdocs.Shared
{
    public static class DateFormatUtilities
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                return $"{timeSpan.Seconds} seconds ago";
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                return $"{timeSpan.Minutes} minutes ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                return $"{timeSpan.Hours} hours ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                return $"{timeSpan.Days} days ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                return $"{timeSpan.Days / 30} months ago";
            }
            else
            {
                return $"{timeSpan.Days / 365} years ago";
            }
        }
    }
}

