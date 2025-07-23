using System;
using UnityEngine;

public class ScissorGrabber : MonoBehaviour
{
    private GameObject potentialTarget;
    private GameObject heldObject;

    private String tonsilTag = "tonsil";
    // --- Events called by the XR Grab Interactable in the Inspector ---

    public void OnGrabbedByPlayer() { }

    public void OnReleasedByPlayer()
    {
        if (heldObject != null)
        {
            ReleaseObject();
        }
    }

    public void OnScissorActivate()
    {
        if (potentialTarget != null && heldObject == null)
        {
            GrabObject();
        }
    }

    public void OnScissorDeactivate()
    {   
        if (heldObject != null)
        {
            ReleaseObject();
        }
    }
    
    // --- Public methods called by the ScissorGrabZone child script ---

    public void OnZoneEnter(Collider other)
    {
        if (other.CompareTag(tonsilTag))
        {
            potentialTarget = other.gameObject;
        }
    }

    public void OnZoneExit(Collider other)
    {
        if (other.gameObject == potentialTarget)
        {
            potentialTarget = null;
        }
    }

    // --- Grab and Release Functions ---
    void GrabObject()
    {
        heldObject = potentialTarget;
        heldObject.transform.SetParent(this.transform);

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void ReleaseObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        heldObject.transform.SetParent(null);
        heldObject = null;
    }
}