using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 20f;
    public float rotationSpeed = 100f;

    private Vector3 lastMousePosition;

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleRotation();
    }

    private void HandlePan()
    {
        if (Input.GetMouseButton(2)) // Middle mouse button
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 pan = new Vector3(-delta.x, -delta.y, 0) * panSpeed * Time.deltaTime;
            transform.Translate(pan, Space.Self);
        }
        lastMousePosition = Input.mousePosition;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = scroll * zoomSpeed * transform.forward;
        transform.Translate(zoom, Space.World);
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float yaw = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float pitch = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, yaw, Space.World);
            transform.Rotate(Vector3.right, pitch, Space.Self);
        }
    }
}
