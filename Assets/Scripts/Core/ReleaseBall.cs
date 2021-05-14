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
            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (!GameSession.Instance.HasStarted)
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