using UnityEngine;

public class ScissorGrabZone : MonoBehaviour
{
    // Reference to the main script on the parent
    private ScissorGrabber parentScissor;

    void Awake()
    {
        // Find the ScissorGrabber script on the parent object
        parentScissor = GetComponentInParent<ScissorGrabber>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tell the parent script that an object has entered the zone
        parentScissor.OnZoneEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Tell the parent script that an object has left the zone
        parentScissor.OnZoneExit(other);
    }
}