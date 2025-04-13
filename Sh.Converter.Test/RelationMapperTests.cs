namespace Sh.Converter.Test
{
    using Sh.Converter.ConsoleApp.Mappers;
    using Sh.Converter.ConsoleApp.Models;

    public class RelationMapperTests
    {
        private static Relation CreateRelation(bool isRoot, string designator, string type, string[] fields, string[] canHaveChildren = null)
        {
            return new Relation
            {
                IsRoot = isRoot,
                Designator = designator,
                Type = type,
                Fields = fields,
                CanHaveChildren = canHaveChildren ?? []
            };
        }

        [Fact]
        public void BuildFromLines_EmptyInput_ReturnsEmptyLists()
        {
            // Arrange
            var relations = new Dictionary<string, Relation>
            {
                { "P", CreateRelation(true, "P", "Person", ["Name", "Age"]) }
            };
            var mapper = new RelationMapper(relations);
            var lines = new List<string>();

            // Act
            var (nodes, edges) = mapper.BuildFromLines(lines, "|");

            // Assert
            Assert.Empty(nodes);
            Assert.Empty(edges);
        }

        [Fact]
        public void BuildFromLines_ValidInput_CreatesNodesAndEdges()
        {
            // Arrange
            var relations = new Dictionary<string, Relation>
            {
                { "P", CreateRelation(true, "P", "Person", ["FirstName", "Age"], ["C"]) },
                { "C", CreateRelation(false, "C", "Child", ["LastName", "Age"]) }
            };
            var mapper = new RelationMapper(relations);
            var lines = new List<string>
            {
                "P|John|30",
                "C|Doe|15"
            };

            // Act
            var (nodes, edges) = mapper.BuildFromLines(lines, "|");

            // Assert
            // First node (root) and second node (child)
            Assert.Equal(2, nodes.Count);
            Assert.Single(edges);
            // Validate that the edge correctly links the parent and child.
            Assert.Equal(nodes.First().Id, edges.First().FromId);
            Assert.Equal(nodes.Last().Id, edges.First().ToId);
        }
    }
}