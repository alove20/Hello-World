namespace ColonyShipExodus.Models;

public class Map
{
    public MapTile[,] Grid { get; }
    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);

    public (int X, int Y) CrashSiteLocation { get; set; }

    public Map(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentException("Map dimensions must be positive.");
        }
        Grid = new MapTile[width, height];
    }
}