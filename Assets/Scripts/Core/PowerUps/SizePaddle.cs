using Controllers.Core;
using System;
using UnityEngine;

namespace Core.PowerUps
{
    public class SizePaddle : PowerUp
    {
        [Header("Additional Required Configuration")]
        [SerializeField] private bool toExpand = false;

        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply() => Resize(toExpand);

        /// <summary>
        /// Sizes the paddle
        /// </summary>
        /// <param name="toExpand"> Is to expand the paddle ? </param>
        private void Resize(bool toExpand)
        {
            try
            {
                if (paddle != null)
                {
                    paddle.DefinePaddleSize(toExpand);
                    Vector2Int minMaxScore = new Vector2Int(toExpand ? 100 : 10000, toExpand ? 500 : 30000);
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