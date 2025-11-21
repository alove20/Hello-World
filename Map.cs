using System;

namespace ColonyShipExodus
{
    public class Map
    {
        private int width;
        private int height;
        private BiomeType[,] grid;

        public Map(int w, int h)
        {
            width = w;
            height = h;
            grid = new BiomeType[w, h];
        }

        public void Generate()
        {
            Random random = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = (BiomeType)random.Next(Enum.GetValues(typeof(BiomeType)).Length);
                }
            }
            // Crash site at center
            grid[width / 2, height / 2] = BiomeType.CrashSite;
        }

        // Additional methods for exploration could be added here
    }

    public enum BiomeType
    {
        Forest,
        Mine,
        Ruins,
        Desert,
        CrashSite
    }
}