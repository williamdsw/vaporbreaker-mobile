using Controllers.Core;
using System;
using UnityEngine;

namespace Core.PowerUps
{
    public class VelocityBall : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private bool moveFaster = false;

        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply() => Define(moveFaster);

        /// <summary>
        /// Define ball velocity
        /// </summary>
        /// <param name="moveFaster"> Is to move faster ? </param>
        private void Define(bool moveFaster)
        {
            try
            {
                Ball[] balls = FindObjectsOfType<Ball>();
                if (balls.Length != 0)
                {
                    foreach (Ball ball in balls)
                    {
                        float moveSpeed = ball.MoveSpeed;
                        float rotationDegree = ball.MinMaxRotationDegree.x;
                        if (moveFaster)
                        {
                            if (moveSpeed < ball.MinMaxMoveSpeed.y)
                            {
                                moveSpeed += 100f;
                            }

                            if (rotationDegree < ball.MinMaxRotationDegree.y)
                            {
                                rotationDegree *= 2;
                            }
                        }
                        else
                        {
                            if (moveSpeed > ball.MinMaxMoveSpeed.x)
                            {
                                moveSpeed -= 100f;
                            }

                            if (rotationDegree > ball.MinMaxRotationDegree.x)
                            {
                                rotationDegree /= 2;
                            }
                        }

                        ball.MoveSpeed = moveSpeed;
                        ball.RotationDegree = rotationDegree;
                        ball.Velocity = (ball.Velocity.normalized * Time.fixedDeltaTime * ball.MoveSpeed);
                    }

                    Vector2Int minMaxScore = new Vector2Int(moveFaster ? 5000 : 1000, moveFaster ? 10000 : 5000);
                    GameSessionController.Instance.AddToScore(UnityEngine.Random.Range(minMaxScore.x, minMaxScore.y));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}