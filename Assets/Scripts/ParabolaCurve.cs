using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.Numerics.Functions;

public class ParabolaCurve
{
    private static float gravity = Mathf.Abs(Physics.gravity.y);
    public static Vector3[] GetCoordinates(Vector3 throwPoint, float finalY, float angle, float throwSpeed, float mass, float drag, int pointsOnCurve)
    {
        Vector3[] parabolaCoordinates = new Vector3[pointsOnCurve];

        float r = mass / drag;
        float sineAngle = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cosineAngle = Mathf.Cos(angle * Mathf.Deg2Rad);

        float A = finalY - throwPoint.y;
        float B = r * gravity;
        float C = r * (throwSpeed * sineAngle + r * gravity);
        double W = AdvancedMath.LambertW(-(C / (B * r)) * Mathf.Exp((A - C) / (B * r)));

        float timeTilFall = ((B * r * (float)W) - A + C) / B;
        float t;

        for (int i = 0; i < pointsOnCurve; i++)
        {
            t = timeTilFall * (float)i / pointsOnCurve;

            float xDistance = r * throwSpeed * cosineAngle * (1 - Mathf.Exp(-t / r));
            float yDistance = C * (1 - Mathf.Exp(-t / r)) - B * t;

            parabolaCoordinates[i] = new Vector3(throwPoint.x + xDistance, throwPoint.y + yDistance, 0);
        }
        
        return parabolaCoordinates;
    }

}
