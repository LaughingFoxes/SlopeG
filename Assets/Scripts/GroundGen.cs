using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GroundGen : MonoBehaviour
{
    public float noiseSpace = 1f;
    public float start = 0f;
    public float end = 10f;

    const int HEIGHT = -50;

    Mesh mesh = null;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Generate();
    }

    public void Generate()
    {
        List<Vector3> vert = MakeVert();

        //int vertPathCount = vert.Count;

        List<int> topPoints, bottomPoints;
        AddStars(ref vert, out topPoints, out bottomPoints);

        mesh.vertices = vert.ToArray();
        mesh.triangles = MakeTri(vert, topPoints.ToArray(), bottomPoints.ToArray());
    }

    List<Vector3> MakeVert()
    {
        List<Vector2> points = new List<Vector2>();

        for (float f = start; f < end; f += noiseSpace)
        {
            points.Add(new Vector2(f, getHeight(f)));
        }

        List<Vector3> vert = new List<Vector3>(); //(points.Count + 2); //We dont know how long the list will be anymore

        // Make collision if collision module exists
        EdgeCollider2D coll = GetComponent<EdgeCollider2D>();
        if (coll != null)
        {
            coll.points = points.ToArray();
        }

        vert.Add(new Vector2(start, HEIGHT));
        vert.Add(new Vector2(end, HEIGHT));

        for (int i = 0; i < points.Count; i++)
            vert.Add(points[i]);

        return vert;
    }

    // I just remembered that c# has this great feature that allows me to code in my style ;)
    void AddStars(ref List<Vector3> vert, out List<int> topPoints, out List<int> bottomPoints)
    {
        topPoints = new List<int>(); //Connection star @bottom of this point
        bottomPoints = new List<int>(); //Places where we shift to connect to the next star
        bool up = true;
        for (int i = 2; i < vert.Count - 1; i++)
        {
            if (vert[i].y < vert[i + 1].y)
            {
                if (!up)
                    bottomPoints.Add(i);
                up = true;
            }
            else
            {
                if (up)
                    topPoints.Add(i);
                up = false;
            }
        }

        for (int i = 0; i < topPoints.Count; i++) // Add the stars
        {
            Vector3 star = vert[topPoints[i]];
            star.y = HEIGHT;
            vert.Add(star);
        }
    }

    int[] MakeTri(List<Vector3> vert, int[] topPoints, int[] bottomPoints)
    {
        int triCount = (vert.Count - 2) * 3 + bottomPoints.Length * 3;

        int[] tri = new int[triCount];
        int index = 0;

        Func<int[], bool> Add = (arr) =>
        {
            for (int i = 0; i < 3; i++)
                tri[index + i] = arr[i];
            index += 3;
            return true;
        };



        int connector = 0;
        int splitIndex = 0;

        int check = vert.Count - topPoints.Length - 2; //I put this into a variable so the compiler understands that this value wont change
        for (int i = 2; i < check; i++)
        {
            if (splitIndex >= 0 && i >= bottomPoints[splitIndex])
            {
                splitIndex++;
                if (bottomPoints.Length <= splitIndex)
                {
                    splitIndex = -1; // Just so we "disable" it
                    connector = 1; // Make it use the 2nd point (last known)
                }
                else
                    connector = vert.Count - bottomPoints.Length + splitIndex - 1;
            }

            Add(new int[] { connector, i, i + 1 });
        }

        //Add the star -> splitPoint -> next star triangles
        int s = vert.Count - topPoints.Length;
        for (int i = 1; i < topPoints.Length - 2; i++)
        {
            Add(new int[] { s + i, bottomPoints[i], s + i + 1 });
        }

        //Manually add first and last triangle
        Add(new int[] { 0, bottomPoints[0], s + 1 });
        Add(new int[] { 1, vert.Count - 2, bottomPoints[bottomPoints.Length - 1] });

        return tri;
    }

    float getHeight(float x)
    {
        return Mathf.PerlinNoise(x * 0.1f, 0.0f) * 20.0f;
    }
}
