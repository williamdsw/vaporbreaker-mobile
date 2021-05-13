using UnityEngine;

namespace Effects
{
    public class LineRendererScroller : MonoBehaviour
    {
        // || Config
        private readonly Vector2 movementSpeeds = new Vector2(-5f, 0f);

        // || Cached
        private Material material;
        private LineRenderer lineRenderer;
        private Vector2 offset;

        private void Awake() => lineRenderer = GetComponent<LineRenderer>();

        private void Start()
        {
            material = lineRenderer.material;
            offset = new Vector2(movementSpeeds.x, movementSpeeds.y);
        }

        private void FixedUpdate() => material.mainTextureOffset += (offset * Time.fixedDeltaTime);
    }
}