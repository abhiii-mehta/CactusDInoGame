using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(-speed, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If it hits the player, damage them and destroy the flame
        if (other.CompareTag("Player"))
        {
            // If your PlayerController has a public way to take damage, or we trigger it via collision:
            // (Note: Since player collision with Dinos handles damage in PlayerController, 
            // you can also add a simple OnTriggerEnter2D on your PlayerController for "FlameProjectile")
            
            Debug.Log("Flame hit the player!");
            Destroy(gameObject);
        }
        // Parried by sword!
        else if (other.CompareTag("SwordHitbox"))
        {
            Debug.Log("Flame parried by sword!");
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}