using UnityEngine;

public class LineRendererScroller : MonoBehaviour
{
    // Config
    private float xMovementSpeed = - 5f;
    private float yMovementSpeed = 0f;

    // Cached
    private Material material;
    private LineRenderer lineRenderer;
    private Vector2 offset;

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        // Components
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    private void Start ()
    {
        material = lineRenderer.material;
        offset = new Vector2 (xMovementSpeed, yMovementSpeed);
    }

    private void FixedUpdate ()
    {
        material.mainTextureOffset += (offset * Time.fixedDeltaTime);
    }
}