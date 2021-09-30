using Controllers.Core;
using System;

namespace Core.PowerUps
{
    public class FireBall : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                GameSessionController.Instance.MakeFireBalls();
                GameSessionController.Instance.AddToScore(UnityEngine.Random.Range(-10000, 10000));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}