using UnityEngine;
using Vector3 = UnityEngine.Vector3;


[RequireComponent(typeof(LineRenderer))]
public class TangentLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    public Transform destination;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] positions = new Vector3[] { gameObject.transform.position, destination.position };
        
        _lineRenderer.startColor = Color.grey;
        _lineRenderer.endColor = Color.grey;
        _lineRenderer.SetPositions(positions);
    }
}
