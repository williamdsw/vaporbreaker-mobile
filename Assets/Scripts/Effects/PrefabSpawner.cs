﻿using Controllers.Core;
using UnityEngine;
using Utilities;

namespace Effects
{
    public class PrefabSpawner : MonoBehaviour
    {
        [Header("Required Configuration")]
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private int numberOfSpawns = 0;

        // || Config
        private readonly float startTimeToSpawn = 5f;

        // State
        private bool hasLimitedNumberOfSpawns = false;
        private int currentNumberOfSpawns = 0;
        private float timeToSpawn = 5f;

        private void Start() => hasLimitedNumberOfSpawns = (numberOfSpawns != 0);

        private void Update() => SpawnPrefab();

        private void SpawnPrefab()
        {
            if (prefabs.Length == 0) return;

            if (GameSession.Instance.ActualGameState == Enumerators.GameStates.GAMEPLAY)
            {
                if (GameSession.Instance.HasStarted)
                {
                    timeToSpawn -= Time.deltaTime;
                    if (timeToSpawn <= 0)
                    {
                        timeToSpawn = startTimeToSpawn;
                        int chance = Random.Range(0, 100);
                        int index = (prefabs.Length == 2 ? (chance >= 80 ? 1 : 0) : Random.Range(0, prefabs.Length));
                        GameObject powerUp = Instantiate(prefabs[index], this.transform.position, Quaternion.identity) as GameObject;
                        powerUp.transform.SetParent(GameObject.Find(NamesTags.PowerUpsParentName).transform);

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
    }
}