using System;
using System.Configuration;

namespace WineFinder.Shared.Helpers
{
    public static class CacheHelper
    {
        private const string CacheSetting = "Cache.Duration";
        private static System.Web.Caching.Cache _cache { get { return System.Web.HttpContext.Current.Cache; } }
        private static int _cacheDuration { get; set; }

        static CacheHelper()
        {
            _cacheDuration = GetCacheDuration();
        }

        public static void Set(string key, object value)
        {
            _cache.Insert(key, value,
                          null,
                          System.Web.Caching.Cache.NoAbsoluteExpiration,
                          GetSlidingExpiration(_cacheDuration),
                          System.Web.Caching.CacheItemPriority.Default,
                          null);
        }

        public static T Get<T>(string key)
        {
            T value = default(T);

            if (Exists(key))
                value = (T)_cache.Get(key);

            return value;
        }

        public static bool Exists(string key)
        {
            return _cache.Get(key) != null;
        }

        private static TimeSpan GetSlidingExpiration(int duration)
        {
            return new TimeSpan(0, duration, 0);
        }

        private static int GetCacheDuration()
        {
            //default
            int duration = 15;
            var setting = ConfigurationManager.AppSettings[CacheSetting];

            if (!string.IsNullOrEmpty(setting))
                int.TryParse(setting, out duration);

            return duration;
        }
    }
}