using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, rotationX, Space.World);
            transform.Rotate(Vector3.up, rotationY, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}