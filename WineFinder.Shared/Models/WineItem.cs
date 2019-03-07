using System.Xml;

namespace WineFinder.Shared.Models
{
    public class WineItem
    {
        public WineItem(XmlNode node)
        {
            Name = node["Wine"].InnerText;
            Year = GetNumeric(node["Vintage"].InnerText);
            WineType = GetWineType(node["Type"].InnerText);
            Producer = node["Producer"].InnerText;
            Country = node["Country"].InnerText;
            Region = node["Region"].InnerText;
            District = node["SubRegion"].InnerText;
            Appelation = node["Appellation"].InnerText;
            GrapeVariety = node["MasterVarietal"].InnerText;
            WhenToDrink = node["BeginConsume"].InnerText + "-" + node["EndConsume"].InnerText;
            Price = GetDecimal(node["Price"].InnerText);
            Currency = node["Currency"].InnerText;
            BottlesBought = 0;
            BottlesDrunk = 0;
            Quantity = GetNumeric(node["Quantity"].InnerText);
            Pending = GetNumeric(node["Pending"].InnerText);
            Id = GetNumeric(node["iWine"].InnerText);
            Points = GetDecimal(node["CT"].InnerText);
            if (node["Location"] != null) Location = node["Location"].InnerText;
            if (node["Bin"] != null) Bin = node["Bin"].InnerText;
        }

        private decimal GetDecimal(string value)
        {
            return (decimal.TryParse(value.Replace('−', '-'), out decimal res)) ? decimal.Round(res) : 0;
        }

        private int GetNumeric(string value)
        {
            return (int.TryParse(value, out int res)) ? res : 0;
        }

        public int Id { get; private set; }
        public decimal Points { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public WineType WineType { get; set; }
        public string Producer { get; set; }
        public string Country{ get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Appelation { get; set; }
        public string GrapeVariety{ get; set; }
        public string WhenToDrink { get; set; }
        public decimal Price{ get; set; }
        public string Currency { get; set; }
        public decimal Quantity { get; set; }
        public int Pending { get; set; }
        public decimal BottlesBought { get; set; }
        public decimal BottlesDrunk { get; set; }
        public string Vintage { get { return Year == 0 || Year == 1001 ? string.Empty : Year.ToString(); } }
        public decimal CostSum { get { return (BottlesBought * Price); } }
        public string Location { get; set; }
        public string Bin { get; set; }

        private WineType GetWineType(string typeFromString)
        {
            if (string.IsNullOrEmpty(typeFromString))
            {
                return WineType.Undefined;
            }
            WineType wineType;
            switch (typeFromString.Trim().ToLower())
            {
                case "rött vin":
                case "red":
                    {
                        wineType = WineType.RedWine;
                        break;
                    }
                case "vitt vin":
                case "white":
                    {
                        wineType = WineType.WhiteWine;
                        break;
                    }
                case "mousserande vin":
                case "white - sparkling":
                    {
                        wineType = WineType.Sparkling;
                        break;
                    }
                default:
                    {
                        wineType = WineType.Undefined;
                        break;
                    }
            }
            return wineType;
        }
    }
    
    public enum WineType
    {
        RedWine = 1,
        WhiteWine = 2,
        Sparkling = 3,
        Undefined = 4,
        Other = 5
    }

    public enum ShowType
    {
        Normal = 0, // not showing hidden wines nor wines not in stock
        Pending = 1,
        All = 2 // even bottles no longer in stock
    }
}