using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeGenChild : MonoBehaviour {
    Vector3 oldPos = Vector3.zero;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

    void Update()
    {
        if (oldPos == transform.localPosition)
            return;

        SlopeGen slope = transform.parent.GetComponent<SlopeGen>();

        int index = int.Parse(name.Split('#')[1]);
        slope.points[index] = transform.localPosition;
        slope.Generate(slope.height);

        oldPos = transform.localPosition;
    }
}
