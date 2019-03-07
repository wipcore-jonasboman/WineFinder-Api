namespace WineFinder.Shared.Models
{
    public class WineRequestData
    {
        public ShowType ShowType { get; set; }
        public bool ForceUpdate { get; set; }
        public bool IncludeLocation { get; set; }
    }
}
