using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core.PowerUps
{
    public class UnbreakablesToBreakables : PowerUp
    {
        protected override void Apply()
        {
            if (GameObject.FindGameObjectsWithTag(NamesTags.UnbreakableBlockTag).Length != 0)
            {
                GameObject[] unbreakables = GameObject.FindGameObjectsWithTag(NamesTags.UnbreakableBlockTag);
                if (unbreakables.Length != 0)
                {
                    foreach (GameObject unbreakable in unbreakables)
                    {
                        unbreakable.tag = NamesTags.BreakableBlockTag;
                        GameSession.Instance.CountBlocks();
                        unbreakable.GetComponent<Animator>().enabled = false;

                        foreach (Transform child in unbreakable.transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    GameSession.Instance.AddToStore(Random.Range(100, 500));
                }
            }
        }
    }
}