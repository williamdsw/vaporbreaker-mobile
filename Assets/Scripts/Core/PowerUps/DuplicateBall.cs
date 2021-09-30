using Controllers.Core;
using System;
using UnityEngine;

namespace Core.PowerUps
{
    public class DuplicateBall : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                if (GameSession.Instance.CurrentNumberOfBalls >= GameSession.Instance.MaxNumberOfBalls) return;

                Ball[] balls = FindObjectsOfType<Ball>();
                if (balls.Length != 0)
                {
                    foreach (Ball ball in balls)
                    {
                        if (GameSession.Instance.CurrentNumberOfBalls >= GameSession.Instance.MaxNumberOfBalls) break;

                        Ball newBall = Instantiate(ball, ball.transform.position, Quaternion.identity) as Ball;
                        newBall.Velocity = (ball.Velocity.normalized * -1 * Time.fixedDeltaTime * ball.MoveSpeed);
                        newBall.MoveSpeed = ball.MoveSpeed;
                        if (ball.IsOnFire)
                        {
                            newBall.IsOnFire = true;
                            newBall.ChangeSprite(newBall.IsOnFire);
                        }

                        GameSession.Instance.CurrentNumberOfBalls++;
                    }

                    GameSession.Instance.AddToScore(UnityEngine.Random.Range(500, 2500));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}