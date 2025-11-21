using System;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Models
{
    public class World
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MapTile[,] Map { get; private set; }

        public World(int width, int height, Random rand)
        {
            Width = width;
            Height = height;
            Map = new MapTile[width, height];
            GenerateMap(rand);
        }

        private void GenerateMap(Random rand)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    BiomeType biotype;
                    if (x == Width / 2 && y == Height / 2)
                        biotype = BiomeType.CrashSite;
                    else
                        biotype = GenerateBiome(rand);

                    Map[x, y] = new MapTile(biotype, isExplored: x == Width / 2 && y == Height / 2);
                }
            }

            // Place a few special tiles
            Map[1, 1].SpecialDescription = "Ruined alien machinery";
            Map[Width - 2, Height - 2].SpecialDescription = "Abandoned research outpost";
        }

        private BiomeType GenerateBiome(Random rand)
        {
            int roll = rand.Next(100);
            if (roll < 25)
                return BiomeType.Forest;
            else if (roll < 40)
                return BiomeType.Plains;
            else if (roll < 55)
                return BiomeType.Mountain;
            else if (roll < 70)
                return BiomeType.River;
            else if (roll < 85)
                return BiomeType.Ruins;
            else
                return BiomeType.Wasteland;
        }

        public void PrintMap()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var tile = Map[x, y];
                    if (tile.IsExplored)
                        Console.Write(GetBiomeChar(tile.BiomeType));
                    else
                        Console.Write("?");
                }
                Console.WriteLine();
            }
        }

        private char GetBiomeChar(BiomeType type)
        {
            return type switch
            {
                BiomeType.CrashSite => 'C',
                BiomeType.Forest => 'F',
                BiomeType.Plains => 'P',
                BiomeType.Mountain => 'M',
                BiomeType.River => 'R',
                BiomeType.Ruins => 'U',
                BiomeType.Wasteland => 'W',
                _ => '?'
            };
        }
    }
}