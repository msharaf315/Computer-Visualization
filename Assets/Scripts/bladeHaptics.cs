using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BladeHaptics : MonoBehaviour
{
    // Haptic feedback settings
    [Header("Haptic Settings")]
    [Range(0, 1)]
    public float intensity = 1.0f;
    public float duration = 0.2f;

    // A reference to the knife's XRGrabInteractable component
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Find the XRGrabInteractable on this object or its parent
        grabInteractable = GetComponentInParent<XRGrabInteractable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the blade collides with an object tagged "Mouth"
        if (other.CompareTag("Mouth"))
        {
            // Check if the knife is actually being held by a controller
            if (grabInteractable != null && grabInteractable.isSelected)
            {
                // The XRGrabInteractable knows which interactor (controller) is holding it.
                // We get the first one, which is usually the only one.
                if (grabInteractable.interactorsSelecting.Count > 0)
                {
                    // The interactor needs to be cast to a controller interactor
                    if (grabInteractable.interactorsSelecting[0] is XRBaseControllerInteractor controllerInteractor)
                    {
                        // Send the haptic impulse to the controller
                        controllerInteractor.xrController.SendHapticImpulse(intensity, duration);
                        Debug.Log($"Haptics sent to {controllerInteractor.xrController.name} on collision with {other.name}.");
                    }
                }
            }
        }
    }
}