using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 100.0f;  // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the object around its local X axis at the specified speed
        transform.Rotate(Vector3.forward , speed * Time.deltaTime);
    }
}
