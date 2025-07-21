using UnityEngine;

public class MouthCollisionTracking : MonoBehaviour
{
    private GameObject currentCollidingObject = null;
    private float collisionStartTime = 0f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mouth"))
        {
            Debug.Log("$Colliding with Mouth");
            currentCollidingObject = collision.gameObject;
            collisionStartTime = Time.time;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mouth"))
        {
            Debug.Log($"Collision ended with {collision.gameObject} after {Time.time - collisionStartTime:F2} seconds");
            currentCollidingObject = null;
            collisionStartTime = 0f;
        }
    }
}