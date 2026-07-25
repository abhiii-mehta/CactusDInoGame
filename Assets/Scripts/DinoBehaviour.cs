using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DinoBehavior : MonoBehaviour
{
    public enum DinoType { Runner, SlimeJumper }
    
    [Header("Behavior Type")]
    public DinoType typeOfDino;

    [Header("Runner Settings")]
    public float runSpeed = 3f;

    [Header("Slime Jumper Settings")]
    [Tooltip("The CONSTANT height the slime jumps (matches your single animation).")]
    public float bounceForceY = 10f;
    
    [Tooltip("The slowest and fastest it can move left. This changes how far the arc reaches!")]
    public float minBounceSpeedX = 1.5f;
    public float maxBounceSpeedX = 6f;

    private Rigidbody2D rb;
    private bool isDead = false;
    
    // Tracks how fast it should be moving on its current bounce
    private float currentSpeedX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Give the slime an initial random forward speed when it spawns
        if (typeOfDino == DinoType.SlimeJumper)
        {
            currentSpeedX = Random.Range(minBounceSpeedX, maxBounceSpeedX);
        }
    }

    void Update()
    {
        if (isDead) return;

        if (typeOfDino == DinoType.Runner)
        {
            rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
        }
        else if (typeOfDino == DinoType.SlimeJumper)
        {
            // Apply the current random forward speed, let physics handle gravity
            rb.linearVelocity = new Vector2(-currentSpeedX, rb.linearVelocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (typeOfDino == DinoType.SlimeJumper && collision.gameObject.CompareTag("Ground"))
        {
            // 1. Pick a new random forward distance for this specific bounce
            currentSpeedX = Random.Range(minBounceSpeedX, maxBounceSpeedX);

            // 2. Reset the Y velocity to 0 first, then apply the EXACT SAME upward force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForceY, ForceMode2D.Impulse);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        if (!isDead) Destroy(gameObject);
    }
}