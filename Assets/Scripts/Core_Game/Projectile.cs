using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float yForce = 750f;
    private float timeToSelfDestruct = 2f;
    private Rigidbody2D rigidBody2D;

    private void Awake()
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Invoke("HideObject", 2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(NamesTags.GetBreakableBlockTag()) ||
            other.gameObject.CompareTag(NamesTags.GetUnbreakableBlockTag()))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(NamesTags.GetBreakableBlockTag()) ||
            other.gameObject.CompareTag(NamesTags.GetUnbreakableBlockTag()))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void HideObject()
    {
        this.gameObject.SetActive(false);
    }

    public void MoveProjectile()
    {
        Vector2 newForce = new Vector2(0, yForce);
        if (!rigidBody2D)
        {
            rigidBody2D = this.GetComponent<Rigidbody2D>();
        }

        rigidBody2D.AddForce(newForce);
    }
}