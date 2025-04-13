namespace Sh.Converter.ConsoleApp.Builders
{
    using Sh.Converter.ConsoleApp.Models;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public static class XmlBuilder
    {
        /// <summary>
        /// Creates XML elements from a tuple containing nodes and edges.
        /// Constructs a dictionary of nodes for fast lookup and a lookup for parent-child relationships,
        /// identifies the root nodes (nodes not referenced as children in any edge),
        /// and recursively builds XML elements that mirror the hierarchical structure defined by the nodes and edges.
        /// </summary>
        /// <param name="data">A tuple where the first item is a list of Node objects and the second item is a list of Edge objects.</param>
        /// <returns>A list of XElement objects representing the constructed XML elements.</returns>
        public static List<XElement> CreateElements((List<Node> nodes, List<Edge> edges) data)
        {
            var elements = new List<XElement>();

            if (data.nodes == null || data.nodes.Count == 0) return elements;

            var nodeMap = data.nodes.ToDictionary(n => n.Id);
            var childrenMap = data.edges.ToLookup(e => e.FromId, e => e.ToId);

            // Find root nodes (nodes not referenced as children in any edge)
            var childIds = new HashSet<string>(data.edges.Select(e => e.ToId));
            var rootNodes = data.nodes.Where(n => !childIds.Contains(n.Id));

            // Add all root nodes to the document
            foreach (var rootNode in rootNodes)
            {
                elements.Add(BuildNodeElement(rootNode));
            }

            return elements;

            XElement BuildNodeElement(Node node)
            {
                // Create element with properties as child elements
                var element = new XElement(node.Type);

                // Add name-value pairs as child elements
                foreach (var (name, value) in node.NameValues)
                {
                    element.Add(new XElement(name, value));
                }

                // Add child nodes recursively
                foreach (var childId in childrenMap[node.Id])
                {
                    if (nodeMap.TryGetValue(childId, out var childNode))
                    {
                        element.Add(BuildNodeElement(childNode));
                    }
                }

                return element;
            }
        }

        public static XDocument CreateDocument(string root) => new XDocument(new XElement(root));
    }
}