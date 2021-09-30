using Controllers.Core;
using System;
using UnityEngine;
using Utilities;

namespace Core.PowerUps
{
    public class UnbreakablesToBreakables : PowerUp
    {
        /// <summary>
        /// Applies power up effect
        /// </summary>
        protected override void Apply()
        {
            try
            {
                GameObject[] unbreakables = GameObject.FindGameObjectsWithTag(NamesTags.Tags.Unbreakable);
                if (unbreakables.Length != 0)
                {
                    foreach (GameObject unbreakable in unbreakables)
                    {
                        unbreakable.tag = NamesTags.Tags.Breakable;
                        GameSessionController.Instance.CountBlocks();
                        unbreakable.GetComponent<Animator>().enabled = false;

                        foreach (Transform child in unbreakable.transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    GameSessionController.Instance.AddToScore(UnityEngine.Random.Range(100, 500));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}