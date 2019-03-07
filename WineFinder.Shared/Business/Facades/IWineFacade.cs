using System.Collections.Generic;
using WineFinder.Shared.Models;

namespace WineFinder.Shared.Business.Facades
{
    public interface IWineFacade
    {
        List<WineItem> LoadWines(bool includeLocation);
    }
}