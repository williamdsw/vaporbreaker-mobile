using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Vector2 minXYCoordinates = Vector2.zero;
        [SerializeField] private Vector2 maxXYCoordinates = Vector2.zero;

        // || Cached
        private SpriteRenderer spriteRenderer;

        // || Properties
        public static CursorController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start() => DefineBounds();

        private void Update()
        {
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                DefineBounds();
                MoveOnTouch();
                LockPositionToScreen();
            }
        }

        // Define bounds to camera
        private void DefineBounds()
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

        private void MoveOnTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (touchPosition.y >= minXYCoordinates.y && touchPosition.y <= maxXYCoordinates.y)
                {
                    this.transform.position = new Vector3(touchPosition.x, touchPosition.y, this.transform.position.z);
                }
            }
        }

        private void LockPositionToScreen()
        {
            float xPosition = transform.position.x;
            float yPosition = transform.position.y;
            xPosition = Mathf.Clamp(xPosition, minXYCoordinates.x, maxXYCoordinates.x);
            yPosition = Mathf.Clamp(yPosition, minXYCoordinates.y, maxXYCoordinates.y);
            transform.position = new Vector3(xPosition, yPosition, transform.position.z);
        }
    }
}