using Controllers.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Core
{
    /// <summary>
    /// Release ball | shooter button
    /// </summary>
    public class ReleaseBall : MonoBehaviour, IPointerUpHandler
    {
        public virtual void OnPointerUp(PointerEventData pointerEventData)
        {
            if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!GameSessionController.Instance.HasStarted)
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