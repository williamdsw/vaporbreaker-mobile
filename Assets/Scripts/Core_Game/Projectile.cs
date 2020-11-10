using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Config params
    private float yForce = 750f;
    private float timeToSelfDestruct = 2f;

    // Cached
    private Rigidbody2D rigidBody2D;

    //--------------------------------------------------------------------------------//
    // MONOBEHAVIOUR

    private void Awake () 
    {
        rigidBody2D = this.GetComponent<Rigidbody2D>();
    }

    private void OnEnable () 
    {
        Invoke ("HideObject", 2f);
    }

    private void OnDisable () 
    {
        CancelInvoke ();
    }

    // Collision with blocks
    private void OnCollisionEnter2D (Collision2D other) 
    {
        if (other.gameObject.CompareTag (NamesTags.GetBreakableBlockTag ()) || 
            other.gameObject.CompareTag (NamesTags.GetUnbreakableBlockTag ()))
        {
            this.gameObject.SetActive (false);
        }
    }

    // Collision with blocks
    private void OnTriggerEnter2D (Collider2D other) 
    {
        if (other.gameObject.CompareTag (NamesTags.GetBreakableBlockTag ()) || 
            other.gameObject.CompareTag (NamesTags.GetUnbreakableBlockTag ()))
        {
            this.gameObject.SetActive (false);
        }
    }

    //--------------------------------------------------------------------------------//

    private void HideObject ()
    {
        this.gameObject.SetActive (false);
    }

    // Add random force in Y to projectile
    public void MoveProjectile ()
    {
        Vector2 newForce = new Vector2 (0, yForce);
        if (!rigidBody2D) { rigidBody2D = this.GetComponent<Rigidbody2D>(); }
        rigidBody2D.AddForce (newForce);
    }
}