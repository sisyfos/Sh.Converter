namespace Sh.Converter.ConsoleApp.Options
{
    using Sh.Converter.ConsoleApp.Models;

    class RelationOptions
    {
        public const string Relation = nameof(Relation);

        public required List<Relation> Relations { get; set; }

        
    }
}
