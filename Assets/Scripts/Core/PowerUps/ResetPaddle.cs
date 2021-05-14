
using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class ResetPaddle : PowerUp
    {
        protected override void Apply()
        {
            if (paddle)
            {
                paddle.ResetPaddle();
                GameSession.Instance.AddToStore(Random.Range(100, 1000));
            }
        }
    }
}
