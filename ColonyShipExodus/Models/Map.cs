using System;
using System.Collections.Generic;

namespace ColonyShipExodus.Models
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public MapTile[,] Tiles { get; }

        private static readonly Random rand = new Random();

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new MapTile[height, width];
            GenerateMap();
        }

        private void GenerateMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var biome = GenerateBiome(x, y);
                    Tiles[y, x] = new MapTile(x, y, biome);
                    GenerateResourcesForTile(Tiles[y, x]);
                }
            }

            // Place CrashSite at center
            int centerX = Width / 2;
            int centerY = Height / 2;
            Tiles[centerY, centerX].Biome = BiomeType.CrashSite;
            Tiles[centerY, centerX].Explored = true;
        }

        private BiomeType GenerateBiome(int x, int y)
        {
            int r = rand.Next(100);
            if (r < 25) return BiomeType.Plains;
            else if (r < 45) return BiomeType.Forest;
            else if (r < 60) return BiomeType.Ruins;
            else if (r < 75) return BiomeType.Hills;
            else if (r < 82) return BiomeType.Water;
            else return BiomeType.Rocky;
        }

        private void GenerateResourcesForTile(MapTile tile)
        {
            switch (tile.Biome)
            {
                case BiomeType.Plains:
                    tile.ResourceDeposits[ResourceType.Food] = rand.Next(2, 6);
                    tile.ResourceDeposits[ResourceType.Water] = rand.Next(1, 4);
                    break;
                case BiomeType.Forest:
                    tile.ResourceDeposits[ResourceType.Food] = rand.Next(3, 7);
                    tile.ResourceDeposits[ResourceType.BuildingMaterials] = rand.Next(1, 5);
                    break;
                case BiomeType.Ruins:
                    tile.ResourceDeposits[ResourceType.BuildingMaterials] = rand.Next(3, 8);
                    tile.ResourceDeposits[ResourceType.Medicine] = rand.Next(0, 3);
                    break;
                case BiomeType.Hills:
                    tile.ResourceDeposits[ResourceType.BuildingMaterials] = rand.Next(2, 7);
                    tile.ResourceDeposits[ResourceType.Energy] = rand.Next(0, 2);
                    break;
                case BiomeType.Water:
                    tile.ResourceDeposits[ResourceType.Water] = rand.Next(5, 13);
                    break;
                case BiomeType.Rocky:
                    tile.ResourceDeposits[ResourceType.BuildingMaterials] = rand.Next(1, 5);
                    break;
                case BiomeType.CrashSite:
                    tile.ResourceDeposits[ResourceType.Food] = 6;
                    tile.ResourceDeposits[ResourceType.Water] = 7;
                    break;
            }
        }

        public MapTile GetCrashSiteTile()
        {
            int centerX = Width / 2;
            int centerY = Height / 2;
            return Tiles[centerY, centerX];
        }

        public List<MapTile> GetExplorableAdjacentTiles()
        {
            // Only allow exploring one tile next to crash site for now
            var crash = GetCrashSiteTile();
            var adj = new List<MapTile>();
            var dirs = new[] { (-1,0),(1,0),(0,-1),(0,1) };
            foreach (var (dx, dy) in dirs)
            {
                int nx = crash.X + dx;
                int ny = crash.Y + dy;
                if (nx >= 0 && ny >= 0 && nx < Width && ny < Height)
                    adj.Add(Tiles[ny, nx]);
            }
            return adj;
        }
    }
}