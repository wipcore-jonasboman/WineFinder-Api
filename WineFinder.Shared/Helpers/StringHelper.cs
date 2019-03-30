using System;
using System.Linq;

namespace WineFinder.Shared.Helpers
{
    public static class StringHelper
    {
        private const int StringLength = 6;

        public static string CreateListId()
        {
            var rndString = RandomString();
            while (CacheHelper.Exists(rndString))
            {
                rndString = RandomString();
            }
            return rndString;
        }

        private static Random random = new Random();

        private static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, StringLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
