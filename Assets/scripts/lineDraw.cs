using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDraw : MonoBehaviour
{
    public Transform target;

    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(1.22f,-1.35f,-1.67f), new Vector3(2.64f,2.0765f,0.56f));
        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
