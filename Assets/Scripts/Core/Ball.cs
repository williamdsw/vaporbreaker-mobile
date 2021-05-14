using Controllers.Core;
using Effects;
using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        [Header("Required Configuration Parameters")]
        [SerializeField] private EchoEffect echoEffectSpawnerPrefab;
        [SerializeField] private GameObject initialLinePrefab;
        [SerializeField] private GameObject paddleParticlesPrefab;
        [SerializeField] private Paddle paddle;
        [SerializeField] private Sprite defaultBallSprite;
        [SerializeField] private Sprite fireballSprite;

        // State
        private Vector3 paddleToBallPosition;
        private Vector3 remainingPosition;
        private Color32 defaultBallColor;
        private Color32 fireBallColor = Color.white;

        // Cached Components
        private LineRenderer initialLineRenderer;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rigidBody2D;

        // Cached Other Objects
        private Camera mainCamera;

        // || Ball Configuration

        public bool IsBallOnFire { get; set; } = false;
        public float DefaultSpeed { get; set; } = 300f;
        public float MoveSpeed { get; set; } = 300f;
        public float MinMoveSpeed => 200f;
        public float MaxMoveSpeed => 600f;
        public float MinBallLocalScale => 0.5f;
        public float MaxBallLocalScale => 8f;
        public float BallRotationDegree { get; set; } = 20f;
        public float MinBallRotationDegree => 10f;
        public float MaxBallRotationDegree => 90f;

        public Color GetBallColor() => spriteRenderer.color;

        public Sprite GetSprite() => spriteRenderer.sprite;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Default values
            mainCamera = Camera.main;
            DefaultSpeed = MoveSpeed;
            echoEffectSpawnerPrefab.tag = NamesTags.BallEchoTag;

            // Locks only if it's the first ball
            int ballCount = FindObjectsOfType(GetType()).Length;
            if (ballCount == 1)
            {
                echoEffectSpawnerPrefab.gameObject.SetActive(false);

                // Line between ball / pointer
                if (!initialLinePrefab)
                {
                    initialLinePrefab = GameObject.FindGameObjectWithTag(NamesTags.LineBetweenBallPointerTag);
                }

                initialLineRenderer = initialLinePrefab.GetComponent<LineRenderer>();

                // Distance between ball and paddle
                if (!paddle)
                {
                    paddle = FindObjectOfType<Paddle>();
                }

                paddleToBallPosition = (transform.position - paddle.transform.position);
                transform.position = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 0.25f, paddle.transform.position.z);
                DrawLineToMouse();
            }

            ChooseRandomColor();
            ChangeBallSprite(IsBallOnFire);
        }

        private void Update()
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!GameSession.Instance.HasStarted)
                {
                    LockBallToPaddle();
                    CalculateDistanceToMouse();
                    DrawLineToMouse();
                }
                else
                {
                    RotateBall();
                    if (rigidBody2D.velocity == Vector2.zero)
                    {
                        ClampVelocity();
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (GameSession.Instance.HasStarted)
                {
                    // Combo manipulator
                    if (other.gameObject.CompareTag(NamesTags.PaddleTag) || other.gameObject.CompareTag(NamesTags.WallTag))
                    {
                        GameSession.Instance.ResetCombo();
                    }

                    switch (other.gameObject.tag)
                    {
                        // Colision with paddle
                        case "Paddle":
                        {
                            if (other.GetContact(0).normal != Vector2.down)
                            {
                                ClampVelocity();
                                AudioController.Instance.PlaySFX(AudioController.Instance.BlipSound, AudioController.Instance.MaxSFXVolume);
                            }

                            // Case ball is fast
                            if (other.GetContact(0).normal == Vector2.up)
                            {
                                if (MoveSpeed > DefaultSpeed)
                                {
                                    SpawnPaddleDebris(other.GetContact(0).point);
                                }
                            }

                            // Case "Move Blocks" power-up is activated
                            if (GameSession.Instance.CanMoveBlocks)
                            {
                                GameSession.Instance.MoveBlocks(GameSession.Instance.BlockDirection);
                            }

                            break;
                        }

                        // Colision with walls
                        case "Wall":
                        {
                            ClampVelocity();
                            AudioController.Instance.PlaySFX(AudioController.Instance.BlipSound, AudioController.Instance.MaxSFXVolume);
                            break;
                        }

                        case "Breakable": case "Unbreakable": ClampVelocity(); break;
                        default: break;
                    }
                }
            }
        }

        private void LockBallToPaddle()
        {
            Vector3 paddlePosition = new Vector3(paddle.transform.position.x, paddle.transform.position.y, 0f);
            transform.position = new Vector3(paddlePosition.x + paddleToBallPosition.x, paddlePosition.y + 0.35f, transform.position.z);
        }

        private void CalculateDistanceToMouse()
        {
            Vector3 mousePosition = CursorController.Instance.transform.position;
            remainingPosition = (mousePosition - transform.position);
            remainingPosition.z = 0f;
        }

        public void LaunchBall()
        {
            if (remainingPosition.y >= 1f)
            {
                // Game Session parameters
                GameSession.Instance.HasStarted = true;
                GameSession.Instance.TimeToSpawnAnotherBall = GameSession.Instance.TimeToWaitToSpawnAnotherBall;
                GameSession.Instance.StartTimeToSpawnAnotherBall = 5f;
                GameSession.Instance.CanSpawnAnotherBall = true;
                GameSession.Instance.CurrentNumberOfBalls++;

                // Other
                remainingPosition.Normalize();
                rigidBody2D.velocity = (remainingPosition * MoveSpeed * Time.deltaTime);
                initialLineRenderer.enabled = false;
                echoEffectSpawnerPrefab.gameObject.SetActive(true);
                CursorController.Instance.gameObject.SetActive(false);
            }
        }

        // Draws a line beetween the ball and mouse
        private void DrawLineToMouse()
        {
            if (!initialLineRenderer.enabled)
            {
                initialLineRenderer.enabled = true;
            }

            // Positions
            Vector3 pointerPosition = CursorController.Instance.transform.position;
            Vector3 ballPosition = this.transform.position;
            ballPosition.y += 0.2f;
            ballPosition = new Vector3(ballPosition.x, ballPosition.y, ballPosition.z);
            initialLineRenderer.SetPositions(new Vector3[] { ballPosition, pointerPosition });
        }

        private void ClampVelocity()
        {
            Vector2 currentVelocity = rigidBody2D.velocity;
            float x = Mathf.Clamp(Mathf.Abs(currentVelocity.x), 2f, 20f);
            float y = Mathf.Clamp(Mathf.Abs(currentVelocity.y), 2f, 20f);
            currentVelocity.x = (currentVelocity.x > 0 ? x : x * -1);
            currentVelocity.y = (currentVelocity.y > 0 ? y : y * -1);
            rigidBody2D.velocity = currentVelocity;
        }

        private void RotateBall()
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.z += BallRotationDegree;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        public void ChooseRandomColor()
        {
            Color randomColor = Random.ColorHSV(0f, 1f, 0f, 1f, 0.4f, 1f);
            spriteRenderer.color = randomColor;
            defaultBallColor = randomColor;
        }

        // Spawns debris on paddle collision
        private void SpawnPaddleDebris(Vector2 contactPoint)
        {
            // Instantiate and Destroy
            GameObject particles = Instantiate(paddleParticlesPrefab, contactPoint, paddleParticlesPrefab.transform.rotation) as GameObject;
            particles.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.DebrisParentName).transform);
            ParticleSystem debrisParticleSystem = paddleParticlesPrefab.GetComponent<ParticleSystem>();
            float durationLength = (debrisParticleSystem.main.duration + debrisParticleSystem.main.startLifetime.constant);
            Destroy(particles, durationLength);
        }

        public void ChangeBallSprite(bool isOnFire)
        {
            spriteRenderer.sprite = (isOnFire ? fireballSprite : defaultBallSprite);
            spriteRenderer.color = (isOnFire ? fireBallColor : defaultBallColor);
        }

        public void StopBall() => rigidBody2D.velocity = Vector2.zero;
    }
}