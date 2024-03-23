
using System.Text;

namespace QuickNav.Extensions;

public static class StringBuilderExtension
{
    public static void AppendMarkdownLine(this StringBuilder sb, string text)
    {
        //two spaces = new line in markdown
        sb.AppendLine(text + "  ");
    }
}
