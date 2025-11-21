namespace ColonyShipExodus;

/// <summary>
/// The main entry point for the Colony Ship Exodus application.
/// </summary>
public class Program
{
    /// <summary>
    /// Initializes and starts the game.
    /// </summary>
    public static void Main(string[] args)
    {
        Console.Title = "Colony Ship Exodus";
        var game = new Game();
        game.Run();
    }
}