using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header ("Configuration Parameters")]
    [SerializeField] private EchoEffect echoEffectSpawnerPrefab;
    [SerializeField] private GameObject initialLinePrefab;
    [SerializeField] private GameObject paddleParticlesPrefab;
    [SerializeField] private Paddle paddle;
    [SerializeField] private Sprite defaultBallSprite;
    [SerializeField] private Sprite fireballSprite;

    // Speed config
    private float defaultSpeed = 300f;
    private float maxMoveSpeed = 600f;
    private float minMoveSpeed = 200f;
    private float moveSpeed = 300f;

    // Scale config
    private float maxBallLocalScale = 8f;
    private float minBallLocalScale = 0.5f;

    // Rotation config
    private float ballRotationDegree = 20f;
    private float maxBallRotationDegree = 90f;
    private float minBallRotationDegree = 10f;

    // State
    private Vector3 paddleToBallPosition;
    private Vector3 remainingPosition;
    private Color32 defaultBallColor;
    private Color32 fireBallColor = Color.white;
    private bool isBallOnFire = false;

    // Cached Components
    private LineRenderer initialLineRenderer;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // Cached Other Objects
    private Camera mainCamera;
    private CursorController cursorController;

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public Color GetBallColor () { return spriteRenderer.color; }
    public float GetBallRotationDegree () { return ballRotationDegree; }
    public float GetDefaultSpeed () { return defaultSpeed; }
    public bool GetIsBallOnFire () { return isBallOnFire; }
    public float GetMaxBallLocalScale () { return maxBallLocalScale; }
    public float GetMaxBallRotationDegree () { return maxBallRotationDegree; }
    public float GetMaxMoveSpeed () { return maxMoveSpeed; }
    public float GetMinBallLocalScale () { return minBallLocalScale; }
    public float GetMinBallRotationDegree () { return minBallRotationDegree; }
    public float GetMinMoveSpeed () { return minMoveSpeed; }
    public float GetMoveSpeed () { return moveSpeed; }
    public Sprite GetSprite () { return spriteRenderer.sprite; }

    public void SetBallRotationDegree (float ballRotationDegree) { this.ballRotationDegree = ballRotationDegree; }
    public void SetIsBallOnFire (bool isBallOnFire) { this.isBallOnFire = isBallOnFire; }
    public void SetMoveSpeed (float moveSpeed) { this.moveSpeed = moveSpeed; }

    //--------------------------------------------------------------------------------//

    private void Awake () 
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    private void Start () 
    {
        // Other Objects
        cursorController = FindObjectOfType<CursorController>();

        // Default values
        mainCamera = Camera.main;
        defaultSpeed = moveSpeed;
        echoEffectSpawnerPrefab.tag = NamesTags.GetBallEchoTag ();

        // Locks only if it's the first ball
        int ballCount = FindObjectsOfType (GetType ()).Length;
        if (ballCount == 1)
        {
            echoEffectSpawnerPrefab.gameObject.SetActive (false);
            // Line between ball / pointer
            if (!initialLinePrefab) { initialLinePrefab = GameObject.FindGameObjectWithTag (NamesTags.GetLineBetweenBallPointerTag ()); }
            initialLineRenderer = initialLinePrefab.GetComponent<LineRenderer>();
        
            // Distance between ball and paddle
            if (!paddle) { paddle = FindObjectOfType<Paddle>(); }
            paddleToBallPosition = this.transform.position - paddle.transform.position;
            this.transform.position = new Vector3 (paddle.transform.position.x, paddle.transform.position.y + 0.25f, paddle.transform.position.z);
            DrawLineToMouse ();
        }

        ChooseRandomColor ();
        ChangeBallSprite (this.isBallOnFire);
    }

    private void Update () 
    {
        // Cancels
        if (!GameSession.Instance) { return; }

        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.GAMEPLAY)
        {
            if (!GameSession.Instance.GetHasStarted ())
            {
                LockBallToPaddle ();
                CalculateDistanceToMouse ();
                DrawLineToMouse ();
            }
            else
            {
                RotateBall ();
                if (rigidBody2D.velocity == Vector2.zero) { ClampVelocity (); }
            }
        }
    }

    private void OnCollisionEnter2D (Collision2D other) 
    {
        // Cancels
        if (!GameSession.Instance || !AudioController.Instance) { return; }

        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.GAMEPLAY)
        {
            if (GameSession.Instance.GetHasStarted ())
            {
                // Combo manipulator
                if (other.gameObject.CompareTag (NamesTags.GetPaddleTag ()) || other.gameObject.CompareTag (NamesTags.GetWallTag ()))
                {
                    GameSession.Instance.ResetCombo ();
                }

                switch (other.gameObject.tag)
                {
                    // Colision with paddle
                    case "Paddle":
                    {
                        if (other.GetContact (0).normal != Vector2.down)
                        {
                            ClampVelocity ();
                            AudioController.Instance.PlaySFX (AudioController.Instance.BlipSound, AudioController.Instance.GetMaxSFXVolume ());
                        }

                        // Case ball is fast
                        if (other.GetContact (0).normal == Vector2.up)
                        {
                            if (moveSpeed > defaultSpeed)
                            {
                                SpawnPaddleDebris (other.GetContact (0).point);
                            }
                        }

                        // Case "Move Blocks" power-up is activated
                        if (GameSession.Instance.GetCanMoveBlocks ())
                        {
                            GameSession.Instance.MoveBlocks (GameSession.Instance.GetBlockDirection ());
                        }

                        break;
                    }

                    // Colision with walls
                    case "Wall":
                    {
                        ClampVelocity ();
                        AudioController.Instance.PlaySFX (AudioController.Instance.BlipSound, AudioController.Instance.GetMaxSFXVolume ());
                        break;
                    }

                    case "Breakable": case "Unbreakable":
                    {
                        ClampVelocity ();
                        break;
                    }

                    default: { break; }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------//

    // Locks the ball
    private void LockBallToPaddle ()
    {
        Vector3 paddlePosition = new Vector3 (paddle.transform.position.x, paddle.transform.position.y, 0f);
        this.transform.position = new Vector3 (paddlePosition.x + paddleToBallPosition.x, paddlePosition.y + 0.35f, transform.position.z);
    }

    // Calculate distance to pointer (mouse)
    private void CalculateDistanceToMouse ()
    {
        Vector3 mousePosition = cursorController.transform.position;
        remainingPosition = mousePosition - this.transform.position;
        remainingPosition.z = 0f;
    }

    // Launches the ball on pointer's direction
    public void LaunchBall ()
    {
        if (remainingPosition.y >= 1f)
        {
            // Game Session parameters
            GameSession.Instance.SetHasStarted (true);
            GameSession.Instance.SetTimeToSpawnAnotherBall (GameSession.Instance.GetTimeToWaitToSpawnAnotherBall ());
            GameSession.Instance.SetStartTimeToSpawnAnotherBall (5f);
            GameSession.Instance.SetCanSpawnAnotherBall (true);
            GameSession.Instance.CurrentNumberOfBalls++;

            // Other
            remainingPosition.Normalize ();
            rigidBody2D.velocity = (remainingPosition * moveSpeed * Time.deltaTime);
            initialLineRenderer.enabled = false;
            echoEffectSpawnerPrefab.gameObject.SetActive (true);
            cursorController.gameObject.SetActive (false);
        }
    }

    //--------------------------------------------------------------------------------//
    // LINE RENDERERS

    // Draws a line beetween the ball and mouse
    private void DrawLineToMouse ()
    {
        if (!initialLineRenderer.enabled) { initialLineRenderer.enabled = true; }

        // Positions
        Vector3 pointerPosition = cursorController.transform.position;
        Vector3 ballPosition = this.transform.position;
        ballPosition.y += 0.2f;
        ballPosition = new Vector3 (ballPosition.x, ballPosition.y, ballPosition.z);
        initialLineRenderer.SetPositions (new Vector3 [] { ballPosition, pointerPosition});
    }

    //--------------------------------------------------------------------------------//

    // COLLISION VELOCITY ISSUES

    private void ClampVelocity ()
    {
        Vector2 currentVelocity = rigidBody2D.velocity;
        float x = Mathf.Clamp (Mathf.Abs (currentVelocity.x), 2f, 20f);
        float y = Mathf.Clamp (Mathf.Abs (currentVelocity.y), 2f, 20f);
        currentVelocity.x = (currentVelocity.x > 0 ? x : x * -1);
        currentVelocity.y = (currentVelocity.y > 0 ? y : y * -1);
        rigidBody2D.velocity = currentVelocity;
    }

    private void RotateBall ()
    {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.z += ballRotationDegree;
        transform.rotation = Quaternion.Euler (eulerAngles);
    }

    //--------------------------------------------------------------------------------//
    // VFX

    // Chooses an random color
    public void ChooseRandomColor ()
    {
        if (!spriteRenderer) { return; }
        Color randomColor = Random.ColorHSV (0f, 1f, 0f, 1f, 0.4f, 1f);
        spriteRenderer.color = randomColor;
        defaultBallColor = randomColor;
    }

    // Spawns debris on paddle collision
    private void SpawnPaddleDebris (Vector2 contactPoint)
    {
        if (paddleParticlesPrefab)
        {
            // Instantiate and Destroy
            GameObject particles = Instantiate (paddleParticlesPrefab, contactPoint, paddleParticlesPrefab.transform.rotation) as GameObject;
            particles.transform.SetParent (GameSession.Instance.FindOrCreateObjectParent (NamesTags.GetDebrisParentName ()).transform);
            ParticleSystem debrisParticleSystem = paddleParticlesPrefab.GetComponent<ParticleSystem>();
            float durationLength = debrisParticleSystem.main.duration + debrisParticleSystem.main.startLifetime.constant;
            Destroy (particles, durationLength);
        }
    }

    public void ChangeBallSprite (bool isOnFire)
    {
        if (!defaultBallSprite || !fireballSprite) { return; }
        spriteRenderer.sprite = (isOnFire ? fireballSprite : defaultBallSprite);
        spriteRenderer.color = (isOnFire ? fireBallColor : defaultBallColor);
    }

    public void StopBall ()
    {
        if (!rigidBody2D) { return; }
        rigidBody2D.velocity = Vector2.zero;
    }
}