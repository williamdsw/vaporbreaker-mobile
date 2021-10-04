using Controllers.Core;
using Effects;
using System;
using UnityEngine;
using Utilities;

namespace Core
{
    /// <summary>
    /// Ball entity
    /// </summary>
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

        // || Config

        private const float MIN_DISTANCE_TO_LAUNCH = 1f;

        // || State

        private Vector3 paddleToBallPosition;
        private Vector3 remainingPosition;
        private Color32 defaultBallColor;
        private Color32 fireBallColor = Color.white;

        // || Cached

        private LineRenderer initialLineRenderer;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rigidBody2D;

        // || Properties

        public Color32 CurrentColor => spriteRenderer.color;
        public Sprite Sprite => spriteRenderer.sprite;
        public bool IsOnFire { get; set; } = false;
        public float DefaultSpeed { get; set; }
        public float MoveSpeed { get; set; } = 300f;
        public float RotationDegree { get; set; } = 20f;
        private Vector2 MinMaxVelocity => new Vector2(2f, 20f);
        public Vector2 MinMaxMoveSpeed => new Vector2(200f, 600f);
        public Vector2 MinMaxLocalScale => new Vector2(0.5f, 8f);
        public Vector2 MinMaxRotationDegree => new Vector2(10f, 90f);
        public Vector2 Velocity { get => rigidBody2D.velocity; set => rigidBody2D.velocity = value; }

        private void Awake() => GetRequiredComponents();

        private void Start()
        {
            name = NamesTags.Tags.Ball;
            DefaultSpeed = MoveSpeed;
            echoEffectSpawnerPrefab.tag = NamesTags.Tags.BallEcho;

            FirstBallConfiguration();
            ChooseRandomColor();
            ChangeSprite(IsOnFire);
        }

