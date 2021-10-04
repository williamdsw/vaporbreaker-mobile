using System;
using UnityEngine;
using Utilities;

namespace Core
{
    /// <summary>
    /// Projectile spawned by Player's shooter
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        // || Config

        private readonly float forceInY = 750f;
        private readonly float timeToSelfDestruct = 2f;

        // || Cached

        private Rigidbody2D rigidBody2D;

        private void Awake() => GetRequiredComponents();

        private void OnEnable() => Invoke("HideObject", 2f);

        private void OnDisable() => CancelInvoke();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(NamesTags.Tags.Breakable) ||
                other.gameObject.CompareTag(NamesTags.Tags.Unbreakable))
            {
                HideObject();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(NamesTags.Tags.Breakable) ||
                other.gameObject.CompareTag(NamesTags.Tags.Unbreakable))
            {
                HideObject();
            }
        }

        /// <summary>
        /// Get required components
        /// </summary>
        public void GetRequiredComponents()
        {
            try
            {
                rigidBody2D = GetComponent<Rigidbody2D>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hides this object
        /// </summary>
        private void HideObject() => gameObject.SetActive(false);

        /// <summary>
        /// Move projectile in Y
        /// </summary>
        public void Move()
        {
            if (!rigidBody2D)
            {
                GetRequiredComponents();
            }

            rigidBody2D.AddForce(new Vector2(0, forceInY));
        }
    }
}