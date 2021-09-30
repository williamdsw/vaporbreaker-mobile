using Controllers.Core;
using System;
using UnityEngine;

namespace Core.PowerUps
{
    public class ResetBall : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                Ball[] balls = FindObjectsOfType<Ball>();
                if (balls.Length != 0)
                {
                    foreach (Ball ball in balls)
                    {
                        ball.transform.localScale = Vector3.one;
                        ball.MoveSpeed = ball.DefaultSpeed;
                        ball.Velocity = (ball.Velocity.normalized * Time.fixedDeltaTime * ball.MoveSpeed);
                    }

                    GameSession.Instance.AddToScore(UnityEngine.Random.Range(100, 1000));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}