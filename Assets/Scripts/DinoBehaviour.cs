using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DinoBehavior : MonoBehaviour
{
    public enum DinoType { Runner, SlimeJumper }
    [Header("Runner Flame Settings")]
    public GameObject flamePrefab;
    public Transform firePoint;
    public float shootDistance = 15f;
    private bool hasFired = false;
    private Transform playerTransform;
    [Header("Behavior Type")]
    public DinoType typeOfDino;

    [Header("Runner Settings")]
    public float runSpeed = 3f;

    [Header("Slime Jumper Settings")]
    public float bounceForceY = 12f;
    public float minBounceSpeedX = 1.5f;
    public float maxBounceSpeedX = 4f;

    [Header("Animation")]
    public Animator animator; // Drag your Animator component here

    private Rigidbody2D rb;
    private bool isDead = false;
    private float currentSpeedX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // FIND AND ASSIGN THE PLAYER TRANSFORM
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        if (typeOfDino == DinoType.SlimeJumper)
        {
            currentSpeedX = Random.Range(minBounceSpeedX, maxBounceSpeedX);
            TriggerJump();
        }
    }

    void Update()
    {
        if (isDead) return;

        if (typeOfDino == DinoType.Runner)
        {
            rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
            if (!hasFired && playerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer <= shootDistance)
                {
                    ShootFlame();
                }
            }
        }
        else if (typeOfDino == DinoType.SlimeJumper)
        {
            rb.linearVelocity = new Vector2(-currentSpeedX, rb.linearVelocity.y);
        }
    }
    private void ShootFlame()
    {
        hasFired = true;
        if (flamePrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            Instantiate(flamePrefab, spawnPos, Quaternion.identity);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (typeOfDino == DinoType.SlimeJumper && collision.gameObject.CompareTag("Ground"))
        {
            currentSpeedX = Random.Range(minBounceSpeedX, maxBounceSpeedX);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForceY, ForceMode2D.Impulse);

            // Trigger the jump animation fresh on every bounce!
            TriggerJump();
        }
    }

    private void TriggerJump()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;

        if (UIManager.instance != null)
        {
            UIManager.instance.AddScore();
        }

        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        if (!isDead) Destroy(gameObject);
    }
}