using Controllers.Core;
using UnityEngine;

namespace Core.PowerUps
{
    public class DuplicateBall : PowerUp
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

                GameSession.Instance.AddToStore(Random.Range(500, 2500));
            }
        }
    }
}
