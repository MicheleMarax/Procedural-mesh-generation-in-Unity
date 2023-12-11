using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sensitivity = 5.0f;

    private float rotX = 0;
    private float rotY = 0;

    void Update()
    {
        Move();

        Rotate();
    }

    private void Rotate()
    {
        // Rotate the camera based on the mouse movement
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;

        rotY = Mathf.Clamp(rotY, -90f, 90f);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 10;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed /= 10;

        // Move the camera forward, backward, left, and right
        transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            transform.position -= transform.up * speed * Time.deltaTime;
        if(Input.GetKey(KeyCode.E))
            transform.position += transform.up * speed * Time.deltaTime;
    }
}
