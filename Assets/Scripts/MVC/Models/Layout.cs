using System.Collections.Generic;

namespace MVC.Models
{
    /// <summary>
    /// Level layout data
    /// </summary>
    public class Layout
    {
        /// <summary>
        /// Information about block
        /// </summary>
        public class BlockInfo
        {
            /// <summary>
            /// Information about position
            /// </summary>
            public class PositionInfo
            {
                /// <summary>
                /// Position in X
                /// </summary>
                public float X { get; set; }

                /// <summary>
                /// Position in Y
                /// </summary>
                public float Y { get; set; }

                /// <summary>
                /// Position in Z
                /// </summary>
                public float Z { get; set; }
            }

            /// <summary>
            /// Information about color
            /// </summary>
            public class ColorInfo
            {
                // || Properties

                /// <summary>
                /// Value of Red (0, 255)
                /// </summary>
                public byte R { get; set; }

                /// <summary>
                /// Value of Green (0, 255)
                /// </summary>
                public byte G { get; set; }

                /// <summary>
                /// Value of Blue (0, 255)
                /// </summary>
                public byte B { get; set; }
            }

            // || Fields


            // || Properties

            /// <summary>
            /// Prefab name to instantiated
            /// </summary>
            public string PrefabName { get; set; }

            /// <summary>
            /// Block position info
            /// </summary>
            public PositionInfo Position { get; set; }

            /// <summary>
            /// Color position info
            /// </summary>
            public ColorInfo Color { get; set; }
        }

        // || Properties

        /// <summary>
        /// List of Blocks
        /// </summary>
        public List<BlockInfo> Blocks { get; set; } = new List<BlockInfo>();

        /// <summary>
        /// Level has prefab spawner?
        /// </summary>
        public bool HasPrefabSpawner { get; set; } = false;

        /// <summary>
        /// Game Manager Controller can choose random blocks for power ups?
        /// </summary>
        public bool CanChooseRandomBlocks { get; set; } = false;
    }
}