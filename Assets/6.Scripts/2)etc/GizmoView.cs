using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoView : MonoBehaviour
{
    public Color GizmoColor = Color.blue;
    public float radius = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
