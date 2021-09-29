using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class SizeBall : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private bool makeBigger = false;

        protected override void Apply() => Define(makeBigger);

        private void Define(bool makeBigger)
        {
            Ball[] balls = FindObjectsOfType<Ball>();
            if (balls.Length != 0)
            {
                foreach (Ball ball in balls)
                {
                    Vector3 newLocalScale = ball.transform.localScale;
                    if (makeBigger)
                    {
                        if (newLocalScale.x < ball.MinMaxLocalScale.y)
                        {
                            newLocalScale *= 2f;
                        }
                    }
                    else
                    {
                        if (newLocalScale.x > ball.MinMaxLocalScale.x)
                        {
                            newLocalScale /= 2f;
                        }
                    }

                    ball.transform.localScale = newLocalScale;
                }

                Vector2Int minMaxScore = new Vector2Int(makeBigger ? 0 : 1000, makeBigger ? 1000 : 5000);
                GameSession.Instance.AddToStore(Random.Range(minMaxScore.x, minMaxScore.y));
            }
        }
    }
}