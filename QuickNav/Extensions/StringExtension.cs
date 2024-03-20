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
}
