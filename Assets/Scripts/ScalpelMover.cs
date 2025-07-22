using UnityEngine;

public class ScalpelMover : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A/D = left/right
        float v = Input.GetAxis("Vertical");   // W/S = forward/back
        float y = 0f;

        // Q = down, E = up
        if (Input.GetKey(KeyCode.E)) y = 1f;
        else if (Input.GetKey(KeyCode.Q)) y = -1f;

        Vector3 move = new Vector3(h, y, v) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}