using Controllers.Core;
using System;

namespace Core.PowerUps
{
    public class AllBlocksOneHit : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                Block[] blocks = FindObjectsOfType<Block>();
                if (blocks.Length != 0)
                {
                    foreach (Block block in blocks)
                    {
                        block.MaxHits = block.StartMaxHits = 1;
                    }

                    GameSession.Instance.AddToScore(UnityEngine.Random.Range(0, 1000));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}