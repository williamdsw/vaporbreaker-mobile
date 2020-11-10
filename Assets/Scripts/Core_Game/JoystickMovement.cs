using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // State
    private Vector3 inputDirection;

    // Cached
    private Image backgroundImage;

    //--------------------------------------------------------------------------------//
    // PROPERTIES

    public Vector3 InputDirection { get { return this.inputDirection; }}

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        backgroundImage = this.GetComponent<Image>();
    }

    private void Start () 
    {
        inputDirection = Vector2.zero;
    }

    private void Update ()
    {
        if (!GameSession.Instance) { return; }
        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.TRANSITION)
        {
            inputDirection = Vector2.zero;
        }
    }

    //--------------------------------------------------------------------------------//
    // IDRAGHANDLER, IPOINTERDOWNHANDER

    public virtual void OnDrag (PointerEventData pointerEventData)
    {
        Vector2 position = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (backgroundImage.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out position))
        {
            inputDirection = Camera.main.ScreenToWorldPoint (pointerEventData.position);
        }
    }
  
    public virtual void OnPointerDown (PointerEventData pointerEventData)
    {
        OnDrag (pointerEventData);
    }
}