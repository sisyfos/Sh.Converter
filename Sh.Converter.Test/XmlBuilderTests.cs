
namespace Sh.Converter.Test
{
    using Sh.Converter.ConsoleApp.Models;
    using Sh.Converter.ConsoleApp.Builders;
    using System.Xml.Linq;


    public class XmlBuilderTests
    {
        [Fact]
        public void CreateElements_EmptyNodes_ReturnsEmptyList()
        {
            // Arrange
            var nodes = new List<Node>();
            var edges = new List<Edge>();

            // Act
            List<XElement> elements = XmlBuilder.CreateElements((nodes, edges));

            // Assert
            Assert.Empty(elements);
        }

        [Fact]
        public void CreateElements_ValidNodes_CreatesXmlHierarchy()
        {
            // Arrange
            var rootNode = new Node
            {
                Id = "P_0",
                Type = "Person",
                NameValues = new Dictionary<string, string>
                {
                    {"Name", "John"}
                }
            };
            var childNode = new Node
            {
                Id = "C_1",
                Type = "Child",
                NameValues = new Dictionary<string, string>
                {
                    {"Name", "Doe"}
                }
            };
            var nodes = new List<Node> { rootNode, childNode };
            var edges = new List<Edge>
            {
                new() { FromId = "P_0", ToId = "C_1" }
            };

            // Act
            List<XElement> elements = XmlBuilder.CreateElements((nodes, edges));

            // Assert
            // There should be one root element.
            Assert.Single(elements);
            var personElement = elements.First();
            Assert.Equal("Person", personElement.Name.LocalName);
            // The Person element should have one child element (Child).
            XElement childElement = personElement.Element("Child");
            Assert.NotNull(childElement);
            // Validate the child's content.
            Assert.Equal("Doe", childElement.Element("Name")?.Value);
        }
    }
}
