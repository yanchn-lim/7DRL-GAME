using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderVisuals : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // This method allows enabling or disabling the LineRenderer. It takes a boolean parameter enabled, and the method sets the enabled property of the LineRenderer to the value of the input parameter.
    // If enabled is true, the LineRenderer will become visible; if false, it will be hidden.
    public void SetEnabled(bool enabled)
    {
        lineRenderer.enabled = enabled;
    }


    // This method allows setting the starting position of the LineRenderer. It takes a Vector3 parameter position, which represents the desired starting position.
    // The method then sets the first position of the LineRenderer (index 0) to the specified position.
    public void SetStartPosition(Vector3 position)
    {
        lineRenderer.SetPosition(0, position);
    }


    // This method allows setting the ending position of the LineRenderer. Similar to SetStartPosition, it takes a Vector3 parameter position, which represents the desired ending position.
    // The method then sets the second position of the LineRenderer (index 1) to the specified position.
    public void SetEndPosition(Vector3 position)
    {
        lineRenderer.SetPosition(1, position);
    }
}
