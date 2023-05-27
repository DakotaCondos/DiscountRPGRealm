using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject start;
    public GameObject end;
    public float lineWidth = 2;

    private void Start()
    {
        // Set the material and width for the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        // Test
        DrawLine(start, end);
    }
    public void DrawLine(GameObject startObject, GameObject endObject)
    {
        // Set the positions of the line renderer
        lineRenderer.SetPosition(0, startObject.transform.position);
        lineRenderer.SetPosition(1, endObject.transform.position);
    }
}
