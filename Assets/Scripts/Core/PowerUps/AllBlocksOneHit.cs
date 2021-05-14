using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class AllBlocksOneHit : PowerUp
    {
        protected override void Apply()
        {
            Block[] blocks = FindObjectsOfType<Block>();
            if (blocks.Length != 0)
            {
                foreach (Block block in blocks)
                {
                    block.MaxHits = block.StartMaxHits = 1;
                }

                GameSession.Instance.AddToStore(Random.Range(0, 1000));
            }
        }
    }
}