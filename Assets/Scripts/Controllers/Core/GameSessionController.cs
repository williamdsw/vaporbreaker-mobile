using Core;
using MVC.BL;
using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for Game Session
    /// </summary>
    public class GameSessionController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Elements")]
        [SerializeField] private Canvas gameCanvas;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Block[] blockPrefabs;
        [SerializeField] private PrefabSpawnerController powerUpSpawnerPrefab;

        [Header("Required UI Texts")]
        [SerializeField] private TextMeshProUGUI ellapsedTimeText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private TextMeshProUGUI comboMultiplierText;
        [SerializeField] private TextMeshProUGUI ballCountdownText;

        // || State

        private Enumerators.GameStates actualGameState = Enumerators.GameStates.GAMEPLAY;
        private Vector2Int minMaxNumberOfRandomBlocks = Vector2Int.zero;
        private bool areBallOnFire = false;
        private float ellapsedTime = 0f;
        private int bestCombo = 0;
        private int currentNumberOfBlocks = 0;
        private int currentScore = 0;
        private int numberOfBlocksDestroyed = 0;
        private int totalNumberOfBlocks = 0;
        private int numberOfRandomBlocks = 0;
        private int songIndex = 0;

        // || Config

        private const float TIME_TO_PUT_OUT_FIRE_BALL = 5f;

        // || Cached 

        private Ball[] balls;
        private CanvasGroup canvasGroup;
        private Dictionary<string, Block> blocksPrefabsDictionary;

        // || Properties

        public static GameSessionController Instance { get; private set; }
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

        public Enumerators.Directions BlockDirection { get; set; } = Enumerators.Directions.None;
        public float StartTimeToSpawnAnotherBall => 10f;
        public float TimeToSpawnAnotherBall { get; set; } = -1f;
        public int CurrentNumberOfBalls { get; set; } = 1;
        public int ComboMultiplier { get; private set; } = 0;
        public int MaxNumberOfBalls => 10;
        public bool CanMoveBlocks { get; set; } = false;
        public bool HasStarted { get; set; } = false;
        public bool CanSpawnAnotherBall { private get; set; } = false;

        private void Awake()
        {
            SetupSingleton();
            FillBlockDictionary();
        }

        private void Start()
        {
            balls = FindObjectsOfType<Ball>();
            canvasGroup = FindObjectOfType<CanvasGroup>();

            Level level = new LevelBL().GetById(GameStatusController.Instance.LevelId);
            Layout layout = JsonConvert.DeserializeObject<Layout>(level.Layout);
            LoadLevelLayout(layout);

            songIndex = UnityEngine.Random.Range(0, AudioController.Instance.AllNotLoopedSongs.Length);
            AudioController.Instance.ChangeMusic(AudioController.Instance.AllNotLoopedSongs[songIndex], false, string.Empty, false, true);

            ResetCombo();
            UpdateUI();
            ChooseRandomPowerUpsBlocks();

            if (layout.HasPrefabSpawner)
            {
                SpawnPrefabSpawner();
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

        /// <summary>
        /// Setup singleton instance
        /// </summary>
        private void SetupSingleton()
        {
            try
            {
                if (FindObjectsOfType(GetType()).Length > 1)
                {
                    gameObject.SetActive(false);
                    DestroyInstance();
                }
                else
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fill block prefab dictionary
        /// </summary>
        private void FillBlockDictionary()
        {
            try
            {
                blocksPrefabsDictionary = new Dictionary<string, Block>();
                foreach (Block item in blockPrefabs)
                {
                    blocksPrefabsDictionary.Add(item.name, item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load level's layout
        /// </summary>
        /// <param name="layout"> Instance of Layout </param>
        private void LoadLevelLayout(Layout layout)
        {
            try
            {
                foreach (Layout.BlockInfo info in layout.Blocks)
                {
                    Block clone = Instantiate(blocksPrefabsDictionary[info.PrefabName], new Vector3(info.Position.X, info.Position.Y, info.Position.Z), Quaternion.identity) as Block;
                    clone.transform.SetParent(FindOrCreateObjectParent(NamesTags.Parents.Blocks).transform);
                    clone.SetColor(new Color32(info.Color.R, info.Color.G, info.Color.B, 255));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update UI Elements
        /// </summary>
        public void UpdateUI()
        {
            try
            {
                currentScoreText.SetText(Formatter.FormatToCurrency(currentScore));
                ellapsedTimeText.text = Formatter.GetEllapsedTimeInHours((int)ellapsedTime);

                if (ComboMultiplier > 1)
                {
                    bestCombo = (ComboMultiplier >= bestCombo ? ComboMultiplier : bestCombo);
                    comboMultiplierText.text = string.Concat("x", ComboMultiplier);
                }
                else
                {
                    comboMultiplierText.SetText(string.Empty);
                }

                if (CanSpawnAnotherBall)
                {
                    int ballCountdown = (int)(StartTimeToSpawnAnotherBall - TimeToSpawnAnotherBall);
                    ballCountdownText.SetText(ballCountdown.ToString("00"));
                }
                else
                {
                    ballCountdownText.SetText(string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add value to current score
        /// </summary>
        /// <param name="value"> Score to be added </param>
        public void AddToScore(int value)
        {
            currentScore += value;
            UpdateUI();
        }

        /// <summary>
        /// Count current number of blocks
        /// </summary>
        public void CountBlocks()
        {
            totalNumberOfBlocks++;
            currentNumberOfBlocks = (totalNumberOfBlocks - numberOfBlocksDestroyed);
            UpdateUI();
        }

        /// <summary>
        /// Update number of blocks destroyed
        /// </summary>
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

        /// <summary>
        /// Shows level complete panel
        /// </summary>
        public void CallLevelComplete()
        {
            ActualGameState = Enumerators.GameStates.LEVEL_COMPLETE;
            LevelCompleteController.Instance.CallLevelComplete(ellapsedTime, bestCombo, currentScore);
        }

        /// <summary>
        /// Increments combo multiplier
        /// </summary>
        public void AddToComboMultiplier()
        {
            ComboMultiplier++;
            UpdateUI();
        }

        /// <summary>
        /// Reset combo multiplier
        /// </summary>
        public void ResetCombo()
        {
            ComboMultiplier = 0;
            UpdateUI();
        }

        /// <summary>
        /// Choose random blocks to spawn power ups
        /// </summary>
        private void ChooseRandomPowerUpsBlocks()
        {
            try
            {
                // Finds
                GameObject[] blocks = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable);
                if (blocks.Length == 0) return;

                // Calculates
                minMaxNumberOfRandomBlocks.x = Mathf.FloorToInt((blocks.Length * (5f / 100f)));
                minMaxNumberOfRandomBlocks.y = Mathf.FloorToInt((blocks.Length * (5f / 100f)));
                numberOfRandomBlocks = UnityEngine.Random.Range(minMaxNumberOfRandomBlocks.x, minMaxNumberOfRandomBlocks.y + 1);

                for (int i = 1; i <= numberOfRandomBlocks; i++)
                {
                    int index = UnityEngine.Random.Range(0, blocks.Length);
                    blocks[index].GetComponent<Block>().CanSpawnPowerUp = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Spawn prefab spawner
        /// </summary>
        private void SpawnPrefabSpawner()
        {
            try
            {
                Instantiate(powerUpSpawnerPrefab, Vector3.zero, Quaternion.identity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Move blocks at direction
        /// </summary>
        /// <param name="direction"> Desired direction </param>
        public void MoveBlocks(Enumerators.Directions direction)
        {
            try
            {
                if (ActualGameState == Enumerators.GameStates.GAMEPLAY)
                {
                    if (CanMoveBlocks)
                    {
                        int numberOfOcorrences = 0;
                        foreach (Block block in FindObjectsOfType<Block>())
                        {
                            if (block.CompareTag(NamesTags.Tags.Unbreakable)) return;

                            switch (direction)
                            {
                                case Enumerators.Directions.Right:
                                {
                                    Vector3 right = new Vector3(block.transform.position.x + 1f, block.transform.position.y, 0f);
                                    MoveBlockAtPosition(block, right, (right.x <= BlockGrid.MaxCoordinatesInXY.x), ref numberOfOcorrences);
                                    break;
                                }

                                case Enumerators.Directions.Left:
                                {
                                    Vector3 left = new Vector3(block.transform.position.x - 1f, block.transform.position.y, 0f);
                                    MoveBlockAtPosition(block, left, (left.x >= BlockGrid.MinCoordinatesInXY.x), ref numberOfOcorrences);
                                    break;
                                }

                                case Enumerators.Directions.Down:
                                {
                                    Vector3 down = new Vector3(block.transform.position.x, block.transform.position.y - 0.5f, 0f);
                                    MoveBlockAtPosition(block, down, down.y >= BlockGrid.MinCoordinatesInXY.y, ref numberOfOcorrences);
                                    break;
                                }

                                case Enumerators.Directions.Up:
                                {
                                    Vector3 up = new Vector3(block.transform.position.x, block.transform.position.y + 0.5f, 0f);
                                    MoveBlockAtPosition(block, up, up.y <= BlockGrid.MaxCoordinatesInXY.y, ref numberOfOcorrences);
                                    break;
                                }

                                default: break;
                            }
                        }

                        CanMoveBlocks = (numberOfOcorrences >= 1);
                        if (CanMoveBlocks)
                        {
                            AudioController.Instance.PlaySFX(AudioController.Instance.HittingWall, AudioController.Instance.MaxSFXVolume);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Move block at position
        /// </summary>
        /// <param name="block"> Block instance </param>
        /// <param name="position"> Desired position </param>
        /// <param name="expression"> Expression to be evaluated </param>
        /// <param name="numberOfOcorrences"> Number of blocks moved </param>
        private void MoveBlockAtPosition(Block block, Vector3 position, bool expression, ref int numberOfOcorrences)
        {
            if (expression && BlockGrid.CheckPosition(position) && BlockGrid.GetBlock(position) == null)
            {
                BlockGrid.RedefineBlock(block.transform.position, null);
                BlockGrid.RedefineBlock(position, block);
                block.transform.position = position;
                numberOfOcorrences++;
            }
        }

        /// <summary>
        /// Fills block grid
        /// </summary>
        private void FillBlockGrid()
        {
            try
            {
                BlockGrid.InitGrid();

                foreach (Block block in FindObjectsOfType<Block>())
                {
                    if (BlockGrid.CheckPosition(block.transform.position) && BlockGrid.GetBlock(block.transform.position) == null)
                    {
                        BlockGrid.RedefineBlock(block.transform.position, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Make all balls to fire balls
        /// </summary>
        public void MakeFireBalls()
        {
            try
            {
                if (ActualGameState == Enumerators.GameStates.GAMEPLAY)
                {
                    if (areBallOnFire)
                    {
                        CancelInvoke(NamesTags.Functions.UndoFireBalls);
                        Invoke(NamesTags.Functions.UndoFireBalls, TIME_TO_PUT_OUT_FIRE_BALL);
                    }
                    else
                    {
                        areBallOnFire = true;

                        foreach (GameObject blockObject in GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable))
                        {
                            Block block = blockObject.GetComponent<Block>();
                            block.MaxHits = 1;
                            block.BoxCollider2D.isTrigger = true;
                        }

                        foreach (Ball ball in FindObjectsOfType<Ball>())
                        {
                            ball.IsOnFire = true;
                            ball.ChangeSprite(true);
                        }

                        AudioController.Instance.PlayME(AudioController.Instance.FireEffect, AudioController.Instance.MaxMEVolume / 2f, true);

                        Invoke(NamesTags.Functions.UndoFireBalls, TIME_TO_PUT_OUT_FIRE_BALL);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Undo fire balls effect
        /// </summary>
        private void UndoFireBalls()
        {
            try
            {
                areBallOnFire = false;

                foreach (GameObject blockObject in GameObject.FindGameObjectsWithTag(NamesTags.Tags.Breakable))
                {
                    Block block = blockObject.GetComponent<Block>();
                    block.MaxHits = block.StartMaxHits;
                    block.BoxCollider2D.isTrigger = false;
                }

                foreach (Ball ball in FindObjectsOfType<Ball>())
                {
                    ball.IsOnFire = false;
                    ball.ChangeSprite(false);
                }

                AudioController.Instance.StopME();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Find or create object parent
        /// </summary>
        /// <param name="parentName"> Desired name </param>
        /// <returns> Instance of Object </returns>
        public GameObject FindOrCreateObjectParent(string parentName)
        {
            GameObject parent = GameObject.Find(parentName);
            if (!parent)
            {
                parent = new GameObject(parentName);
            }

            return parent;
        }

        /// <summary>
        /// Find main camera
        /// </summary>
        private void FindCamera()
        {
            if (gameCanvas.worldCamera) return;
            gameCanvas.worldCamera = Camera.main;
        }

        /// <summary>
        /// Show current ellapsed time
        /// </summary>
        private void ShowEllapsedTime()
        {
            ellapsedTime += Time.deltaTime;
            UpdateUI();
        }

        /// <summary>
        /// Check if can spawn another ball
        /// </summary>
        private void CheckSpawnAnotherBall()
        {
            if (CanSpawnAnotherBall)
            {
                if (CurrentNumberOfBalls >= MaxNumberOfBalls)
                {
                    TimeToSpawnAnotherBall = -1f;
                    return;
                }

                TimeToSpawnAnotherBall += Time.deltaTime;

                if (TimeToSpawnAnotherBall >= StartTimeToSpawnAnotherBall)
                {
                    Ball[] balls = FindObjectsOfType<Ball>();
                    if (balls.Length != 0)
                    {
                        foreach (Ball ball in balls)
                        {
                            if (CurrentNumberOfBalls >= MaxNumberOfBalls) break;

                            Ball newBall = Instantiate(ball, ball.transform.position, Quaternion.identity) as Ball;
                            newBall.Velocity = (ball.Velocity.normalized * -1 * Time.fixedDeltaTime * ball.MoveSpeed);
                            newBall.MoveSpeed = ball.MoveSpeed;
                            if (ball.IsOnFire)
                            {
                                newBall.IsOnFire = true;
                                newBall.ChangeSprite(newBall.IsOnFire);
                            }

                            CurrentNumberOfBalls++;
                        }
                    }

                    TimeToSpawnAnotherBall = -1f;
                }
            }
        }

        /// <summary>
        /// Reset current objects
        /// </summary>
        private void ResetObjects()
        {
            try
            {
                foreach (Ball ball in FindObjectsOfType<Ball>())
                {
                    Destroy(ball.gameObject);
                }

                foreach (Shooter shooter in FindObjectsOfType<Shooter>())
                {
                    Destroy(shooter.gameObject);
                }

                foreach (PowerUp powerUp in FindObjectsOfType<PowerUp>())
                {
                    Destroy(powerUp.gameObject);
                }

                Destroy(FindObjectOfType<Paddle>().gameObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reset current level if last ball touches the Death Zone
        /// </summary>
        public void ResetLevel()
        {
            try
            {
                TimeToSpawnAnotherBall = -1f;
                CurrentNumberOfBalls = 0;
                currentScore = 0;
                bestCombo = 0;
                CanSpawnAnotherBall = false;
                HasStarted = false;
                CanMoveBlocks = false;
                BlockDirection = Enumerators.Directions.None;
                ResetCombo();
                UpdateUI();

                UndoFireBalls();
                ResetObjects();
                SceneManagerController.ReloadScene();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Reset current level
        /// </summary>
        /// <param name="sceneName"> Next scene name </param>
        public void GotoScene(string sceneName)
        {
            AudioController.Instance.StopMusic();
            GameStatusController.Instance.NextSceneName = sceneName;
            GameStatusController.Instance.HasStartedSong = false;
            GameStatusController.Instance.CameFromLevel = true;
            SceneManagerController.CallScene(SceneManagerController.LoadingSceneName);
            DestroyInstance();
        }

        /// <summary>
        /// Destroy this GameObject
        /// </summary>
        public void DestroyInstance() => Destroy(gameObject);
    }
}