using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class SwordHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("SWORD TOUCHED: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Dino"))
        {
            DinoBehavior dino = other.GetComponent<DinoBehavior>();
            
            if (dino != null)
            {
                Debug.Log("DINO KILLED!");
                dino.TakeDamage();
            }
            else
            {
                Debug.LogWarning("Found the Dino tag, but the DinoBehavior script is missing!");
            }
        }
        // SEPARATE CHECK FOR FLAME PROJECTILES
        else if (other.CompareTag("FlameProjectile"))
        {
            Debug.Log("PROJECTILE PARRIED!");
            Destroy(other.gameObject);
        }
    }
}