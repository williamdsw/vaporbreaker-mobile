using Core;
using TMPro;
using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    public class GameSession : MonoBehaviour
    {
        [Header("Required Elements")]
        [SerializeField] private Canvas gameCanvas;

        [Header("Required UI Texts")]
        [SerializeField] private TextMeshProUGUI ellapsedTimeText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private TextMeshProUGUI comboMultiplierText;
        [SerializeField] private TextMeshProUGUI ballCountdownText;

        // || State
        private int bestCombo = 0;
        private int currentNumberOfBlocks = 0;
        private int currentScore = 0;
        private int numberOfBlocksDestroyed = 0;
        private int totalNumberOfBlocks = 0;
        private float ellapsedTime = 0f;
        [SerializeField] private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;

        // || Config
        private const int MAX_NUMBER_OF_BALLS = 10;

        // Random blocks to spawn another power up block
        [Header("Additional Blocks Configuration")]
        [SerializeField] private bool chooseRandomBlocks = false;
        private Vector2Int minMaxNumberOfRandomBlocks = Vector2Int.zero;
        private int numberOfRandomBlocks = 0;

        // Fire balls
        private bool areBallOnFire = false;
        private float timeToPutOutBallFire = 5f;

        // Spawns another ball configuration

        [Header("Required Ball Config")]
        [SerializeField] private GameObject ballPrefab;

        // Change music in game
        private int songIndex = 0;

        // || Cached 
        private Ball[] balls;
        private CanvasGroup canvasGroup;

        // || Properties

        public static GameSession Instance { get; private set; }
        public Enumerators.GameStates ActualGameState
        {
            get => actualGameState;
            set
            {
                actualGameState = value;
                switch (ActualGameState)
                {
                    case Enumerators.GameStates.LEVEL_COMPLETE:
                        {
                            PauseController.Instance.CanPause = false;
                            canvasGroup.interactable = true;
                            break;
                        }

                    case Enumerators.GameStates.GAMEPLAY:
                        {
                            Time.timeScale = 1f;
                            PauseController.Instance.CanPause = true;
                            canvasGroup.interactable = true;
                            break;
                        }

                    case Enumerators.GameStates.PAUSE:
                        {
                            Time.timeScale = 0f;
                            PauseController.Instance.CanPause = true;
                            canvasGroup.interactable = true;
                            break;
                        }

                    case Enumerators.GameStates.TRANSITION:
                        {
                            PauseController.Instance.CanPause = false;
                            canvasGroup.interactable = false;
                            break;
                        }
                }
            }
        }

        public int CurrentNumberOfBalls { get; set; } = 1;
        public Enumerators.Directions BlockDirection { get; set; } = Enumerators.Directions.None;
        public bool CanMoveBlocks { get; set; } = false;
        public int ComboMultiplier { get; private set; } = 0;
        public bool HasStarted { get; set; } = false;
        public float TimeToWaitToSpawnAnotherBall => -5f;
        public bool CanSpawnAnotherBall { private get; set; } = false;
        public float StartTimeToSpawnAnotherBall { private get; set; } = 0f;
        public float TimeToSpawnAnotherBall { private get; set; } = 0f;
        public int MaxNumberOfBalls => MAX_NUMBER_OF_BALLS;

        private void Awake() => SetupSingleton();

        private void Start()
        {
            UnityUtilities.DisableAnalytics();

            // Find other objects
            balls = FindObjectsOfType<Ball>();
            canvasGroup = FindObjectOfType<CanvasGroup>();

            // Audio Controller
            songIndex = Random.Range(0, AudioController.Instance.AllNotLoopedSongs.Length);
            AudioController.Instance.ChangeMusic(AudioController.Instance.AllNotLoopedSongs[songIndex], false, "", false, true);

            ResetCombo();
            UpdateUI();

            if (chooseRandomBlocks)
            {
                ChooseBlocks();
            }

            FillBlockGrid();
        }

        private void Update()
        {
            FindCamera();

            if (ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (HasStarted)
                {
                    ShowEllapsedTime();
                    CheckSpawnAnotherBall();
                }
            }
        }

        private void SetupSingleton()
        {
            int numberOfInstances = FindObjectsOfType(GetType()).Length;
            if (numberOfInstances > 1)
            {
                gameObject.SetActive(false);
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void UpdateUI()
        {
            currentScoreText.SetText(currentScore.ToString());
            ellapsedTimeText.SetText(Formatter.GetEllapsedTimeInHours((int)ellapsedTime));

            // Combo text
            if (ComboMultiplier > 1)
            {
                bestCombo = (ComboMultiplier >= bestCombo ? ComboMultiplier : bestCombo);
                comboMultiplierText.SetText(string.Concat("x", ComboMultiplier));
            }
            else
            {
                comboMultiplierText.SetText(string.Empty);
            }

            // Ball Countdown
            if (CanSpawnAnotherBall && TimeToSpawnAnotherBall >= 0)
            {
                int ballCountdown = (int)(StartTimeToSpawnAnotherBall - TimeToSpawnAnotherBall);
                ballCountdownText.SetText((ballCountdown > 0 ? ballCountdown.ToString("00") : string.Empty));
            }
            else
            {
                ballCountdownText.SetText(string.Empty);
            }
        }

        public void AddToStore(int value)
        {
            currentScore += value;
            UpdateUI();
        }

        public void CountBlocks()
        {
            totalNumberOfBlocks++;
            currentNumberOfBlocks = (totalNumberOfBlocks - numberOfBlocksDestroyed);
            UpdateUI();
        }

        public void BlockDestroyed()
        {
            numberOfBlocksDestroyed++;
            currentNumberOfBlocks = (totalNumberOfBlocks - numberOfBlocksDestroyed);
            UpdateUI();

            if (currentNumberOfBlocks <= 0 && numberOfBlocksDestroyed == totalNumberOfBlocks)
            {
                CallLevelComplete();
            }
        }

        public void CallLevelComplete()
        {
            ActualGameState = Enumerators.GameStates.LEVEL_COMPLETE;
            LevelCompleteController.Instance.CallLevelComplete(ellapsedTime, bestCombo, currentScore);
        }

        public void AddToComboMultiplier()
        {
            ComboMultiplier++;
            UpdateUI();
        }

        public void ResetCombo()
        {
            ComboMultiplier = 0;
            UpdateUI();
        }

        private void ChooseBlocks()
        {
            // Finds
            GameObject[] blocks = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable);
            if (blocks.Length == 0) return;

            // Calculates
            minMaxNumberOfRandomBlocks.x = Mathf.FloorToInt((blocks.Length * (10f / 100f)));
            minMaxNumberOfRandomBlocks.y = Mathf.FloorToInt((blocks.Length * (20f / 100f)));
            numberOfRandomBlocks = Random.Range(minMaxNumberOfRandomBlocks.x, minMaxNumberOfRandomBlocks.y + 1);

            for (int i = 1; i <= numberOfRandomBlocks; i++)
            {
                int index = Random.Range(0, blocks.Length);
                blocks[index].GetComponent<Block>().CanSpawnPowerUp = true;
            }
        }

        // Moves all blocks in some sirection
        public void MoveBlocks(Enumerators.Directions direction)
        {
            if (ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (CanMoveBlocks)
                {
                    Block[] blocks = FindObjectsOfType<Block>();
                    int numberOfOcorrences = 0;
                    foreach (Block block in blocks)
                    {
                        if (block.CompareTag(NamesTags.Tags.Unbreakable)) return;

                        switch (direction)
                        {
                            case Enumerators.Directions.Right:
                            {
                                Vector3 right = new Vector3(block.transform.position.x + 1f, block.transform.position.y, 0f);
                                MoveBlockAtPosition(block, right, (right.x <= BlockGrid.MaxXYCoordinates.x), ref numberOfOcorrences);
                                break;
                            }

                            case Enumerators.Directions.Left:
                            {
                                Vector3 left = new Vector3(block.transform.position.x - 1f, block.transform.position.y, 0f);
                                MoveBlockAtPosition(block, left, (left.x >= BlockGrid.MinXYCoordinates.x), ref numberOfOcorrences);
                                break;
                            }

                            case Enumerators.Directions.Down:
                            {
                                Vector3 down = new Vector3(block.transform.position.x, block.transform.position.y - 0.5f, 0f);
                                MoveBlockAtPosition(block, down, (down.y >= BlockGrid.MinXYCoordinates.y), ref numberOfOcorrences);
                                break;
                            }

                            case Enumerators.Directions.Up:
                            {
                                Vector3 up = new Vector3(block.transform.position.x, block.transform.position.y + 0.5f, 0f);
                                MoveBlockAtPosition(block, up, (up.y <= BlockGrid.MaxXYCoordinates.y), ref numberOfOcorrences);
                                break;
                            }

                            default: break;
                        }
                    }

                    // Checks and play SFX
                    CanMoveBlocks = (numberOfOcorrences >= 1);
                    if (CanMoveBlocks)
                    {
                        AudioController.Instance.PlaySFX(AudioController.Instance.HittingWall, AudioController.Instance.MaxSFXVolume);
                    }
                }
            }
        }

        private void MoveBlockAtPosition(Block block, Vector3 position, bool expression, ref int numberOfOcorrences)
        {
            if (expression && BlockGrid.GetBlock(position) == null)
            {
                BlockGrid.RedefineBlock(block.transform.position, null);
                BlockGrid.RedefineBlock(position, block);
                block.transform.position = position;
                numberOfOcorrences++;
            }
        }

        // Fills the grid with block or null
        private void FillBlockGrid()
        {
            BlockGrid.Grid.Clear();

            Block[] blocks = FindObjectsOfType<Block>();
            for (float y = BlockGrid.MinXYCoordinates.y; y <= BlockGrid.MaxXYCoordinates.y; y += 0.5f)
            {
                for (float x = BlockGrid.MinXYCoordinates.x; x <= BlockGrid.MaxXYCoordinates.x; x++)
                {
                    Vector3 position = new Vector3(x, y, 0f);

                    foreach (Block block in blocks)
                    {
                        if (block.transform.position == position)
                        {
                            BlockGrid.PutBlock(position, block);
                            break;
                        }
                    }

                    if (!BlockGrid.CheckPosition(position))
                    {
                        BlockGrid.PutBlock(position, null);
                    }
                }
            }
        }

        public void MakeFireBalls()
        {
            if (ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                // Checks
                if (areBallOnFire)
                {
                    CancelInvoke("UndoFireBalls");
                    Invoke("UndoFireBalls", timeToPutOutBallFire);
                }
                else
                {
                    areBallOnFire = true;

                    // Check Blocks
                    GameObject[] blocks = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable);
                    foreach (GameObject blockObject in blocks)
                    {
                        BoxCollider2D blockCollider = blockObject.GetComponent<BoxCollider2D>();
                        blockCollider.isTrigger = true;
                        Block block = blockObject.GetComponent<Block>();
                        block.MaxHits = 1;
                    }

                    // Check Balls
                    Ball[] balls = FindObjectsOfType<Ball>();
                    foreach (Ball ball in balls)
                    {
                        ball.IsBallOnFire = true;
                        ball.ChangeBallSprite(true);
                    }

                    AudioController.Instance.PlayME(AudioController.Instance.FireEffect, AudioController.Instance.MaxMEVolume / 2f, true);

                    // Calls cancel after 'n' seconds
                    Invoke("UndoFireBalls", timeToPutOutBallFire);
                }
            }
        }

        private void UndoFireBalls()
        {
            areBallOnFire = false;

            // Check Blocks
            GameObject[] blocks = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable);
            foreach (GameObject blockObject in blocks)
            {
                BoxCollider2D blockCollider = blockObject.GetComponent<BoxCollider2D>();
                blockCollider.isTrigger = false;
                Block block = blockObject.GetComponent<Block>();
                block.MaxHits = block.StartMaxHits;
            }

            // Check Balls
            Ball[] balls = FindObjectsOfType<Ball>();
            foreach (Ball ball in balls)
            {
                ball.IsBallOnFire = false;
                ball.ChangeBallSprite(false);
            }

            AudioController.Instance.StopME();
        }

        // Finds a GameObject by name or create one
        public GameObject FindOrCreateObjectParent(string parentName)
        {
            GameObject parent = GameObject.Find(parentName);
            if (!parent)
            {
                parent = new GameObject(parentName);
            }

            return parent;
        }

        private void FindCamera()
        {
            if (gameCanvas.worldCamera) return;
            gameCanvas.worldCamera = Camera.main;
        }

        // Increments ellapsed time
        private void ShowEllapsedTime()
        {
            ellapsedTime += Time.deltaTime;
            UpdateUI();
        }

        // Checks if can spawn another ball 
        private void CheckSpawnAnotherBall()
        {
            if (CurrentNumberOfBalls < MaxNumberOfBalls)
            {
                if (CanSpawnAnotherBall)
                {
                    TimeToSpawnAnotherBall += Time.deltaTime;

                    if (TimeToSpawnAnotherBall >= StartTimeToSpawnAnotherBall)
                    {
                        if (ballPrefab)
                        {
                            Ball firstBall = FindObjectOfType<Ball>();

                            if (firstBall)
                            {
                                Rigidbody2D ballRB = firstBall.GetComponent<Rigidbody2D>();
                                Ball newBall = Instantiate(firstBall, firstBall.transform.position, Quaternion.identity) as Ball;
                                Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>();
                                newBallRB.velocity = (ballRB.velocity.normalized * -1 * Time.deltaTime * firstBall.MoveSpeed);
                                newBall.MoveSpeed = firstBall.DefaultSpeed;
                                newBall.IsBallOnFire = firstBall.IsBallOnFire;
                                newBall.ChangeBallSprite(newBall.IsBallOnFire);
                                CurrentNumberOfBalls++;
                            }
                        }

                        TimeToSpawnAnotherBall = TimeToWaitToSpawnAnotherBall;
                        StartTimeToSpawnAnotherBall = 5f;
                    }
                }
            }
        }

        private void ResetObjects()
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            foreach (Ball ball in balls)
            {
                ball.transform.localScale = Vector2.one;
                float defaultSpeed = ball.DefaultSpeed;
                ball.MoveSpeed = defaultSpeed;
            }

            Shooter[] shooters = FindObjectsOfType<Shooter>();
            if (shooters.Length != 0)
            {
                foreach (Shooter shooter in shooters)
                {
                    Destroy(shooter.gameObject);
                }
            }

            PowerUp[] powerUps = FindObjectsOfType<PowerUp>();
            if (powerUps.Length != 0)
            {
                foreach (PowerUp powerUp in powerUps)
                {
                    Destroy(powerUp.gameObject);
                }
            }

            Paddle paddle = FindObjectOfType<Paddle>();
            if (paddle != null)
            {
                paddle.ResetPaddle();
            }
        }

        // Resets level if ball touches 'death zone'
        public void ResetLevel()
        {
            TimeToSpawnAnotherBall = TimeToWaitToSpawnAnotherBall;
            StartTimeToSpawnAnotherBall = 5f;
            CurrentNumberOfBalls = 0;
            currentScore = 0;
            CanSpawnAnotherBall = false;
            HasStarted = false;
            CanMoveBlocks = false;
            BlockDirection = Enumerators.Directions.None;
            ResetCombo();
            UpdateUI();

            // Reset objects
            UndoFireBalls();
            ResetObjects();
            SceneManagerController.ReloadScene();
        }

        // Destroys this
        public void ResetGame(string sceneName)
        {
            AudioController.Instance.StopMusic();
            GameStatusController.Instance.NextSceneName = sceneName;
            GameStatusController.Instance.HasStartedSong = false;
            GameStatusController.Instance.CameFromLevel = true;
            SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
            DestroyInstance();
        }

        public void DestroyInstance() => Destroy(gameObject);
    }
}