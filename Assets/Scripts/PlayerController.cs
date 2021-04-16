using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Transform _transform;
    private Camera camera;

    private float rotationSpeed = 150f;
    private float speed = 5f;

    private float pitch;
    private float yaw;
    private float roll;

    private float translationZ;
    private float translationX;

    void Start()
    {
        camera = Camera.main;
        _transform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        translationZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        translationX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        yaw = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;

        _transform.position += (_transform.forward * translationZ) + (_transform.right * translationX);
        _transform.eulerAngles += new Vector3(0, yaw, 0);
    }

    private void LateUpdate()
    {
        pitch = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
        roll = 0f;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        camera.transform.localEulerAngles += new Vector3(-pitch, 0, roll);
    }
}