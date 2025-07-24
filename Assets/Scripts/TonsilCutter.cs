using UnityEngine;
using System.Collections.Generic;
using EzySlice;
using System;

public class TonsilCutter : MonoBehaviour
{
    public Material sliceMaterial;

    private List<Vector3> cutPath = new List<Vector3>();
    private GameObject currentTarget;
    private bool isCutting = false;

    private String tonsilTag = "tonsil";
    private String tonsilLeftTag = "Left-Tonsil";
    private String tonsilRightTag = "Right-Tonsil";

    private Vector3 tonsilScale = new Vector3(0.13f, 0.06f, 0.06f);

    private bool CompareTags(Collider other)
    {
        return other.CompareTag(tonsilTag) || other.CompareTag(tonsilLeftTag) || other.CompareTag(tonsilRightTag);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (CompareTags(other))
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
        if (isCutting && CompareTags(other) && other.gameObject == currentTarget)
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
            String pieceToStayTag = tonsilTag;
            // Create the two new GameObjects from the sliced hull
            GameObject upperHull = sliced.CreateUpperHull(target, sliceMaterial);
            GameObject lowerHull = sliced.CreateLowerHull(target, sliceMaterial);

            GameObject pieceToFall = null;
            GameObject pieceToStay = null;

            if (target.CompareTag(tonsilRightTag))
            {
                // GOAL: Detach the right side of the left tonsil.
                // The "right" piece is in the direction of the positive X-axis.
                // If sliceNormal.x > 0, the upper hull is the right piece.
                // Otherwise, the lower hull is the right piece.
                pieceToFall = (sliceNormal.x > 0) ? upperHull : lowerHull;
                pieceToStay = (sliceNormal.x > 0) ? lowerHull : upperHull;
                pieceToStayTag = tonsilRightTag;
            }
            else if (target.CompareTag(tonsilLeftTag))
            {
                // The "left" piece is in the direction of the negative X-axis.
                // If sliceNormal.x < 0, the upper hull is the left piece.
                // Otherwise, the lower hull is the left piece.
                pieceToFall = (sliceNormal.x < 0) ? upperHull : lowerHull;
                pieceToStay = (sliceNormal.x < 0) ? lowerHull : upperHull;
                pieceToStayTag = tonsilLeftTag;
            }
            // If we successfully identified the pieces (i.e., a tonsil was cut)
            if (pieceToFall != null && pieceToStay != null)
            {
                // --- Configure the piece that STAYS ---
                // Keep its original tag so it can be sliced again
                pieceToStay.tag = pieceToStayTag;
                // Set its transform to match the original object
                pieceToStay.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
                pieceToStay.transform.localScale = tonsilScale;
                // Add a collider so it's still detectable
                pieceToStay.AddComponent<MeshCollider>().convex = true;

                // --- Configure the piece that FALLS ---
                // Give it a generic tag, as it's just a fragment now
                pieceToFall.tag = tonsilTag;
                // Set its transform
                pieceToFall.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
                pieceToFall.transform.localScale = tonsilScale;
                // Add physics components to make it fall
                Rigidbody rb = pieceToFall.AddComponent<Rigidbody>();
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
                rb.isKinematic = true;
                pieceToFall.AddComponent<MeshCollider>().convex = true;
                // Remove the original, unsliced object from the scene
                Destroy(target);
            }
            else if (target.CompareTag(tonsilTag))
            {
                
                // // where both pieces get a Rigidbody.
                // // upperHull.AddComponent<Rigidbody>();
                // upperHull.AddComponent<MeshCollider>().convex = true;
                // upperHull.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);
                // upperHull.transform.localScale = tonsilScale;

                // // lowerHull.AddComponent<Rigidbody>();
                // lowerHull.AddComponent<MeshCollider>().convex = true;
                // lowerHull.transform.SetPositionAndRotation(target.transform.position, target.transform.rotation);

                // lowerHull.transform.localScale = tonsilScale;
                // lowerHull.tag = tonsilTag;
                // upperHull.tag = tonsilTag;

                // Destroy(target);
            }


            Destroy(target);
        }
    }
}
