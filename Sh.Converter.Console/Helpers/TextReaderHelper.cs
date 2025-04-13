namespace Sh.Converter.ConsoleApp.Helpers
{
    class TextReaderHelper()
    {
        public static IEnumerable<string> ReadLines(string source)
        {
            if (string.IsNullOrEmpty(source) || !File.Exists(source)) yield break;

            string? line;

            using StreamReader sr = new(File.OpenRead(source));
            while ((line = sr.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static IEnumerable<List<string>> ReadAllLines(Func<string, bool> isNextNode, string source)
        {
            var firstNodeFound = false;
            var nodes = new List<string>();

            foreach (var line in ReadLines(source))
            {
                // Skip completely empty lines
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Check if this is the start of a new node group
                if (isNextNode(line))
                {
                    // If we've already collected some data, return the previous node group
                    if (firstNodeFound && nodes.Count > 0)
                    {
                        yield return new List<string>(nodes);
                        nodes.Clear();
                    }

                    firstNodeFound = true;
                }
                else if (!firstNodeFound)
                {
                    // Skip data before the first 'P' node
                    continue;
                }

                nodes.Add(line);
            }

            // Return any remaining nodes
            if (nodes.Count > 0)
            {
                yield return new List<string>(nodes);
            }
        }
    }
}
