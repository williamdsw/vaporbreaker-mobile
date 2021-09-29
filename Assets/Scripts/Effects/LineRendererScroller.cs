using System;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererScroller : MonoBehaviour
    {
        // || Config

        private readonly Vector2 movementSpeeds = new Vector2(-5f, 0f);

        // || Cached

        private Material material;
        private LineRenderer lineRenderer;
        private Vector2 offset;

        private void Awake() => GetRequiredComponents();

        private void Start()
        {
            material = lineRenderer.material;
            offset = new Vector2(movementSpeeds.x, movementSpeeds.y);
        }

        private void FixedUpdate() => material.mainTextureOffset += (offset * Time.fixedDeltaTime);

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}