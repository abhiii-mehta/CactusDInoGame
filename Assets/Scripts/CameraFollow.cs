using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Drag your Player object here")]
    public Transform player;
    
    [Tooltip("How far ahead or behind the player the camera should sit")]
    public float xOffset = 5f;

    void LateUpdate()
    {
        if (player != null)
        {
            // Follow the player's X position, but keep the camera's original Y and Z
            Vector3 newPosition = new Vector3(player.position.x + xOffset, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}