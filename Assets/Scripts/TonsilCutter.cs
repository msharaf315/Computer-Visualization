using UnityEngine;
using System.Collections.Generic;
using EzySlice;

public class TonsilCutter : MonoBehaviour
{
    public Material sliceMaterial;

    private List<Vector3> cutPath = new List<Vector3>();
    private GameObject currentTarget;
    private bool isCutting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tonsil"))
        {
            currentTarget = other.gameObject;
            cutPath.Clear();
            cutPath.Add(transform.position);
            isCutting = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCutting && other.gameObject == currentTarget)
        {
            if (cutPath.Count == 0 || Vector3.Distance(cutPath[cutPath.Count - 1], transform.position) > 0.01f)
            {
                cutPath.Add(transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCutting && other.CompareTag("tonsil") && other.gameObject == currentTarget)
        {
            isCutting = false;

            if (cutPath.Count >= 2)
            {
                Vector3 start = cutPath[0];
                Vector3 end = cutPath[cutPath.Count - 1];
                Vector3 sliceDir = (end - start).normalized;
                Vector3 sliceNormal = Vector3.Cross(sliceDir, transform.right);
                SliceObject(currentTarget, sliceNormal, transform.position);
            }
        }
    }

    private void SliceObject(GameObject target, Vector3 sliceNormal, Vector3 planePosition)
    {
        SlicedHull sliced = target.Slice(planePosition, sliceNormal, sliceMaterial);

        if (sliced != null)
        {
            GameObject upper = sliced.CreateUpperHull(target, sliceMaterial);
            GameObject lower = sliced.CreateLowerHull(target, sliceMaterial);

            upper.transform.position = target.transform.position;
            lower.transform.position = target.transform.position;
            upper.transform.rotation = target.transform.rotation;
            lower.transform.rotation = target.transform.rotation;
            upper.transform.localScale = target.transform.localScale;
            lower.transform.localScale = target.transform.localScale;

            Rigidbody upperRb = upper.AddComponent<Rigidbody>();
            upper.AddComponent<MeshCollider>().convex = true;
            upperRb.AddForce(Vector3.up * 2f, ForceMode.Impulse);

            Rigidbody lowerRb = lower.AddComponent<Rigidbody>();
            lower.AddComponent<MeshCollider>().convex = true;
            lowerRb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

            Destroy(target);
        }
    }
}
