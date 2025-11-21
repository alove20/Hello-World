namespace ColonyShipExodus;

/// <summary>
/// The main entry point for the Colony Ship Exodus game.
/// </summary>
public class Program
{
    /// <summary>
    /// Initializes and starts the game.
    /// </summary>
    public static void Main(string[] args)
    {
        Console.Title = "Colony Ship Exodus";
        GameManager gameManager = new GameManager();
        gameManager.RunGame();
    }
}