using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Paddle : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Ball ball;
        [SerializeField] private Sprite[] paddleSprites;
        [SerializeField] private JoystickMovement joystickMovement;

        // State
        private int currentPaddleIndex = 1;
        private float defaultSpeed = 0f;
        private float doubleSpeed = 0f;
        private float maxXCoordinate;
        private float minXCoordinate;
        private float moveSpeed = 15f;
        private Vector3 currentDirection;

        // Cached Components
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider2D;

        // Cached Others
        private Camera mainCamera;

        public Sprite GetSprite() => spriteRenderer.sprite;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Find others
            mainCamera = Camera.main;
            if (!joystickMovement)
            {
                GameObject touchpad = GameObject.Find(NamesTags.TouchPadName);
                joystickMovement = touchpad.GetComponent<JoystickMovement>();
            }

            // Default values
            defaultSpeed = moveSpeed;
            doubleSpeed = (moveSpeed * 2f);

            DefineStartPosition();
            DefineBounds();
        }

        private void Update()
        {
            if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
            {
                DefineBounds();
                Move();
                LockPositionToScreen();
            }
        }

        private void DefineStartPosition()
        {
            Vector3 startPosition = new Vector3(Screen.width / 2f, 0, 0);
            startPosition = mainCamera.ScreenToWorldPoint(startPosition);
            transform.position = new Vector3(startPosition.x, this.transform.position.y, transform.position.z);
        }

        public void DefineBounds()
        {
            float minScreenX = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
            float maxScreenX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            float spriteExtentsX = spriteRenderer.bounds.extents.x;
            minXCoordinate = minScreenX + spriteExtentsX;
            maxXCoordinate = maxScreenX - spriteExtentsX;
        }

        private void Move()
        {
            currentDirection = new Vector3(joystickMovement.InputDirection.x, 0, 0);
            transform.position = new Vector3(currentDirection.x, this.transform.position.y, this.transform.position.z);
        }

        private void LockPositionToScreen()
        {
            float xPosition = transform.position.x;
            xPosition = Mathf.Clamp(xPosition, minXCoordinate, maxXCoordinate);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        }

        // Expands or shrink paddle size if index is valid
        public void DefinePaddleSize(bool toExpand)
        {
            currentPaddleIndex = (toExpand ? currentPaddleIndex + 1 : currentPaddleIndex - 1);
            if (currentPaddleIndex < 0)
            {
                currentPaddleIndex++;
                return;
            }
            else if (currentPaddleIndex >= paddleSprites.Length)
            {
                currentPaddleIndex--;
                return;
            }

            // Define properties
            spriteRenderer.sprite = paddleSprites[currentPaddleIndex];
            Destroy(boxCollider2D);
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            DefineBounds();

            // Case have shooter power up
            Shooter shooter = FindObjectOfType<Shooter>();
            if (shooter)
            {
                shooter.DefineCannonsPosition();
            }
        }

        public void ResetPaddle()
        {
            currentPaddleIndex = 1;

            // Define properties
            spriteRenderer.sprite = paddleSprites[currentPaddleIndex];
            Destroy(boxCollider2D);
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            DefineBounds();

            // Case have shooter power up
            Shooter shooter = FindObjectOfType<Shooter>();
            if (shooter)
            {
                shooter.DefineCannonsPosition();
            }
        }

        private void CheckAndFindBall()
        {
            if (ball) return;
            ball = FindObjectOfType<Ball>();
        }
    }
}