using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class SwordHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // This tells us the hitbox is actually working and what it touched
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
    }
}