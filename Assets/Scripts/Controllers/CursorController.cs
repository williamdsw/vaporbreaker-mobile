using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Animator))]
public class CursorController : MonoBehaviour
{
    // Config params
    [SerializeField] private float minXCoordinate = 0f;
    [SerializeField] private float maxXCoordinate = 0f;
    [SerializeField] private float minYCoordinate = 0f;
    [SerializeField] private float maxYCoordinate = 0f;

    // Cached
    private SpriteRenderer spriteRenderer;

    //--------------------------------------------------------------------------------//
    // GETTERS / SETTERS

    public SpriteRenderer GetSpriteRenderer () { return spriteRenderer; }

    //--------------------------------------------------------------------------------//

    private void Awake () 
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Start () 
    {
        DefineBounds ();
    }

    private void Update () 
    {
        // Cancels
        if (!GameSession.Instance) { return; }

        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.GAMEPLAY)
        {
            DefineBounds ();
            MoveOnTouch ();
            LockPositionToScreen();
        }
    }

    //--------------------------------------------------------------------------------//

    // Define bounds to camera
    private void DefineBounds ()
    {
        // Cancels
        if (!spriteRenderer) { return; }

        // Values
        Vector3 zeroPoints = new Vector3 (0, 0, 0);
        Vector3 screenSize = new Vector3 (Screen.width, Screen.height, 0);
        float minScreenX = Camera.main.ScreenToWorldPoint (zeroPoints).x;
        float maxScreenX = Camera.main.ScreenToWorldPoint (screenSize).x;
        float minScreenY = Camera.main.ScreenToWorldPoint (zeroPoints).y;
        float maxScreenY = Camera.main.ScreenToWorldPoint (screenSize).y;
        float spriteExtentsX = spriteRenderer.bounds.extents.x;
        float spriteExtentsY = spriteRenderer.bounds.extents.y;

        // Set
        minXCoordinate = (minScreenX + spriteExtentsX);
        maxXCoordinate = (maxScreenX - spriteExtentsX);
        minYCoordinate = (minScreenY + spriteExtentsY) + 4f;
        maxYCoordinate = (maxScreenY - spriteExtentsY) - 1.5f;
    }

    private void MoveOnTouch()
    {
        if (Input.touchCount > 0)
        {
            // Get and check Touch's position
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (touchPosition.y >= minYCoordinate && touchPosition.y <= maxYCoordinate)
            {
                this.transform.position = new Vector3 (touchPosition.x, touchPosition.y, this.transform.position.z);
            }
        }
    }

    // Locks cursor to screen
    private void LockPositionToScreen ()
    {
        float xPosition = transform.position.x;
        float yPosition = transform.position.y;
        xPosition = Mathf.Clamp (xPosition, minYCoordinate, maxXCoordinate);
        yPosition = Mathf.Clamp (yPosition, minYCoordinate, maxYCoordinate);
        transform.position = new Vector3 (xPosition, yPosition, transform.position.z);
    }

    public void DestroyInstance ()
    {
        Destroy (this.gameObject);
    }
}