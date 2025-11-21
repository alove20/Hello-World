using Blackjack.Src;

namespace Blackjack;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var game = new GameEngine();
            game.Run();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}