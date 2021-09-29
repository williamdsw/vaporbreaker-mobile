using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(EdgeCollider2D))]
    public class Wall : MonoBehaviour
    {
        // || Cached

        private EdgeCollider2D edgeCollider;

        private void Awake() => GetRequiredComponents();

        private void Start() => DefineColliderPoints();

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                edgeCollider = GetComponent<EdgeCollider2D>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set collider points with Screen size
        /// </summary>
        private void DefineColliderPoints()
        {
            try
            {
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
                    lowerLeftCorner, upperLeftCorner, upperRightCorner, lowerRightCorner
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}