using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineWidth = 2;

    private void Start()
    {
        // Set the material and width for the line
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    public void DrawLine(GameObject startObject, GameObject endObject)
    {
        // Set the positions of the line renderer
        lineRenderer.SetPosition(0, startObject.transform.position);
        lineRenderer.SetPosition(1, endObject.transform.position);
    }
}
