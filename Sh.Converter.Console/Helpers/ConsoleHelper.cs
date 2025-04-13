namespace Sh.Converter.ConsoleApp.Helpers
{
    static class ConsoleHelper
    {
        public static void WriteInColor<T>(Action<T> action,  T message, ConsoleColor color, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = backgroundColor;

            action(message);
            
            Console.ResetColor();
        }

        public static void DisplayTxtToXmlAsciiArt()
        {
            string[] asciiArt =
            [
                @"  _____  __  __  _____        __  __  __  __   _     ",
                @" |_   _| \ \/ /|_   _|        \ \/ / |  \/  | | |    ",
                @"   | |    >  <   | |     >>>   >  <  | |\/| | | |__  ",
                @"   |_|   /_/\_\  |_|          /_/\_\ |_|  |_| |____| ",
            ];

            Console.WriteLine();

            foreach (var line in asciiArt)
            {
                WriteInColor(Console.WriteLine, line, ConsoleColor.Cyan, ConsoleColor.Black);
            }

            Console.WriteLine();
        }
    }
}
