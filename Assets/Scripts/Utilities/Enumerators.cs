
namespace Utilities
{
    public class Enumerators
    {
        public enum GameStates { LEVEL_COMPLETE, GAMEPLAY, PAUSE, SAVE_LOAD, TRANSITION }

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

        public enum Directions { Down, Left, Right, Up, None }
    }
}
