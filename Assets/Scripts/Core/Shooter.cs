using Controllers.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Core
{
    /// <summary>
    /// Shooter used by the Player
    /// </summary>
    public class Shooter : MonoBehaviour
    {
        // || Inspector References

        [Header("Required References")]
        [SerializeField] private GameObject[] cannons;
        [SerializeField] private Projectile[] projectiles;
        [SerializeField] private Transform[] shootingPoints;

        // || Config

        private const int MAX_NUMBER_OF_PROJECTILES = 30;
        private const float TIME_TO_SELF_DESTRUCT = 3f;

        // || Cached

        private Paddle paddle;
        private List<GameObject> projectilesPool;

        private void Awake()
        {
            projectilesPool = new List<GameObject>();
            paddle = FindObjectOfType<Paddle>();

            CreateProjectilesPool();
            SetCannonsPosition();
            StartCoroutine(SelfDestruct());
        }

        /// <summary>
        /// Create list of projectiles
        /// </summary>
        private void CreateProjectilesPool()
        {
            for (int i = 0; i < MAX_NUMBER_OF_PROJECTILES; i++)
            {
                int index = UnityEngine.Random.Range(0, projectiles.Length);
                Projectile projectile = Instantiate(projectiles[index]);
                projectile.gameObject.SetActive(false);
                projectile.transform.SetParent(GameSessionController.Instance.FindOrCreateObjectParent(NamesTags.Parents.Projectiles).transform);
                projectilesPool.Add(projectile.gameObject);
            }
        }

        /// <summary>
        /// Define cannons position
        /// </summary>
        public void SetCannonsPosition()
        {
            try
            {
                SpriteRenderer cannonSR = cannons[0].GetComponent<SpriteRenderer>();

                float leftCannonX = (paddle.SpriteRenderer.bounds.min.x + cannonSR.bounds.extents.x + 0.03f);
                float rightCannonX = (paddle.SpriteRenderer.bounds.max.x - cannonSR.bounds.extents.x - 0.03f);
                float positionInY = paddle.SpriteRenderer.bounds.max.y;

                cannons[0].transform.position = new Vector3(leftCannonX, positionInY, 1f);
                cannons[1].transform.position = new Vector3(rightCannonX, positionInY, 1f);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Shoot projectiles
        /// </summary>
        public void Shoot()
        {
            foreach (Transform point in shootingPoints)
            {
                foreach (GameObject obj in projectilesPool)
                {
                    if (!obj.activeInHierarchy)
                    {
                        obj.transform.SetPositionAndRotation(point.position, obj.transform.rotation);
                        obj.SetActive(true);
                        obj.GetComponent<Projectile>().Move();
                        break;
                    }
                }
            }

            AudioController.Instance.PlaySFX(AudioController.Instance.LaserPewSound, 0.7f);
        }

        /// <summary>
        /// Self destruct after delay
        /// </summary>
        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(TIME_TO_SELF_DESTRUCT);
            Destroy(gameObject);
        }
    }
}