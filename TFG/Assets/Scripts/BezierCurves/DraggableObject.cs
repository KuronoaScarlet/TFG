using UnityEngine;

public class DraggableObject : MonoBehaviour
{
   public LayerMask layerMask;

    private Vector3 _mouseOffset;
    private float _mouseZCoord;
    private Rigidbody2D _rb;
    private Vector3 _startPos;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }

    private void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }
    }

    void OnMouseDown()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _mouseZCoord;

        _mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _mouseZCoord;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePoint) + _mouseOffset;
        mousePos.z = gameObject.transform.position.z;

        _rb.MovePosition(mousePos);
    }

    public void ResetPosition()
    {
        _rb.position = _startPos;
    }
}

