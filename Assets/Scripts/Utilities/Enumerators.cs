
namespace Utilities
{
    public class Enumerators
    {
        /// <summary>
        /// Available Game States
        /// </summary>
        public enum GameStates
        {
            /// <summary>
            /// Level is completed
            /// </summary>
            LEVEL_COMPLETE,

            /// <summary>
            /// Current Gameplay
            /// </summary>
            GAMEPLAY,

            /// <summary>
            /// Game is paused
            /// </summary>
            PAUSE,

            /// <summary>
            /// Is saving or loading progress
            /// </summary>
            SAVE_LOAD,

            /// <summary>
            /// Is transitioning between fade in / fade out
            /// </summary>
            TRANSITION
        }

        public enum PowerUpsNames
        {
            PowerUp_All_Blocks_1_Hit,
            PowerUp_Ball_Bigger,
            PowerUp_Ball_Faster,
            PowerUp_Ball_Slower,
            PowerUp_Ball_Smaller,
            PowerUp_Duplicate_Ball,
            PowerUp_FireBall,
            PowerUp_Move_Blocks_Right,
            PowerUp_Move_Blocks_Left,
            PowerUp_Move_Blocks_Up,
            PowerUp_Move_Blocks_Down,
            PowerUp_Paddle_Expand,
            PowerUp_Paddle_Shrink,
            PowerUp_Reset_Ball,
            PowerUp_Reset_Paddle,
            PowerUp_Shooter,
            PowerUp_Unbreakables_To_Breakables
        }

        /// <summary>
        /// Available block directions
        /// </summary>
        public enum Directions
        {
            /// <summary>
            /// Vector2.down (0, -1)
            /// </summary>
            Down,

            /// <summary>
            /// Vector2.left (-1, 0)
            /// </summary>
            Left, 
            
            /// <summary>
            /// Vector2.right (1, 0)
            /// </summary>
            Right, 
            /// <summary>
            /// Vector2.up (0, 1)
            /// </summary>
            Up, 
            
            /// <summary>
            /// No valid direction
            /// </summary>
            None
        }
    }
}
