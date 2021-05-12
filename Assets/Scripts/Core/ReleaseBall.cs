using Controllers.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Core
{
    public class ReleaseBall : MonoBehaviour, IPointerUpHandler
    {
        public virtual void OnPointerUp(PointerEventData pointerEventData)
        {
            // Cancels
            if (!GameSession.Instance) return;

            if (GameSession.Instance.GetActualGameState() == Enumerators.GameStates.GAMEPLAY)
            {
                if (!GameSession.Instance.GetHasStarted())
                {
                    Ball ball = FindObjectOfType<Ball>();
                    if (ball)
                    {
                        ball.LaunchBall();
                    }
                }
                else
                {
                    Shooter shooter = FindObjectOfType<Shooter>();
                    if (shooter)
                    {
                        shooter.Shoot();
                    }
                }
            }
        }
    }
}