using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Core.PowerUps
{
    public class UnbreakablesToBreakables : PowerUp
    {
        protected override void Apply()
        {
            if (GameObject.FindGameObjectsWithTag(NamesTags.Tags.Unbreakable).Length != 0)
            {
                GameObject[] unbreakables = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Unbreakable);
                if (unbreakables.Length != 0)
                {
                    foreach (GameObject unbreakable in unbreakables)
                    {
                        unbreakable.tag = NamesTags.Tags.Breakable;
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