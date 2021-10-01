using System;
using UnityEngine;
using Utilities;

namespace Controllers.Core
{
    /// <summary>
    /// Controller for Prefab Spawner
    /// </summary>
    public class PrefabSpawnerController : MonoBehaviour
    {
        // || Inspector References

        [Header("Required Configuration")]
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private int numberOfSpawns = 0;

        // || Config

        private readonly float startTimeToSpawn = 5f;

        // || State

        private bool hasLimitedNumberOfSpawns = false;
        private int currentNumberOfSpawns = 0;
        private float timeToSpawn = 5f;

        private void Awake() => hasLimitedNumberOfSpawns = (numberOfSpawns != 0);

        private void Update() => SpawnPrefab();

        /// <summary>
        /// Spawn prefabs
        /// </summary>
        private void SpawnPrefab()
        {
            try
            {
                if (prefabs.Length == 0) return;

                if (GameSessionController.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
                {
                    if (GameSessionController.Instance.HasStarted)
                    {
                        timeToSpawn -= Time.deltaTime;
                        if (timeToSpawn <= 0)
                        {
                            timeToSpawn = startTimeToSpawn;
                            int chance = UnityEngine.Random.Range(0, 100);
                            int index = (prefabs.Length == 2 ? (chance >= 80 ? 1 : 0) : UnityEngine.Random.Range(0, prefabs.Length));
                            GameObject powerUp = Instantiate(prefabs[index], transform.position, Quaternion.identity) as GameObject;
                            powerUp.transform.SetParent(GameObject.Find(NamesTags.Parents.PowerUps).transform);

                            if (hasLimitedNumberOfSpawns)
                            {
                                currentNumberOfSpawns++;
                                if (currentNumberOfSpawns >= numberOfSpawns)
                                {
                                    Destroy(gameObject);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}