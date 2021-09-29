using Controllers.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Core
{
    public class Shooter : MonoBehaviour
    {
        // | Inspector References

        [Header("Required References")]
        [SerializeField] private GameObject[] cannons;
        [SerializeField] private Projectile[] projectiles;
        [SerializeField] private Transform[] shootingPoints;

        // || Object pooling
        private List<GameObject> projectilesList = new List<GameObject>();
        private const int MAX_NUMBER_OF_PROJECTILES = 30;

        // || Cached
        private Paddle paddle;

        private void Start()
        {
            paddle = FindObjectOfType<Paddle>();

            CreateProjectilesPool();
            DefineCannonsPosition();
            StartCoroutine(SelfDestruct());
        }

        private void CreateProjectilesPool()
        {
            for (int i = 0; i < MAX_NUMBER_OF_PROJECTILES; i++)
            {
                int index = Random.Range(0, projectiles.Length);
                Projectile projectile = Instantiate(projectiles[index]);
                projectile.gameObject.SetActive(false);
                projectile.transform.SetParent(GameSession.Instance.FindOrCreateObjectParent(NamesTags.Parents.Projectiles).transform);
                projectilesList.Add(projectile.gameObject);
            }
        }

        public void Shoot()
        {
            foreach (Transform point in shootingPoints)
            {
                foreach (GameObject obj in projectilesList)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.transform.SetPositionAndRotation(point.position, obj.transform.rotation);
                        obj.SetActive(true);
                        obj.GetComponent<Projectile>().MoveProjectile();
                        break;
                    }
                }
            }

            AudioController.Instance.PlaySFX(AudioController.Instance.LaserPewSound, 0.7f);
        }

        public void DefineCannonsPosition()
        {
            // Get components
            SpriteRenderer paddleSR = paddle.GetComponent<SpriteRenderer>();
            SpriteRenderer cannonSR = cannons[0].GetComponent<SpriteRenderer>();

            // Calculates
            float leftCannonX = (paddleSR.bounds.min.x + cannonSR.bounds.extents.x + 0.15f);
            float rightCannonX = (paddleSR.bounds.max.x - cannonSR.bounds.extents.x - 0.15f);
            float positionY = paddleSR.bounds.max.y;

            cannons[0].transform.position = new Vector3(leftCannonX, positionY, 1f);
            cannons[1].transform.position = new Vector3(rightCannonX, positionY, 1f);
        }

        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}