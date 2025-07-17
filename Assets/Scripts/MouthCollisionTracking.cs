using UnityEngine;

public class MouthCollisionTracking : MonoBehaviour
{
    private GameObject currentCollidingObject = null;
    private float collisionStartTime = 0f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Debug.Log("$Colliding with knife");
            currentCollidingObject = collision.gameObject;
            collisionStartTime = Time.time;
            Debug.Log($"Started colliding with {currentCollidingObject}");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            Debug.Log("$Colliding with knife");
            Debug.Log($"Collision ended with {collision.gameObject} after {Time.time - collisionStartTime:F2} seconds");
            currentCollidingObject = null;
            collisionStartTime = 0f;
        }
             Debug.Log($"Collision ended with {collision.gameObject} after {Time.time - collisionStartTime:F2} seconds");
                currentCollidingObject = null;
                collisionStartTime = 0f;
      }
}