using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    public class BlockGrid
    {
        // Config
        // Grid properties
        private static int width = 9;
        private static int height = 13;
        private static Dictionary<Vector3, Block> grid = new Dictionary<Vector3, Block>();

        // Min and Max XY Coordinates
        private static float minXCoordinate = -4f;
        private static float maxXCoordinate = 4f;
        private static float minYCoordinate = 0f;
        private static float maxYCoordinate = 6f;

        public static int Width { get => width; }
        public static int Height { get => height; }
        public static Dictionary<Vector3, Block> Grid { get => grid; }

        public static float MinXCoordinate { get => minXCoordinate; }
        public static float MaxXCoordinate { get => maxXCoordinate; }
        public static float MinYCoordinate { get => minYCoordinate; }
        public static float MaxYCoordinate { get => maxYCoordinate; }

        public static void PutBlock(Vector3 position, Block block)
        {
            grid.Add(position, block);
        }

        public static bool CheckPosition(Vector3 position)
        {
            return grid.ContainsKey(position);
        }

        public static Block GetBlock(Vector3 position)
        {
            return grid[position];
        }

        public static void RedefineBlock(Vector3 position, Block block)
        {
            grid[position] = block;
        }
    }
}