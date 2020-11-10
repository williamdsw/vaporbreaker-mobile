
public class NamesTags
{
    // Names
    // PARENTS
    private const string BLOCKS_PARENT_NAME = "Blocks";
    private const string BLOCK_SCORE_TEXT_PARENT_NAME = "Block_Score_Text_Container";
    private const string DEBRIS_PARENT_NAME = "Debris_Container";
    private const string ECHOS_PARENT_NAME = "Echos_Container";
    private const string EXPLOSIONS_PARENT_NAME = "Explosions_Container";
    private const string POWER_UPS_PARENT_NAME = "Power_Ups_Container";
    private const string PROJECTILES_PARENT_NAME = "Projectiles_Container";

    // OBJECTS
    private const string TOUCHPAD_NAME = "Touchpad";

    // Tags
    private const string BALL_TAG = "Ball";
    private const string BALL_ECHO_TAG = "Ball_Echo";
    private const string BREAKABLE_BLOCK_TAG = "Breakable";
    private const string LINE_BETWEEN_BALL_POINTER_TAG = "Line_Between_Ball_Pointer";
    private const string PADDLE_TAG = "Paddle";
    private const string POWER_UP_TAG = "PowerUp";
    private const string WALL_TAG = "Wall";
    private const string UNBREAKABLE_BLOCK_TAG = "Unbreakable";

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS
    
    // Names

    // PARENTS
    public static string GetBlocksParentName () { return BLOCKS_PARENT_NAME; }
    public static string GetBlockScoreTextParentName () { return BLOCK_SCORE_TEXT_PARENT_NAME; }
    public static string GetDebrisParentName () { return DEBRIS_PARENT_NAME; }
    public static string GetEchosParentName () { return ECHOS_PARENT_NAME; }
    public static string GetExplosionsParentName () { return EXPLOSIONS_PARENT_NAME; }
    public static string GetProjectilesParentName () { return PROJECTILES_PARENT_NAME; }
    public static string GetPowerUpsParentName () { return POWER_UPS_PARENT_NAME; }

    // OBJECTS
    public static string GetTouchPadName () { return TOUCHPAD_NAME; }

    // TAGS
    public static string GetBallTag () { return BALL_TAG; }
    public static string GetBallEchoTag () { return BALL_ECHO_TAG; }
    public static string GetBreakableBlockTag () { return BREAKABLE_BLOCK_TAG; }
    public static string GetLineBetweenBallPointerTag () { return LINE_BETWEEN_BALL_POINTER_TAG; }
    public static string GetPaddleTag () { return PADDLE_TAG; }
    public static string GetPowerUpTag () { return POWER_UP_TAG; }
    public static string GetUnbreakableBlockTag () { return UNBREAKABLE_BLOCK_TAG; }
    public static string GetWallTag () { return WALL_TAG; }
}