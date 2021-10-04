using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Grid of Blocks
    /// </summary>
    public class BlockGrid
    {
        // || Cached

        private static Dictionary<Vector3, Block> grid;

        // || Properties

        public static Dictionary<Vector3, Block> Grid { get; } = new Dictionary<Vector3, Block>();
        public static Vector2 MinCoordinatesInXY => new Vector2(-4f, -7.5f);
        public static Vector2 MaxCoordinatesInXY => new Vector2(4f, 7.5f);

        /// <summary>
        /// Initialize Grid with positions
        /// </summary>
        public static void InitGrid()
        {
            grid = new Dictionary<Vector3, Block>();
            const float Y_INCREMENT = 0.5f;
            for (float x = MinCoordinatesInXY.x; x <= MaxCoordinatesInXY.x; x++)
            {
                for (float y = MinCoordinatesInXY.y; y <= MaxCoordinatesInXY.y; y += Y_INCREMENT)
                {
                    grid.Add(new Vector3(x, y, 0), null);
                }
            }
        }

        /// <summary>
        /// Put block at position
        /// </summary>
        /// <param name="position"> Desired position </param>
        /// <param name="block"> Desired block </param>
        public static void PutBlock(Vector3 position, Block block) => grid.Add(position, block);

        /// <summary>
        /// Check if position exists
        /// </summary>
        /// <param name="position"> Desired position </param>
        /// <returns> true | false </returns>
        public static bool CheckPosition(Vector3 position) => grid.ContainsKey(position);

        /// <summary>
        /// Get block at position
        /// </summary>
        /// <param name="position"> Desired position </param>
        /// <returns> Instance of Block </returns>
        public static Block GetBlock(Vector3 position) => grid[position];

        /// <summary>
        /// Redefine block at position
        /// </summary>
        /// <param name="position"> Desired position </param>
        /// <param name="block"> Instance of Block </param>
        public static void RedefineBlock(Vector3 position, Block block) => grid[position] = block;
    }
}