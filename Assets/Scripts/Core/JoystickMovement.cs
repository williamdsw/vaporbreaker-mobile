using Controllers.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(Image))]
    public class JoystickMovement : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        private Image backgroundImage;

        public Vector3 InputDirection { get; private set; }

        private void Awake() => backgroundImage = GetComponent<Image>();

        private void Start() => InputDirection = Vector2.zero;

        private void Update()
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.TRANSITION)
            {
                InputDirection = Vector2.zero;
            }
        }

        public virtual void OnDrag(PointerEventData pointerEventData)
        {
            Vector2 position = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out position))
            {
                InputDirection = Camera.main.ScreenToWorldPoint(pointerEventData.position);
            }
        }

        public virtual void OnPointerDown(PointerEventData pointerEventData) => OnDrag(pointerEventData);
    }
}