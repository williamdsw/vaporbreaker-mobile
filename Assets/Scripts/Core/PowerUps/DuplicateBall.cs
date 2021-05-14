using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class DuplicateBall : PowerUp
    {
        protected override void Apply()
        {
            if (GameSession.Instance.CurrentNumberOfBalls >= GameSession.Instance.MaxNumberOfBalls) return;

            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    Ball newBall = Instantiate(ball, ball.transform.position, Quaternion.identity) as Ball;
                    Rigidbody2D newBallRB = newBall.GetComponent<Rigidbody2D>();
                    newBallRB.velocity = (ballRB.velocity.normalized * -1 * Time.deltaTime * ball.MoveSpeed);
                    newBall.MoveSpeed = ball.MoveSpeed;
                    if (ball.IsBallOnFire)
                    {
                        newBall.IsBallOnFire = true;
                        newBall.ChangeBallSprite(newBall.IsBallOnFire);
                    }
                }

                GameSession.Instance.AddToStore(Random.Range(500, 2500));
            }
        }
    }
}