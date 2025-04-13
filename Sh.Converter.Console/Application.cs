namespace Sh.Converter.ConsoleApp
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Sh.Converter.ConsoleApp.Builders;
    using Sh.Converter.ConsoleApp.Helpers;
    using Sh.Converter.ConsoleApp.Mappers;
    using Sh.Converter.ConsoleApp.Options;

    class Application(IOptions<SettingsOptions> settingsOptions, IOptions<RelationOptions> relationOptions, ILogger<Application> logger) : IHostedService
    {
        private readonly RelationMapper _relationMapper = new(relationOptions.Value.Relations.ToDictionary(x => x.Designator, x => x));
        private readonly SettingsOptions _settings = settingsOptions.Value;
        private readonly RelationOptions _relations = relationOptions.Value;
        private readonly ILogger<Application> _logger = logger;

        bool NodeIdentfier(string line) => line.StartsWith(_relations.Relations.First(x => x.IsRoot).Designator);

        void Start()
        {
            ConsoleHelper.DisplayTxtToXmlAsciiArt();
            
            var document = XmlBuilder.CreateDocument(_settings.OutputDocumentRoot);
            var enumerator = TextReaderHelper.ReadAllLines(NodeIdentfier, _settings.SourceFilePath).GetEnumerator();

            while (enumerator.MoveNext())
            {
                var (nodes, edges) = _relationMapper.BuildFromLines(enumerator.Current, _settings.SourceSeparator);

                document.Root!.Add(XmlBuilder.CreateElements((nodes, edges)));
            }

            ConsoleHelper.WriteInColor(Console.WriteLine, document, ConsoleColor.Cyan, ConsoleColor.Black);
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Conversion failed: {message}", e.Message);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
