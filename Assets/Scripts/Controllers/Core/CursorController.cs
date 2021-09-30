using System;
using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for In-Game Cursor
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class CursorController : MonoBehaviour
    {
        // || State

        [SerializeField] private Vector2 minXYCoordinates = Vector2.zero;
        [SerializeField] private Vector2 maxXYCoordinates = Vector2.zero;

        // || Cached

        private SpriteRenderer spriteRenderer;

        // || State

        public static CursorController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GetRequiredComponents();
            DefineBounds();
        }

        private void Update()
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                MoveOnTouch();
                LockPositionToScreen();
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Define cursor bounds
        /// </summary>
        private void DefineBounds()
        {
            try
            {
                // Values
                Vector3 zeroPoints = new Vector3(0, 0, 0);
                Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
                float minScreenX = Camera.main.ScreenToWorldPoint(zeroPoints).x;
                float maxScreenX = Camera.main.ScreenToWorldPoint(screenSize).x;
                float minScreenY = Camera.main.ScreenToWorldPoint(zeroPoints).y;
                float maxScreenY = Camera.main.ScreenToWorldPoint(screenSize).y;
                float spriteExtentsX = spriteRenderer.bounds.extents.x;
                float spriteExtentsY = spriteRenderer.bounds.extents.y;

                // Set
                minXYCoordinates.x = (minScreenX + spriteExtentsX);
                maxXYCoordinates.x = (maxScreenX - spriteExtentsX);
                minXYCoordinates.y = (minScreenY + spriteExtentsY) + 4f;
                maxXYCoordinates.y = (maxScreenY - spriteExtentsY) - 1.5f;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Move cursor on screen touch
        /// </summary>
        private void MoveOnTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (touchPosition.y >= minXYCoordinates.y && touchPosition.y <= maxXYCoordinates.y)
                {
                    transform.position = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);
                }
            }
        }

        /// <summary>
        /// Lock cursor position to screen
        /// </summary>
        private void LockPositionToScreen()
        {
            float positionInX = transform.position.x;
            float positionInY = transform.position.y;
            positionInX = Mathf.Clamp(positionInX, minXYCoordinates.x, maxXYCoordinates.x);
            positionInY = Mathf.Clamp(positionInY, minXYCoordinates.y, maxXYCoordinates.y);
            transform.position = new Vector3(positionInX, positionInY, transform.position.z);
        }
    }
}