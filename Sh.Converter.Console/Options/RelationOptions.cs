using Sh.Converter.ConsoleApp.Models;

namespace Sh.Converter.ConsoleApp.Options
{
    class RelationOptions
    {
        public const string Relation = nameof(Relation);

        public required List<Relation> Relations { get; set; }

        
    }
}
