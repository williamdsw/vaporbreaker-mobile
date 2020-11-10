using System.Collections.Generic;
using UnityEngine;

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

    //--------------------------------------------------------------------------------//
    // PROPERTIES

    public static int Width { get { return width; }}
    public static int Height { get { return height; }}
    public static Dictionary<Vector3, Block> Grid { get { return grid; }}

    public static float MinXCoordinate { get { return minXCoordinate; }}
    public static float MaxXCoordinate { get { return maxXCoordinate; }}
    public static float MinYCoordinate { get { return minYCoordinate; }}
    public static float MaxYCoordinate { get { return maxYCoordinate; }}

    //--------------------------------------------------------------------------------//
    // HELPER FUNCTIONS

    // Put block at position
    public static void PutBlock (Vector3 position, Block block)
    {
        grid.Add (position, block);
    }

    // Check if positions exists
    public static bool CheckPosition (Vector3 position)
    {
        return grid.ContainsKey (position);
    }

    // Get block at position
    public static Block GetBlock (Vector3 position)
    {
        return grid[position];
    }

    // Reset block at position
    public static void RedefineBlock (Vector3 position, Block block)
    {
        grid[position] = block;
    }
}