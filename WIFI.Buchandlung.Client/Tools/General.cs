namespace WIFI.Buchandlung.Client.Tools
{
    public static class General : System.Object
    {
        public static bool AreStringsValid(params string[] strings)
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
