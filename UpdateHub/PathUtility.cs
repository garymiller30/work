namespace UpdateHub
{
    public static class PathUtility
    {
        public static string NormalizeRelativePath(string path)
        {
            return (path ?? string.Empty).Replace('\\', '/').TrimStart('/');
        }

        public static string CombineUrl(string left, string right)
        {
            left = (left ?? string.Empty).Trim();
            right = NormalizeRelativePath(right);

            if (string.IsNullOrWhiteSpace(left))
            {
                return right;
            }

            return left.TrimEnd('/') + "/" + right;
        }

        public static string EnsureTrailingSlash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.TrimEnd('/', '\\') + "/";
        }
    }
}
