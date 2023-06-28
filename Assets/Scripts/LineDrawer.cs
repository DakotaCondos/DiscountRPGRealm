using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private float lineWidth = 4f;
    [SerializeField] private float maxWiggleAmount = 0.5f; // Maximum amount of wiggle
    [SerializeField] private int minNumWigglePoints = 2; // Minimum number of points for wiggle
    [SerializeField] private int maxNumWigglePoints = 10; // Maximum number of points for wiggle

    [SerializeField] private Material material;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();

    public void DrawLine(GameObject point1, GameObject point2)
    {
        LineRenderer lineRenderer = new GameObject("LineRenderer").AddComponent<LineRenderer>();
        lineRenderer.transform.SetParent(transform);
        lineRenderer.material = material;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.alignment = LineAlignment.TransformZ;

        List<Vector3> positions = new List<Vector3>();

        // Add fixed endpoints
        positions.Add(point1.transform.position);

        int numWigglePoints = Random.Range(minNumWigglePoints, maxNumWigglePoints + 1);

        for (int i = 1; i <= numWigglePoints; i++)
        {
            Vector3 wiggleOffset = Random.insideUnitSphere * maxWiggleAmount;
            positions.Add(Vector3.Lerp(point1.transform.position, point2.transform.position, i / (float)(numWigglePoints + 1)) + wiggleOffset);
        }

        positions.Add(point2.transform.position);

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        lineRenderers.Add(lineRenderer);
    }
}
