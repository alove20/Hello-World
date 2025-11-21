using ColonyShipExodus;

Console.Title = "Colony Ship Exodus";

try
{
    var game = new Game();
    game.Run();
}
catch (Exception ex)
{
    Console.WriteLine("\nAn unexpected error occurred and the simulation has collapsed.");
    Console.WriteLine($"ERROR: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}

Console.WriteLine("\nPress any key to exit.");
Console.ReadKey();