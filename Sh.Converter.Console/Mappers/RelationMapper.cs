using Sh.Converter.ConsoleApp.Models;

namespace Sh.Converter.ConsoleApp.Mappers
{
    public class RelationMapper(Dictionary<string, Relation> relations)
    {
        private const char IdSeparator = '_';

        private readonly Dictionary<string, Relation> _relations = relations;
        
        private static void AdjustParentStack(Stack<string> parentStack, string key, Dictionary<string, Relation> relations)
        {
            while (parentStack.Count > 0)
            {
                var parentDesignator = parentStack.Peek().Split(IdSeparator)[0];
                
                if (relations[parentDesignator].CanHaveChildren.Contains(key)) break;
                
                parentStack.Pop();
            }
        }

        private static Node CreateNode(string id, Relation relation, string[] fieldValues)
        {
            var nameValues = Enumerable.Range(0, relation.Fields.Length)
                .ToDictionary(i => relation.Fields[i], i => i < fieldValues.Length ? fieldValues[i] : string.Empty);

            return new Node { Id = id, Type = relation.Type, NameValues = nameValues };
        }

        /// <summary>
        /// Builds a list of nodes and edges from a list of input lines using a specified separator.
        /// Each line is parsed to extract a key which is used to retrieve a corresponding Relation.
        /// A unique ID is generated for each node, and field values are extracted based on the separator.
        /// A parent stack is utilized to maintain hierarchical relationships: if a parent exists and the node is not a root,
        /// an edge is created linking the node to its parent. The method returns a tuple containing the constructed nodes and edges.
        /// </summary>
        /// <param name="lines" cref="string[]">The list of input lines to parse.</param>
        /// <param name="separator" cref="string">The separator used to delimit the key and field values in each line.</param>
        /// <returns>
        /// A tuple where the first item is a list of Node objects representing the parsed data,
        /// and the second item is a list of Edge objects representing the parent-child relationships.
        /// </returns>
        public (List<Node> nodes, List<Edge> edges) BuildFromLines(List<string> lines, string separator)
        {
            var nodes = new List<Node>();
            var edges = new List<Edge>();

            if (lines.Count == 0) return (nodes, edges);

            var parentStack = new Stack<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var firstSeparatorIndex = line.IndexOf(separator[0]);

                if (firstSeparatorIndex <= 0) continue;

                var key = line[..firstSeparatorIndex];

                if (!_relations.TryGetValue(key, out var relation)) continue;

                AdjustParentStack(parentStack, key, _relations);

                var id = $"{key}{IdSeparator}{i}";
                var values = line.Split(separator).Skip(1).ToArray();

                nodes.Add(CreateNode(id, relation, values));

                if (parentStack.Count > 0 && !relation.IsRoot)
                    edges.Add(new Edge { FromId = parentStack.Peek(), ToId = id });

                if (relation.CanHaveChildren.Length > 0 || parentStack.Count == 0 && relation.IsRoot)
                    parentStack.Push(id);
            }

            return (nodes, edges);
        }
    }
}
