using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [Header ("UI Texts")]
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private TextMeshProUGUI ellapsedTimeText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI comboMultiplierText;
    [SerializeField] private TextMeshProUGUI ballCountdownText;

    // State
    private int bestCombo = 0;
    private int comboMultiplier = 0;
    private int currentNumberOfBlocks = 0;
    private int currentNumberOfBalls = 1;
    private int currentScore = 0;
    private int numberOfBlocksDestroyed = 0;
    private int totalNumberOfBlocks = 0;
    private float ellapsedTime = 0f;
    private bool hasStarted = false;
    [SerializeField] private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;
    private static GameSession instance;

    // CONS
    private const int MAX_NUMBER_OF_BALLS = 10;

    // Random blocks to spawn another power up block
    [SerializeField] private bool chooseRandomBlocks = false;
    private int minNumberOfRandomBlocks = 0;
    private int maxNumberOfRandomBlocks = 0;
    private int numberOfRandomBlocks = 0;

    // Blocks movement
    private bool canMoveBlocks = false;
    private string blockDirection = string.Empty;

    // Fire balls
    private bool areBallOnFire = false;
    private float timeToPutOutBallFire = 5f;

    // Spawns another ball configuration
    [SerializeField] private GameObject ballPrefab;
    private bool canSpawnAnotherBall = false;
    private float startTimeToSpawnAnotherBall = 0f;
    private float timeToSpawnAnotherBall = 0f;
    private float timeToWaitToSpawnAnotherBall = -5f;

    // Change music in game
    private int songIndex = 0;
    
    // Cached 
    private Ball[] balls;
    private CanvasGroup canvasGroup;
    private CursorController cursorController;
    private LevelCompleteController levelCompleteController;
    private Paddle paddle;
    private PauseController pauseController;

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public Enumerators.GameStates GetActualGameState () { return actualGameState; }
    public bool GetAreBallOnFire () { return this.areBallOnFire; }
    public string GetBlockDirection () { return this.blockDirection; }
    public bool GetCanMoveBlocks () { return this.canMoveBlocks; }
    public int GetComboMultiplier () { return comboMultiplier; }
    public bool GetHasStarted () { return hasStarted; }
    public int GetMaxNumberOfBalls () { return MAX_NUMBER_OF_BALLS; }
    public float GetTimeToWaitToSpawnAnotherBall () { return this.timeToWaitToSpawnAnotherBall; }

    public void SetAreBallOnFire (bool areBallOnFire) { this.areBallOnFire = areBallOnFire; }
    public void SetBlockDirection (string blocksDirection) { this.blockDirection = blocksDirection; }
    public void SetCanMoveBlocks (bool canMoveBlocks) { this.canMoveBlocks = canMoveBlocks; }
    public void SetCanSpawnAnotherBall (bool canSpawnAnotherBall) { this.canSpawnAnotherBall = canSpawnAnotherBall; }
    public void SetHasStarted (bool hasStarted) { this.hasStarted = hasStarted; }
    public void SetStartTimeToSpawnAnotherBall (float startTimeToSpawnAnotherBall) { this.startTimeToSpawnAnotherBall = startTimeToSpawnAnotherBall; }
    public void SetTimeToSpawnAnotherBall (float timeToSpawnAnotherBall) { this.timeToSpawnAnotherBall = timeToSpawnAnotherBall; }

    public void SetActualGameState (Enumerators.GameStates gameState)
    {
        this.actualGameState = gameState;

        switch (actualGameState)
        {
            case Enumerators.GameStates.LEVEL_COMPLETE:
            {
                pauseController.SetCanPause (false);
                canvasGroup.interactable = true;
                break;
            }

            case Enumerators.GameStates.GAMEPLAY:
            {
                Time.timeScale = 1f;
                pauseController.SetCanPause (true);
                canvasGroup.interactable = true;
                break;
            }

            case Enumerators.GameStates.PAUSE:
            {
                Time.timeScale = 0f;
                pauseController.SetCanPause (true);
                canvasGroup.interactable = true;
                break;
            }

            case Enumerators.GameStates.TRANSITION:
            {
                pauseController.SetCanPause (false);
                canvasGroup.interactable = false;
                break;
            }
        }
    }
    
    //--------------------------------------------------------------------------------//
    // PROPERTIES

    public int CurrentNumberOfBalls { get { return this.currentNumberOfBalls; } set {currentNumberOfBalls = value; }}
    public static GameSession Instance { get { return instance; }}

    //--------------------------------------------------------------------------------//

    private void Awake () 
    {
        SetupSingleton ();
    }

    private void Start () 
    {
        UnityUtilities.DisableAnalytics ();
        
        // Find other objects
        balls = FindObjectsOfType<Ball>();
        canvasGroup = FindObjectOfType<CanvasGroup>();
        cursorController = FindObjectOfType<CursorController>();
        levelCompleteController = FindObjectOfType<LevelCompleteController>();
        pauseController = FindObjectOfType<PauseController>();

        // Audio Controller
        songIndex = Random.Range (0, AudioController.Instance.AllNotLoopedSongs.Length);
        AudioController.Instance.ChangeMusic (AudioController.Instance.AllNotLoopedSongs[songIndex], false, "", false, true);

        ResetCombo ();
        UpdateUI ();

        if (chooseRandomBlocks) { ChooseBlocks (); }
        FillBlockGrid ();
    }

    private void Update ()
    {
        FindCamera ();

        if (actualGameState == Enumerators.GameStates.GAMEPLAY)
        {
            if (hasStarted)
            {
                ShowEllapsedTime ();
                CheckSpawnAnotherBall ();
            }
        }
    }

    //--------------------------------------------------------------------------------//

    // Setups a singleton
    private void SetupSingleton ()
    {
        int numberOfInstances = FindObjectsOfType (GetType ()).Length;
        if (numberOfInstances > 1)
        {
            gameObject.SetActive (false);
            DestroyImmediate (this.gameObject);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad (this.gameObject);
        }
    }

    //--------------------------------------------------------------------------------//

    // Updates Texts in UI
    public void UpdateUI ()
    {
        currentScoreText.SetText (currentScore.ToString ());
        ellapsedTimeText.SetText (Formatter.FormatEllapsedTime ((int) ellapsedTime));
        
        // Combo text
        if (comboMultiplier > 1)
        {
            bestCombo = (comboMultiplier >= bestCombo ? comboMultiplier : bestCombo);
            comboMultiplierText.SetText (string.Concat ("x", comboMultiplier));
        }
        else 
        {
            comboMultiplierText.SetText (string.Empty);
        }

        // Ball Countdown
        if (canSpawnAnotherBall && timeToSpawnAnotherBall >= 0)
        {
            int ballCountdown = (int) (startTimeToSpawnAnotherBall - timeToSpawnAnotherBall);
            ballCountdownText.SetText ((ballCountdown > 0 ? ballCountdown.ToString ("00") : string.Empty));
        }
        else 
        {
            ballCountdownText.SetText (string.Empty);
        }
    }

    // Add points to score
    public void AddToStore (int value)
    {
        currentScore += value;
        UpdateUI ();
    }

    // Count blocks
    public void CountBlocks ()
    {
        totalNumberOfBlocks++;
        currentNumberOfBlocks = (totalNumberOfBlocks - numberOfBlocksDestroyed);
        UpdateUI ();
    }

    // Updates number of blocks
    public void BlockDestroyed ()
    {
        numberOfBlocksDestroyed++;
        currentNumberOfBlocks = (totalNumberOfBlocks - numberOfBlocksDestroyed);
        UpdateUI ();

        if (currentNumberOfBlocks <= 0 && numberOfBlocksDestroyed == totalNumberOfBlocks)
        {
            CallLevelComplete ();
        }
    }

    public void CallLevelComplete ()
    {
        SetActualGameState (Enumerators.GameStates.LEVEL_COMPLETE);
        levelCompleteController.CallLevelComplete (ellapsedTime, bestCombo, currentScore);
    }

    // Increment combo
    public void AddToComboMultiplier ()
    {
        comboMultiplier++;
        UpdateUI ();
    }
    
    // Resets combo
    public void ResetCombo ()
    {
        comboMultiplier = 0;
        UpdateUI ();
    }

    //--------------------------------------------------------------------------------//
    // BLOCKS FUNCTIONS

    // Choose random blocks to instantiate Power Up Blocks
    private void ChooseBlocks ()
    {
        // Finds
        GameObject[] blocks = GameObject.FindGameObjectsWithTag (NamesTags.GetBreakableBlockTag ());
        if (blocks.Length == 0 ) { return; }

        // Calculates
        minNumberOfRandomBlocks = Mathf.FloorToInt ((blocks.Length * (10f / 100f)));
        maxNumberOfRandomBlocks = Mathf.FloorToInt ((blocks.Length * (20f / 100f)));
        numberOfRandomBlocks = Random.Range (minNumberOfRandomBlocks, maxNumberOfRandomBlocks + 1);

        for (int i = 1; i <= numberOfRandomBlocks; i++)
        {
            int index = Random.Range (0, blocks.Length);
            blocks[index].GetComponent<Block>().SetCanSpawnPowerUp (true);
        }
    }

    // Moves all blocks in some sirection
    public void MoveBlocks (string direction)
    {
        if (actualGameState == Enumerators.GameStates.GAMEPLAY)
        {
            if (canMoveBlocks)
            {
                Block[] blocks = FindObjectsOfType<Block>();
                int numberOfOcorrences = 0;
                foreach (Block block in blocks)
                {
                    // Cancels
                    if (block.CompareTag (NamesTags.GetUnbreakableBlockTag ())) { return; }

                    switch (direction)
                    {
                        case "Right":
                        {
                            Vector3 rightPosition = new Vector3 (block.transform.position.x + 1f, block.transform.position.y, 0f);
                            if (rightPosition.x <= BlockGrid.MaxXCoordinate)
                            {
                                if (!BlockGrid.CheckPosition (rightPosition)) { return; }
                                if (!BlockGrid.GetBlock (rightPosition))
                                {
                                    BlockGrid.RedefineBlock (block.transform.position, null);
                                    BlockGrid.RedefineBlock (rightPosition, block);
                                    block.transform.position = rightPosition;
                                    numberOfOcorrences++;
                                }
                            }
                            
                            break;
                        }

                        case "Left":
                        {
                            Vector3 leftPosition = new Vector3 (block.transform.position.x - 1f, block.transform.position.y, 0f);
                            if (leftPosition.x >= BlockGrid.MinXCoordinate)
                            {
                                if (!BlockGrid.CheckPosition (leftPosition)) { return; }
                                if (!BlockGrid.GetBlock (leftPosition))
                                {
                                    BlockGrid.RedefineBlock (block.transform.position, null);
                                    BlockGrid.RedefineBlock (leftPosition, block);
                                    block.transform.position = leftPosition;
                                    numberOfOcorrences++;
                                }
                            }

                            break;
                        }

                        case "Down":
                        {
                            Vector3 downPosition = new Vector3 (block.transform.position.x, block.transform.position.y - 0.5f, 0f);
                            if (downPosition.y >= BlockGrid.MinYCoordinate)
                            {
                                if (!BlockGrid.CheckPosition (downPosition)) { return; }
                                if (!BlockGrid.GetBlock (downPosition))
                                {
                                    BlockGrid.RedefineBlock (block.transform.position, null);
                                    BlockGrid.RedefineBlock (downPosition, block);
                                    block.transform.position = downPosition;
                                    numberOfOcorrences++;
                                }
                            }

                            break;
                        }

                        case "Up":
                        {
                            Vector3 upPosition = new Vector3 (block.transform.position.x, block.transform.position.y + 0.5f, 0f);
                            if (upPosition.y <= BlockGrid.MaxYCoordinate)
                            {
                                if (!BlockGrid.CheckPosition (upPosition)) { return; }
                                if (!BlockGrid.GetBlock (upPosition))
                                {
                                    BlockGrid.RedefineBlock (block.transform.position, null);
                                    BlockGrid.RedefineBlock (upPosition, block);
                                    block.transform.position = upPosition;
                                    numberOfOcorrences++;
                                }
                            }

                            break;
                        }

                        default: { break; }
                    }
                }

                // Checks and play SFX
                canMoveBlocks = (numberOfOcorrences >= 1);
                if (canMoveBlocks)
                {
                    AudioController.Instance.PlaySFX (AudioController.Instance.HittingWall, AudioController.Instance.GetMaxSFXVolume ());
                }
            }
        }
    }

    // Fills the grid with block or null
    private void FillBlockGrid ()
    {
        // Clears the grid
        BlockGrid.Grid.Clear ();
        
        // Finds and populate the grid
        Block[] blocks = FindObjectsOfType<Block>();
        for (float y = BlockGrid.MinYCoordinate; y <= BlockGrid.MaxYCoordinate; y += 0.5f)
        {
            for (float x = BlockGrid.MinXCoordinate; x <= BlockGrid.MaxXCoordinate; x++)
            {
                Vector3 position = new Vector3 (x, y, 0f);

                // Iterates and check positions
                foreach (Block block in blocks)
                {
                    if (block.transform.position == position)
                    {
                        BlockGrid.PutBlock (position, block);
                        break;
                    }
                }

                // Otherwise put null at position
                if (!BlockGrid.CheckPosition (position))
                {
                    BlockGrid.PutBlock (position, null);
                }
            }
        }
    }

    //--------------------------------------------------------------------------------//
    // BALLS FUNCTIONS

    public void MakeFireBalls ()
    {
        if (actualGameState == Enumerators.GameStates.GAMEPLAY)
        {
            // Cancels
            if (!AudioController.Instance) { return; }

            // Checks
            if (areBallOnFire)
            {
                CancelInvoke ("UndoFireBalls");
                Invoke ("UndoFireBalls", timeToPutOutBallFire);
            }
            else 
            {
                areBallOnFire = true;

                // Check Blocks
                GameObject[] blocks = GameObject.FindGameObjectsWithTag (NamesTags.GetBreakableBlockTag ());
                foreach (GameObject blockObject in blocks)
                {
                    BoxCollider2D blockCollider = blockObject.GetComponent<BoxCollider2D>();
                    blockCollider.isTrigger = true;
                    Block block = blockObject.GetComponent<Block>();
                    block.SetMaxHits (1);
                }

                // Check Balls
                Ball[] balls = FindObjectsOfType<Ball>();
                foreach (Ball ball in balls) 
                { 
                    ball.SetIsBallOnFire (true);
                    ball.ChangeBallSprite (true); 
                }

                AudioController.Instance.PlayME (AudioController.Instance.FireEffect, AudioController.Instance.GetMaxMEVolume () / 2f, true);

                // Calls cancel after 'n' seconds
                Invoke ("UndoFireBalls", timeToPutOutBallFire);
            }
        }
    }

    private void UndoFireBalls ()
    {
        // Cancels
        if (!AudioController.Instance) { return; }

        areBallOnFire = false;

        // Check Blocks
        GameObject[] blocks = GameObject.FindGameObjectsWithTag (NamesTags.GetBreakableBlockTag ());
        foreach (GameObject blockObject in blocks)
        {
            BoxCollider2D blockCollider = blockObject.GetComponent<BoxCollider2D>();
            blockCollider.isTrigger = false;
            Block block = blockObject.GetComponent<Block>();
            block.SetMaxHits (block.GetStartMaxHits ());
        }

        // Check Balls
        Ball[] balls = FindObjectsOfType<Ball>();
        foreach (Ball ball in balls) 
        { 
            ball.SetIsBallOnFire (false);
            ball.ChangeBallSprite (false); 
        }

        AudioController.Instance.StopME ();
    }

    //--------------------------------------------------------------------------------//
    // FINDER / CREATER

    // Finds a GameObject by name or create one
    public GameObject FindOrCreateObjectParent (string parentName)
    {
        GameObject parent = GameObject.Find (parentName);
        if (!parent) { parent = new GameObject (parentName); }
        return parent;
    }

    private void FindCamera ()
    {
        if (gameCanvas.worldCamera) { return; }
        gameCanvas.worldCamera = Camera.main;
    }

    //--------------------------------------------------------------------------------//
    // TIME RELATED

    // Increments ellapsed time
    private void ShowEllapsedTime ()
    {
        ellapsedTime += Time.deltaTime;
        UpdateUI ();
    }

    // Checks if can spawn another ball 
    private void CheckSpawnAnotherBall ()
    {
        if (currentNumberOfBalls < MAX_NUMBER_OF_BALLS)
        {
            if (canSpawnAnotherBall)
            {
                timeToSpawnAnotherBall += Time.deltaTime;

                if (timeToSpawnAnotherBall >= startTimeToSpawnAnotherBall)
                {
                    if (ballPrefab)
                    {
                        Ball firstBall = FindObjectOfType<Ball>();

                        if (firstBall)
                        {
                            Rigidbody2D ballRB = firstBall.GetComponent<Rigidbody2D>();
                            Ball newBall = Instantiate (firstBall, firstBall.transform.position, Quaternion.identity) as Ball;
                            Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>();
                            newBallRB.velocity = (ballRB.velocity.normalized * -1 * Time.deltaTime * firstBall.GetMoveSpeed());
                            newBall.SetMoveSpeed (firstBall.GetDefaultSpeed ());
                            newBall.SetIsBallOnFire (firstBall.GetIsBallOnFire ());
                            newBall.ChangeBallSprite (newBall.GetIsBallOnFire ());
                            currentNumberOfBalls++;
                        }
                    }

                    timeToSpawnAnotherBall = timeToWaitToSpawnAnotherBall;
                    startTimeToSpawnAnotherBall = 5f;
                }
            }
        }
    }

    //--------------------------------------------------------------------------------//
    // RESET RELATED

    private void ResetObjects ()
    {
        // Balls
        Ball[] balls = FindObjectsOfType<Ball> ();
        foreach (Ball ball in balls)
        {
            ball.transform.localScale = Vector2.one;
            float defaultSpeed = ball.GetDefaultSpeed ();
            ball.SetMoveSpeed (defaultSpeed);
        }

        // Shooters
        Shooter[] shooters = FindObjectsOfType<Shooter>( );
        if (shooters.Length != 0)
        {
            foreach (Shooter shooter in shooters) { Destroy (shooter.gameObject); }
        }

        // PowerUps
        PowerUp[] powerUps = FindObjectsOfType<PowerUp> ();
        if (powerUps.Length != 0)
        {
            foreach (PowerUp powerUp in powerUps) { Destroy (powerUp.gameObject); }
        }

        // Paddle
        Paddle paddle = FindObjectOfType<Paddle> ();
        if (paddle) { paddle.ResetPaddle (); }
    }

    // Resets level if ball touches 'death zone'
    public void ResetLevel ()
    {
        timeToSpawnAnotherBall = timeToWaitToSpawnAnotherBall;
        startTimeToSpawnAnotherBall = 5f;
        currentNumberOfBalls = 0;
        currentScore = 0;
        canSpawnAnotherBall = false;
        hasStarted = false;
        canMoveBlocks = false;
        blockDirection = string.Empty;
        ResetCombo();
        UpdateUI();

        // Reset objects
        UndoFireBalls ();
        ResetObjects ();
        SceneManagerController.ReloadScene ();
    }

    // Destroys this
    public void ResetGame (string sceneName)
    {
        // Checks and cancels
        if (!AudioController.Instance || !GameStatusController.Instance) { return; }

        AudioController.Instance.StopMusic ();
        GameStatusController.Instance.SetNextSceneName (sceneName);
        GameStatusController.Instance.SetHasStartedSong (false);
        GameStatusController.Instance.SetCameFromLevel (true);
        SceneManagerController.CallScene (SceneManagerController.GetLoadingSceneName ());
        Destroy (this.gameObject);
    }

    public void DestroyInstance ()
    {
        Destroy (this.gameObject);
    }
}