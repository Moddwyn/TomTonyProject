using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class LineLightningEffect : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int segments = 10;
    public float displacement = 5.0f;
    public float roughness = 0.5f;
    public float duration = 0.3f; // Duration the lightning is visible
    
    float startingDisplacement;
    Tower tower;

    void OnEnable()
    {
        tower = GetComponent<Tower>();
        lineRenderer.enabled = false;

        startingDisplacement = displacement;
    }

    public void Shoot()
    {
        lineRenderer.enabled = true;
        Vector3 startPoint = tower.projectileLocation.position;
        Vector3 endPoint = tower.target.position;
        DrawLightning(startPoint, endPoint);

        // Hide the line renderer after a delay
        StartCoroutine(HideAfterDelay(duration));
    }

    void DrawLightning(Vector3 startPoint, Vector3 endPoint)
    {
        displacement = startingDisplacement;
        Vector3[] positions = new Vector3[segments + 1];
        lineRenderer.positionCount = segments + 1;
        float segmentLength = 1.0f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float t = i * segmentLength;
            Vector3 linePos = Vector3.Lerp(startPoint, endPoint, t);

            if (i != 0 && i != segments) // not start or end point
            {
                linePos += Random.insideUnitSphere * displacement;
            }

            positions[i] = linePos;

            // Decrease displacement for next segment
            displacement *= roughness;
        }

        lineRenderer.SetPositions(positions);
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }
}
