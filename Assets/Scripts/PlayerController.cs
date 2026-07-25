using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSequencePlayer audioSequencePlayer;
    [Header("Movement Settings")]
    public float[] speedLevels = new float[7] { 5f, 6f, 7.5f, 9f, 11f, 13f, 15f };
    public float timeToIncreaseSpeed = 10f;
    
    private int currentSpeedIndex = 0;
    private float speedTimer = 0f;

    [Header("Health Settings")]
    public int lives = 3;
    [Tooltip("How long the player is invincible after getting hit (in seconds).")]
    public float gracePeriodDuration = 1f;

    [Header("Combat Settings")]
    public GameObject attackHitbox; 
    public float attackDuration = 0.2f;
    public Animator animator;
    
    [Header("Anti-Spam Settings")]
    public float[] slashCooldowns = new float[7] { 1f, 0.9f, 0.8f, 0.6f, 0.5f, 0.4f, 0.3f };
    
    private float attackTimer = 0f;
    private float currentCooldownTimer = 0f;
    private bool isAttacking = false;

    // Grace period trackers
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

   void Start()
    {
        if (attackHitbox != null) attackHitbox.SetActive(false);
    }
    public void SetSpeedTier(int tierIndex)
    {
        // Clamp it safely between 0 and the max speed level index (up to 6)
        currentSpeedIndex = Mathf.Clamp(tierIndex, 0, speedLevels.Length - 1);
        Debug.Log("Music shifted! Speed Tier is now: " + currentSpeedIndex);
    }
    void Update()
    {
        HandleMovement();
        HandleSpeedIncrease();
        
        // Tick down the attack cooldown
        if (currentCooldownTimer > 0)
        {
            currentCooldownTimer -= Time.deltaTime;
        }

        // Tick down the grace period invincibility
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
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
        
    }

    // This helper lets the Jumper dinos know how fast we are moving!
    public float GetCurrentSpeed()
    {
        return speedLevels[currentSpeedIndex];
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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dino"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (isInvincible) return;

        lives--;
        
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateLives(lives);
        }

        if (lives <= 0)
        {
            Debug.Log("GAME OVER! No lives left.");
            
            // This triggers the Game Over panel via the GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.TriggerGameOver();
            }
        }
        else
        {
            isInvincible = true;
            invincibilityTimer = gracePeriodDuration;

            GameObject[] activeDinos = GameObject.FindGameObjectsWithTag("Dino");
            foreach (GameObject dino in activeDinos)
            {
                Destroy(dino);
            }
        }
    }
    
}