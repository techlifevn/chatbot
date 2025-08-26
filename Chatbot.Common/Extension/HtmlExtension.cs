using System.Text.RegularExpressions;

namespace Chatbot.Common.Extension
{
    public static class HtmlExtension
    {
        public static IEnumerable<string> GetImageTagsInHTMLString(this string htmlString)
        {
            MatchCollection matches = Regex.Matches(htmlString, @"<(img)\b[^>]*>", RegexOptions.IgnoreCase);
            for (int i = 0, l = matches.Count; i < l; i++)
            {
                yield return matches[i].Value;
            }
        }

        public static string GetImageSourceAttribute(this string imageTagString)
        {
            return Regex.Match(imageTagString, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
        }
    }
}
