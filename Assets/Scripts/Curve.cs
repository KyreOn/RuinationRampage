using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve
{
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _controlPoint;

    public Curve(Vector3 a, Vector3 b, Vector3 c)
    {
        _startPoint = a;
        _endPoint = b;
        _controlPoint = c;
    }
    
    public Vector3 Evaluate(float t)
    {
        var ac = Vector3.Lerp(_startPoint,   _controlPoint, t);
        var bc = Vector3.Lerp(_controlPoint, _endPoint,     t);
        return Vector3.Lerp(ac, bc, t);
    }
}
