using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OutputCurveRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector3[] _positions;
    private readonly Color _lineColor = new Color(47, 77, 120, 255);
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.positionCount = _positions.Length;
        _lineRenderer.SetPositions(_positions);
    }

    public void SetCurvePositions(List<Vector3> newPositions)
    {
        _positions = new Vector3[newPositions.Count];
        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = newPositions[i];
        }
    }
}
