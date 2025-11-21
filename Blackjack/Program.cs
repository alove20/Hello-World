using Blackjack.Core;

// Set console encoding to UTF8 to display card suits (♥ ♦ ♣ ♠)
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Title for the console window
Console.Title = "Blackjack .NET 9";

try
{
    GameEngine game = new GameEngine();
    game.Run();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("A critical error occurred: " + ex.Message);
    Console.ResetColor();
}