using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class BlockGrid
    {
        // || Properties

        public static Dictionary<Vector3, Block> Grid { get; } = new Dictionary<Vector3, Block>();
        public static Vector2 MinXYCoordinates { get; } = new Vector2(-4f, 0f);
        public static Vector2 MaxXYCoordinates { get; } = new Vector2(4f, 6f);

        public static void PutBlock(Vector3 position, Block block) => Grid.Add(position, block);

        public static bool CheckPosition(Vector3 position) => Grid.ContainsKey(position);

        public static Block GetBlock(Vector3 position) => Grid[position];

        public static void RedefineBlock(Vector3 position, Block block) => Grid[position] = block;
    }
}