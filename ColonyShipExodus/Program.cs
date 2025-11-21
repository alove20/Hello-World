namespace ColonyShipExodus;

/// <summary>
/// Main entry point for the application.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Colony Ship Exodus";
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(@"
          ___  _  _  _  _  ___   __  _  _    ___ __ __ _  _  ___  ___
         / __)| || || \| |/ __) /  \| || |  / __)\  /  \ || |/ __)| __)
        ( (_-.| \/ | \/ | (_-.| | | | \/ | ( (-. )  \ O | \/ | (_-.| _)
         \___/ \__/  \__/ \___/ \__/ \__/   \___/ \_/\_/ \__/ \___/|___)
        ");
        Console.ResetColor();
        Console.WriteLine("\nYour colony ship has crash-landed. You must lead the survivors to build a new home.");
        Console.WriteLine("Press any key to begin...");
        Console.ReadKey();

        var game = new Game();
        game.Run();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nThank you for playing. Press any key to exit.");
        Console.ResetColor();
        Console.ReadKey();
    }
}