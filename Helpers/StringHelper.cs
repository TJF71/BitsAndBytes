using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Helpers
{
    public static class StringHelper
    {
        public static string BlogPostSlug(string? title)
        {
            string? output = RemoveAccents(title);
            
            // Remove the special characters
            output = Regex.Replace(output!, @"[^A-Za-z0-9\s-]","");

            // Remove multiple spaces replaced single space
            output = Regex.Replace(output, @"\s+", " ");

            // Replace single spaces with hypen
            output = Regex.Replace(output, @"\s", "-");
            output = output.Trim();

            return output.ToLower();
        }

        private static string? RemoveAccents(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            // Convert to Unicode
            title = title.Normalize(NormalizationForm.FormD);

            // Format unicode/ascii
            char[] chars = title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray(); 

            // Convert back return the new title

            return new string(chars).Normalize(NormalizationForm.FormC);    
        }
    }
}
