using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class FireBall : PowerUp
    {
        protected override void Apply()
        {
            GameSession.Instance.MakeFireBalls();
            GameSession.Instance.AddToStore(Random.Range(-10000, 10000));
        }
    }
}
