using Controllers.Core;
using System;

namespace Core.PowerUps
{
    public class ResetPaddle : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                if (paddle != null)
                {
                    paddle.ResetPaddle();
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