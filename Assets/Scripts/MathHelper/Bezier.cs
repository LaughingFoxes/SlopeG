using Boo.Lang;
using UnityEngine;

public class Bezier {
    const int CURVE_QUALITY = 10;

    Vector2[] points;
    public List<Vector2> positions;

    public Bezier(Vector2[] points)
    {
        this.points = points;
        this.positions = new List<Vector2>();

        CalculatePoints();
    }

    void CalculatePoints()
    {
        List<Vector2> subPoints = new List<Vector2>();
        for (int i = 0; i < this.points.Length; i++)
        {
            if (i == this.points.Length - 1)
            {
                subPoints.Add(this.points[i]);
                MakeBezier(subPoints);
                subPoints.Clear();
            }
            else if(subPoints.Count > 1 && this.points[i] == subPoints[subPoints.Count - 1])
            {
                MakeBezier(subPoints);
                subPoints.Clear();
            }

            subPoints.Add(this.points[i]);
        }
    }

    void MakeBezier(List<Vector2> points)
    {
        float step = 0.25f / CURVE_QUALITY / points.Count;
        float i = 0;
        int n = points.Count - 1;

        while (i < 1 + step)
        {
            Vector2 point = Vector2.zero;

            for (int p = 0; p < points.Count; p++)
            {
                float a = Cpn(p, n) * Mathf.Pow((1 - i), (n - p)) * Mathf.Pow(i, p);
                point += a * this.points[p];
            }

            this.positions.Add(point);
            i += step;
        }
    }

    float Cpn(int p, int n)
    {
        if (p < 0 || p > n)
            return 0;
        p = Mathf.Min(p, n - p);
        int o = 1;
        for (int i = 1; i < p + 1; i++)
            o = o * (n - p + i) / i;
        return o;
    }
}
