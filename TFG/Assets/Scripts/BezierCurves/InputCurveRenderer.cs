using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class InputCurveRenderer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Transform startTangent;
    public Transform endTangent;

    private LineRenderer _lineRenderer;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] curvePoints = new Vector3[100];
        for (int i = 0; i < curvePoints.Length; i++)
        {
            float t = i / (float)(curvePoints.Length - 1);
            curvePoints[i] = CalculateBezierPoint(t, startPoint.position, endPoint.position, startTangent.position, endTangent.position);
        }
        
        _lineRenderer.startColor = Color.magenta;
        _lineRenderer.endColor = Color.magenta;
        _lineRenderer.positionCount = curvePoints.Length;
        _lineRenderer.SetPositions(curvePoints);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tSq = t * t;
        float uSq = u * u;
        float uQb = uSq * u;
        float tQb = tSq * t;

        Vector3 point = uQb * p0;
        point += 3 * uSq * t * p1;
        point += 3 * u * tSq * p2;
        point += tQb * p3;

        return point;
    }
}