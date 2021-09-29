using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class VelocityBall : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private bool moveFaster = false;

        protected override void Apply() => Define(moveFaster);

        private void Define(bool moveFaster)
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    float moveSpeed = ball.MoveSpeed;
                    float ballRotationDegree = ball.RotationDegree;
                    if (moveFaster)
                    {
                        if (moveSpeed < ball.MinMaxMoveSpeed.y)
                        {
                            moveSpeed += 100f;
                        }

                        if (ballRotationDegree < ball.MinMaxRotationDegree.y)
                        {
                            ballRotationDegree *= 2;
                        }
                    }
                    else
                    {
                        if (moveSpeed > ball.MinMaxMoveSpeed.x)
                        {
                            moveSpeed -= 100f;
                        }

                        if (ballRotationDegree > ball.MinMaxRotationDegree.x)
                        {
                            ballRotationDegree /= 2;
                        }
                    }

                    ball.MoveSpeed = moveSpeed;
                    ball.RotationDegree = ballRotationDegree;
                    ballRB.velocity = (ballRB.velocity.normalized * Time.deltaTime * moveSpeed);
                }

                Vector2Int minMaxScore = new Vector2Int(moveFaster ? 5000 : 1000, moveFaster ? 10000 : 5000);
                GameSession.Instance.AddToStore(Random.Range(minMaxScore.x, minMaxScore.y));
            }
        }
    }
}