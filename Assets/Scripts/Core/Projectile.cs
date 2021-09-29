using UnityEngine;
using Utilities;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        // || Config
        private readonly float yForce = 750f;
        private readonly float timeToSelfDestruct = 2f;

        // || Cached
        private Rigidbody2D rigidBody2D;

        private void Awake() => rigidBody2D = GetComponent<Rigidbody2D>();

        private void OnEnable() => Invoke("HideObject", 2f);

        private void OnDisable() => CancelInvoke();

        private void HideObject() => gameObject.SetActive(false);

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

        public void MoveProjectile()
        {
            Vector2 newForce = new Vector2(0, yForce);
            if (!rigidBody2D)
            {
                rigidBody2D = GetComponent<Rigidbody2D>();
            }

            rigidBody2D.AddForce(newForce);
        }
    }
}