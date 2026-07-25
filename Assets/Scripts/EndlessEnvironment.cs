using UnityEngine;

public class EndlessEnvironment : MonoBehaviour
{
    public float groundWidth = 20f;
    
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (mainCamera.position.x - transform.position.x > groundWidth)
        {
            transform.Translate(Vector2.right * (groundWidth * 2));
        }
    }
}