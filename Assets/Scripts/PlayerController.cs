using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float[] speedLevels = new float[7] { 5f, 6f, 7.5f, 9f, 11f, 13f, 15f };
    public float timeToIncreaseSpeed = 10f;
    
    private int currentSpeedIndex = 0;
    private float speedTimer = 0f;

    [Header("Health Settings")]
    public int lives = 3;

    [Header("Combat Settings")]
    public GameObject attackHitbox; 
    public float attackDuration = 0.2f;
    public Animator animator;
    
    [Header("Anti-Spam Settings")]
    public float[] slashCooldowns = new float[7] { 1f, 0.9f, 0.8f, 0.6f, 0.5f, 0.4f, 0.3f };
    
    private float attackTimer = 0f;
    private float currentCooldownTimer = 0f;
    private bool isAttacking = false;

    void Start()
    {
        if (attackHitbox != null) attackHitbox.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
        HandleSpeedIncrease();
        
        if (currentCooldownTimer > 0)
        {
            currentCooldownTimer -= Time.deltaTime;
        }

        HandleInput();
        HandleAttacking();
    }

    private void HandleMovement()
    {
        float currentSpeed = speedLevels[currentSpeedIndex];
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }

    private void HandleSpeedIncrease()
    {
        if (currentSpeedIndex < speedLevels.Length - 1)
        {
            speedTimer += Time.deltaTime;
            if (speedTimer >= timeToIncreaseSpeed)
            {
                currentSpeedIndex++;
                speedTimer = 0f;
            }
        }
    }

    private void HandleInput()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isAttacking && currentCooldownTimer <= 0f)
            {
                StartAttack();
            }
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackDuration;
        currentCooldownTimer = slashCooldowns[currentSpeedIndex];
        
        // Just turn on the large Polygon Collider!
        if (attackHitbox != null) attackHitbox.SetActive(true);
        if (animator != null) animator.SetTrigger("Slash");
    }

    private void HandleAttacking()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
                if (attackHitbox != null) attackHitbox.SetActive(false);
            }
        }
    }

    // --- DAMAGE LOGIC ---
    
    // This triggers when the solid body of the Dino touches the solid body of the Player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dino"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        lives--;
        Debug.Log("Dino killed you! Lives remaining: " + lives);

        if (lives <= 0)
        {
            Debug.Log("GAME OVER! No lives left.");
            // Pauses the game immediately for debugging
            Time.timeScale = 0f; 
        }
    }
    public float GetCurrentSpeed()
    {
        return speedLevels[currentSpeedIndex];
    }
}