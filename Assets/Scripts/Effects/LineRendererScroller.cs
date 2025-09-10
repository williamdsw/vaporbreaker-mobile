using System;
using UnityEngine;

namespace Effects
{
    /// <summary>
    /// Scroller for Line Renderer
    /// </summary>
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

        private void FixedUpdate() => Scroll();

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

        /// <summary>
        /// Scroll texture
        /// </summary>
        private void Scroll() => material.mainTextureOffset += (offset * Time.fixedDeltaTime);
    }
}