namespace Sh.Converter.ConsoleApp.Models
{
    public class Node
    {
        public required string Id { get; set; }
        public required string Type { get; set; }
        public Dictionary<string, string> NameValues { get; set; } = [];
    }
}
