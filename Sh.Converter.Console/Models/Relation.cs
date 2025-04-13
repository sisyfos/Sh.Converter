namespace Sh.Converter.ConsoleApp.Models
{
    class Relation
    {
        public bool IsRoot { get; set; }
        public required string Designator { get; set; }
        public required string Type { get; set; }
        public required string[] Fields { get; set; }
        public string[] CanHaveChildren { get; set; } = [];
    }
}
