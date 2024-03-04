using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    public void EnableLineRenderer()
    {
        lineRenderer.enabled = true;
    }

    public void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    public void MovementLine(Vector2 targetPos)
    {
        //shows a line towards the cursor from the player position
        lineRenderer.SetPosition(0, transform.position);
        
        lineRenderer.SetPosition(1, targetPos);
    }
}
