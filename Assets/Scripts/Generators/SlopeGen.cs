using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SlopeGen : MonoBehaviour {
    public Vector2[] points;
    public int slopeVersion = 0;
    public bool tmpSlope = true;

    Mesh mesh = null;

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        for (int i = 0; i < points.Length; i++) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + (Vector3)this.points[i], 0.2f);
        }
    }

    void Awake()
    {
        CreateChildPoints();

        mesh = GetComponent<MeshFilter>().mesh;
        Generate(2000);
    }

    public void CreateChildPoints()
    {
        while (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);

        for (int i = 0; i < points.Length; i++)
        {
            GameObject child = new GameObject();
            child.name = "Point #" + i;
            child.AddComponent<SlopeGenChild>();
            child.transform.localPosition = points[i];
            child.transform.parent = transform;
        }
    }

    public void Generate(float height)
    {
        List<Vector3> vert = MakeVert(height);
        mesh.vertices = vert.ToArray();
        mesh.triangles = MakeTri(vert);
    }

    List<Vector3> MakeVert(float height)
    {
        Bezier bezier = new Bezier(this.points);
        List<Vector3> vert = new List<Vector3>(bezier.positions.Count + 2);

        // Make collision if collision module exists
        EdgeCollider2D coll = GetComponent<EdgeCollider2D>();
        if (coll != null)
        {
            coll.points = bezier.positions.ToArray();
        }

        vert.Add(new Vector2(bezier.positions[0].x, -height));
        vert.Add(new Vector2(bezier.positions[bezier.positions.Count - 1].x, -height));

        for (int i = 0; i < bezier.positions.Count; i++)
            vert.Add(bezier.positions[i]);

        return vert;
    }

    int[] MakeTri(List<Vector3> vert)
    {
        int triCount = (vert.Count - 2) * 3 + 3;

        int[] tri = new int[triCount];
        int index = 0;

        Func<int[], bool> Add = (arr) =>
        {
            for (int i = 0; i < 3; i++)
                tri[index + i] = arr[i];
            index += 3;
            return true;
        };

        int splitIndex = 0;
        float lowestSlope = float.PositiveInfinity;
        for (int i = 2; i < vert.Count; i++)
        {
            if (vert[i].y < lowestSlope)
            {
                splitIndex = i;
                lowestSlope = vert[i].y;
            }
        }

        //Debug.Log("Lowest y value is: " + lowestSlope);
        //Debug.Log("With index: " + splitIndex);

        for (int i = 2; i < splitIndex; i++)
        {
            Add(new int[] { 0, i, i + 1 });
        }

        for (int i = splitIndex; i < vert.Count - 2; i++)
        {
            Add(new int[] { 1, i, i + 1 });
        }

        Add(new int[] { 0, splitIndex, 1 });

        return tri;
    }
}
