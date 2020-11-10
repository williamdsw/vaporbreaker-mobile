﻿using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    // Config
    [SerializeField] private GameObject[] prefabs;
    private bool hasLimitedNumberOfSpawns = false;
    [SerializeField] private int numberOfSpawns = 0;
    private int currentNumberOfSpawns = 0;

    // State
    private float startTimeToSpawn = 5f;
    private float timeToSpawn = 5f;
    
    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Start () 
    {
        hasLimitedNumberOfSpawns = (numberOfSpawns != 0);
    }

    private void Update () 
    {
        SpawnPrefab ();
    }

    //--------------------------------------------------------------------------------//
    // HELPER FUNCTIONS

    private void SpawnPrefab ()
    {
        // Cancels
        if (!GameSession.Instance) { return; }
        if (prefabs.Length == 0) { return; }

        if (GameSession.Instance.GetActualGameState () == Enumerators.GameStates.GAMEPLAY)
        {
            if (GameSession.Instance.GetHasStarted ())
            {
                timeToSpawn -= Time.deltaTime;
                if (timeToSpawn <= 0)
                {
                    timeToSpawn = startTimeToSpawn;
                    int chance = Random.Range (0, 100);
                    int index = (prefabs.Length == 2 ? (chance >= 80 ? 1 : 0) : Random.Range (0, prefabs.Length));
                    GameObject powerUp = Instantiate (prefabs[index], this.transform.position, Quaternion.identity) as GameObject;
                    powerUp.transform.SetParent (GameObject.Find (NamesTags.GetPowerUpsParentName ()).transform);

                    // Case have limited number of spawns
                    if (hasLimitedNumberOfSpawns)
                    {
                        currentNumberOfSpawns++;
                        if (currentNumberOfSpawns >= numberOfSpawns)
                        {
                            Destroy (this.gameObject);
                        }
                    }
                }
            }
        }
    }
}