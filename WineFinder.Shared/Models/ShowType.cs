namespace WineFinder.Shared.Models
{
    public enum ShowType
    {
        Normal = 0, // not showing hidden wines nor wines not in stock
        Pending = 1,
        All = 2 // even bottles no longer in stock
    }
}
