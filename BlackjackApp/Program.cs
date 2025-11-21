using BlackjackApp.Game;

namespace BlackjackApp;

class Program
{
    static void Main(string[] args)
    {
        // Ensure console supports special characters/colors
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        catch
        {
            // Fallback for older terminals
        }

        var game = new BlackjackEngine();
        game.Run();
    }
}