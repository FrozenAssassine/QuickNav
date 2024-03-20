using System;

namespace QuickNav.Extensions;

public static class StringExtension
{
    public static bool IsUrl(this string input, out Uri uriResult)
    {
        return Uri.TryCreate(input, UriKind.Absolute, out uriResult);
    }
    public static bool IsUrl(this string input)
    {
        return IsUrl(input, out Uri uri);
    }

    public static int CountWords(this string text)
    {
        return text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static int CountLines(this string text)
    {
        if (text.Contains("\r\n"))
        {
            return text.Split("\r\n").Length;
        }
        return text.Split("\n").Length;
    }
}
