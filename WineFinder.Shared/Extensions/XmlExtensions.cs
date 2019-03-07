namespace WineFinder.Shared.Extensions
{
    public static class XmlExtensions
    {
        public static bool Validates(this System.Xml.XmlNode node)
        {
            return (
                !string.IsNullOrEmpty(node["Wine"].ToString())
                );
        }

        public static bool DoesNotValidate(this System.Xml.XmlDocument xmlDoc)
        {
            return xmlDoc.InnerText.StartsWith("You are currently not logged into CellarTracker.");
        }
    }
}