        private void FixedUpdate()
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!GameSessionController.Instance.HasStarted)
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
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (GameSessionController.Instance.HasStarted)
                {
                    if (other.gameObject.CompareTag(NamesTags.Tags.Paddle) ||
                        other.gameObject.CompareTag(NamesTags.Tags.Wall))
                    {
                        GameSessionController.Instance.ResetCombo();
                    }

                    switch (other.gameObject.tag)
                    {
                        case "Paddle":
                            if (other.GetContact(0).normal != Vector2.down)
                            {
                                ClampVelocity();
                                AudioController.Instance.PlaySFX(AudioController.Instance.BlipSound, AudioController.Instance.MaxSFXVolume);
                            }

                            if (other.GetContact(0).normal == Vector2.up)
                            {
                                if (MoveSpeed > DefaultSpeed)
                                {
                                    SpawnPaddleDebris(other.GetContact(0).point);
                                }
                            }

                            if (GameSessionController.Instance.CanMoveBlocks)
                            {
                                GameSessionController.Instance.MoveBlocks(GameSessionController.Instance.BlockDirection);
                            }

                            break;

                        case "Wall":
                            ClampVelocity();
                            AudioController.Instance.PlaySFX(AudioController.Instance.BlipSound, AudioController.Instance.MaxSFXVolume);
                            break;

                        case "Breakable": case "Unbreakable": ClampVelocity(); break;
                        default: break;
                    }
                }
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                rigidBody2D = GetComponent<Rigidbody2D>();
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Configuration for the first ball of the game
        /// </summary>
        private void FirstBallConfiguration()
        {
            if (FindObjectsOfType(GetType()).Length == 1)
            {
                echoEffectSpawnerPrefab.gameObject.SetActive(false);
                initialLinePrefab = GameObject.FindGameObjectWithTag(NamesTags.Tags.LineBetweenBallPointer);
                initialLineRenderer = initialLinePrefab.GetComponent<LineRenderer>();

                paddleToBallPosition = (transform.position - paddle.transform.position);
                Vector3 destination = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 0.35f, paddle.transform.position.z);
                rigidBody2D.MovePosition((Vector2)destination);
                DrawLineToMouse();
            }
        }

        /// <summary>
        /// Locks the ball to paddle movement
        /// </summary>
        private void LockBallToPaddle()
        {
            rigidBody2D.MovePosition(new Vector2(paddle.transform.position.x + paddleToBallPosition.x, paddle.transform.position.y + 0.35f));
        }

        /// <summary>
        /// Calculate distance from ball to mouse
        /// </summary>
        private void CalculateDistanceToMouse()
        {
            Vector3 mousePosition = CursorController.Instance.transform.position;
            remainingPosition = (mousePosition - transform.position);
            remainingPosition.z = 0f;
        }

        /// <summary>
        /// Launch ball at cursor position
        /// </summary>
        public void LaunchBall()
        {
            try
            {
                if (remainingPosition.y >= MIN_DISTANCE_TO_LAUNCH)
                {
                    // Game Session parameters
                    GameSessionController.Instance.HasStarted = true;
                    GameSessionController.Instance.TimeToSpawnAnotherBall = 0f;
                    GameSessionController.Instance.CanSpawnAnotherBall = true;
                    GameSessionController.Instance.CurrentNumberOfBalls++;

                    // Other
                    rigidBody2D.velocity = (remainingPosition.normalized * MoveSpeed * Time.fixedDeltaTime);
                    initialLineRenderer.enabled = false;
                    echoEffectSpawnerPrefab.gameObject.SetActive(true);
                    CursorController.Instance.gameObject.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Draws a line between ball and cursor
        /// </summary>
        private void DrawLineToMouse()
        {
            if (!initialLineRenderer.enabled)
            {
                initialLineRenderer.enabled = true;
            }

            Vector3 pointerPosition = CursorController.Instance.transform.position;
            Vector3 ballPosition = transform.position;
            ballPosition.y += 0.2f;
            ballPosition = new Vector3(ballPosition.x, ballPosition.y, ballPosition.z);
            initialLineRenderer.SetPositions(new Vector3[] { ballPosition, pointerPosition });
        }

        /// <summary>
        /// Clamps ball velocity
        /// </summary>
        private void ClampVelocity()
        {
            Vector2 currentVelocity = Velocity;
            float x = Mathf.Clamp(Mathf.Abs(currentVelocity.x), MinMaxVelocity.x, MinMaxVelocity.y);
            float y = Mathf.Clamp(Mathf.Abs(currentVelocity.y), MinMaxVelocity.x, MinMaxVelocity.y);
            currentVelocity.x = (currentVelocity.x > 0 ? x : x * -1);
            currentVelocity.y = (currentVelocity.y > 0 ? y : y * -1);
            Velocity = currentVelocity;
        }

        /// <summary>
        /// Rotates the ball
        /// </summary>
        private void RotateBall()
        {
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.z += RotationDegree;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        /// <summary>
        /// Chooses a random color
        /// </summary>
        private void ChooseRandomColor() => defaultBallColor = spriteRenderer.color = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0.4f, 1f);

        /// <summary>
        /// Spawns debris on paddle collision
        /// </summary>
        /// <param name="contactPoint"> Exact contact point </param>
        private void SpawnPaddleDebris(Vector2 contactPoint)
        {
            try
            {
                GameObject particles = Instantiate(paddleParticlesPrefab, contactPoint, paddleParticlesPrefab.transform.rotation) as GameObject;
                particles.transform.SetParent(GameSessionController.Instance.FindOrCreateObjectParent(NamesTags.Parents.Debris).transform);
                ParticleSystem debrisParticleSystem = paddleParticlesPrefab.GetComponent<ParticleSystem>();
                float durationLength = (debrisParticleSystem.main.duration + debrisParticleSystem.main.startLifetime.constant);
                Destroy(particles, durationLength);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Changes ball sprite
        /// </summary>
        /// <param name="isOnFire"> Is on fire by power up ? </param>
        public void ChangeSprite(bool isOnFire)
        {
            spriteRenderer.sprite = (isOnFire ? fireballSprite : defaultBallSprite);
            spriteRenderer.color = (isOnFire ? fireBallColor : defaultBallColor);
        }
    }
}