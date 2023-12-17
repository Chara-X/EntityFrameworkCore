namespace EntityFrameworkCore.Tools;

public static class StringExtension
{
    public static string ReplaceFirst(this string source)
    {
        var tem = source.Replace('.', '_');
        var chars = tem.ToCharArray();
        chars[tem.IndexOf('_')] = '.';
        return '[' + new string(chars).Replace(".", "].[") + ']';
    }

    public static string ReplaceLast(this string source)
    {
        var tem = source.Replace('.', '_');
        var chars = tem.ToCharArray();
        chars[tem.LastIndexOf('_')] = '.';
        return '[' + new string(chars).Replace(".", "].[") + ']';
    }
}