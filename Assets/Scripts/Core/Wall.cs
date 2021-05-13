using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class Wall : MonoBehaviour
    {
        private EdgeCollider2D edgeCollider;

        private void Awake() => edgeCollider = GetComponent<EdgeCollider2D>();

        private void Start() => DefineColliderPoints();

        public void DefineColliderPoints()
        {
            Camera mainCamera = Camera.main;

            // Size Vectors
            Vector2 screenHeight = new Vector2(0, Screen.height);
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenWidth = new Vector2(Screen.width, 0);

            // Converts
            Vector2 lowerLeftCorner = Camera.main.ScreenToWorldPoint(Vector2.zero);
            Vector2 upperLeftCorner = Camera.main.ScreenToWorldPoint(screenHeight);
            Vector2 upperRightCorner = Camera.main.ScreenToWorldPoint(screenSize);
            Vector2 lowerRightCorner = Camera.main.ScreenToWorldPoint(screenWidth);

            edgeCollider.points = new Vector2[]
            {
                lowerLeftCorner , upperLeftCorner , upperRightCorner  , lowerRightCorner
            };
        }
    }
}