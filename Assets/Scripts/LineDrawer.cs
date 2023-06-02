using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private float lineWidth = 4f;
    [SerializeField] Material material;

    private List<LineRenderer> lineRenderers = new();

    public void DrawLine(GameObject point1, GameObject point2)
    {
        LineRenderer lineRenderer = new GameObject("LineRenderer").AddComponent<LineRenderer>();
        lineRenderer.transform.SetParent(transform);
        lineRenderer.material = material;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, point1.transform.position);
        lineRenderer.SetPosition(1, point2.transform.position);

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        lineRenderers.Add(lineRenderer);
    }
}
