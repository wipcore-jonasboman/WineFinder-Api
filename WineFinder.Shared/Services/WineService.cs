using System;
using System.Collections.Generic;
using System.Linq;
using WineFinder.Shared.Business.Facades;
using WineFinder.Shared.Helpers;
using WineFinder.Shared.Models;

namespace WineFinder.Shared.Services
{
    public class WineService
    {
        private string _winesCacheKey = "MyWines";
        private string _handle;
        private string _key;

        public WineService(string handle, string key)
        {
            _handle = handle;
            _key = key;
            _winesCacheKey += $"{handle}_{key}".GetHashCode();
        }

        public List<WineItem> Get(WineRequestData request)
        {
            var wines = CacheHelper.Get<List<WineItem>>(_winesCacheKey);

            if (request.ForceUpdate || wines == null)
            {
                var cellarTrackerFacade = new CellarTrackerFacade(_handle, _key);

                var winesFromRepo = cellarTrackerFacade.LoadWines(request.IncludeLocation);

                if (winesFromRepo != null)
                {
                    CacheHelper.Set(_winesCacheKey, winesFromRepo);
                    wines = winesFromRepo;
                }
            }

            switch (request.ShowType)
            {
                case ShowType.All:
                {
                    return wines;
                }
                case ShowType.Pending:
                {
                    return wines.Where(wine => wine.Pending > 0).ToList();
                }
                default:
                {
                    return wines.Where(wine => wine.Quantity > 0).ToList();
                }
            }
        }
    }
}