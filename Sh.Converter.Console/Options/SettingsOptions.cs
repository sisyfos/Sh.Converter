namespace Sh.Converter.ConsoleApp.Options
{
    class SettingsOptions
    {
        public const string Settings = nameof(Settings);

        public required string SourceFilePath { get; set; }
        public required string SourceSeparator { get; set; }
        public required string OutputDocumentRoot { get; set; }
    }
}
