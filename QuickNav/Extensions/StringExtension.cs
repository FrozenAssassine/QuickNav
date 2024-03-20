using System;

namespace QuickNav.Extensions;

public static class StringExtension
{
    public static bool IsUrl(this string input, out Uri uri)
    {
        return Uri.TryCreate(input, UriKind.Absolute, out uri);
    }
    public static bool IsUrl(this string input)
    {
        return Uri.TryCreate(input, UriKind.Absolute, out Uri uri);
    }
}
