using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class ResetBall : PowerUp
    {
        protected override void Apply()
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    // Local Scale
                    ball.transform.localScale = Vector3.one;

                    // Movement
                    Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
                    float defaultSpeed = ball.DefaultSpeed;
                    ball.MoveSpeed = defaultSpeed;
                    ballRB.velocity = (ballRB.velocity.normalized * Time.deltaTime * defaultSpeed);
                }

                GameSession.Instance.AddToStore(Random.Range(100, 1000));
            }
        }
    }
}