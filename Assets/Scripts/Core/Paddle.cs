using Controllers.Core;
using System;
using UnityEngine;
using Utilities;

namespace Core
{
    /// <summary>
    /// Paddle, the player
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Paddle : MonoBehaviour
    {
        // || Inspector References

        [Header("Configuration")]
        [SerializeField] private Sprite[] paddleSprites;

        // || State

        private int currentPaddleIndex = 1;
        private Vector2 minMaxCoordinatesInX = Vector2.zero;
        private Vector3 currentDirection;
        private float moveSpeed = 40f;

        // || Cached

        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rigidBody2D;

        // || Properties

        public SpriteRenderer SpriteRenderer { get; private set; }

        private void Awake()
        {
            GetRequiredComponents();
            DefineStartPosition();
            DefineBounds();
        }

        private void FixedUpdate()
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                Move();
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                boxCollider2D = GetComponent<BoxCollider2D>();
                rigidBody2D = GetComponent<Rigidbody2D>();
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Define start position of Paddle
        /// </summary>
        private void DefineStartPosition()
        {
            Vector3 startPosition = new Vector3(Screen.width / 2f, 0, 0);
            startPosition = Camera.main.ScreenToWorldPoint(startPosition);
            rigidBody2D.MovePosition((Vector2)startPosition);
        }

        /// <summary>
        /// Define bounds for Paddle
        /// </summary>
        public void DefineBounds()
        {
            float minScreenX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
            float maxScreenX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            float spriteExtentsX = SpriteRenderer.bounds.extents.x;
            minMaxCoordinatesInX = new Vector2(minScreenX + spriteExtentsX, maxScreenX - spriteExtentsX);
        }

        /// <summary>
        /// Move paddle by Input or Autoplay
        /// </summary>
        private void Move()
        {
            float x = GameSessionController.Instance.JoystickMovement.InputDirection.x * moveSpeed * Time.fixedDeltaTime;
            x = Mathf.Clamp(x, minMaxCoordinatesInX.x, minMaxCoordinatesInX.y);
            rigidBody2D.MovePosition(new Vector2(x, 0));
        }

        /// <summary>
        /// Expands or shrink paddle
        /// </summary>
        /// <param name="toExpand"> Is to expand the paddle size ? </param>
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

            ResetProperties();
        }

        /// <summary>
        /// Reset paddle
        /// </summary>
        public void ResetPaddle()
        {
            currentPaddleIndex = 1;
            ResetProperties();
        }

        /// <summary>
        /// Reset paddle's properties
        /// </summary>
        private void ResetProperties()
        {
            try
            {
                SpriteRenderer.sprite = paddleSprites[currentPaddleIndex];
                Destroy(boxCollider2D);
                boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
                DefineBounds();

                Shooter shooter = FindObjectOfType<Shooter>();
                if (shooter != null)
                {
                    shooter.SetCannonsPosition();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}