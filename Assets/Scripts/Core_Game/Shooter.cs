using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // Config
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Transform[] shootingPoints;

    // Object pooling
    private List<GameObject> projectilesList = new List<GameObject>();
    private const int MAX_NUMBER_OF_PROJECTILES = 30;

    // Cached
    private Paddle paddle;

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Start ()
    {
        // Find 
        paddle = FindObjectOfType<Paddle>();

        CreateProjectilesPool ();
        DefineCannonsPosition ();
    }

    //--------------------------------------------------------------------------------//

    private void CreateProjectilesPool ()
    {
        // Cancels
        if (!GameSession.Instance) { return; }

        for (int i = 0; i < MAX_NUMBER_OF_PROJECTILES; i++)
        {
            int index = Random.Range (0, projectiles.Length);
            Projectile projectile = Instantiate (projectiles[index]);
            projectile.gameObject.SetActive (false);
            projectile.transform.SetParent (GameSession.Instance.FindOrCreateObjectParent (NamesTags.GetProjectilesParentName ()).transform);
            projectilesList.Add (projectile.gameObject);
        }
    }

    // Shoots projectiles in shooting points
    public void Shoot ()
    {
        // Cancels
        if (!AudioController.Instance) { return; }
        if (shootingPoints.Length == 0) { return; }

        foreach (Transform point in shootingPoints)
        {
            for (int i = 0; i < projectilesList.Count; i++)
            {
                if (!projectilesList[i].activeInHierarchy)
                {
                    projectilesList[i].transform.SetPositionAndRotation (point.position, projectilesList[i].transform.rotation);
                    projectilesList[i].SetActive (true);
                    projectilesList[i].GetComponent<Projectile>().MoveProjectile ();
                    break;
                }
            }
        }

        AudioController.Instance.PlaySFX (AudioController.Instance.LaserPewSound, 0.7f);
    }

    public void DefineCannonsPosition ()
    {
        // Cancels
        if (cannons.Length == 0) { return; }
         
        // Get components
        SpriteRenderer paddleSR = paddle.GetComponent<SpriteRenderer>();
        SpriteRenderer cannonSR = cannons[0].GetComponent<SpriteRenderer>();

        // Calculates
        float leftCannonX = (paddleSR.bounds.min.x + cannonSR.bounds.extents.x + 0.15f);
        float rightCannonX = (paddleSR.bounds.max.x - cannonSR.bounds.extents.x - 0.15f);
        float positionY = paddleSR.bounds.max.y;

        cannons[0].transform.position = new Vector3 (leftCannonX, positionY, 1f);
        cannons[1].transform.position = new Vector3 (rightCannonX, positionY, 1f);
    }
}