using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;

    private float _xRotation = 0f; // biến lưu trữ góc xoay dọc của camera
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // khóa và ẩn con trỏ chuột
        Cursor.lockState = CursorLockMode.Locked; // khóa chuột vô giữu
        Cursor.visible = false; // ẩn con trỏ chuột

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // xoay thân người ngang
        transform.Rotate(Vector3.up * mouseX); // xoay quanh trục y

        // ngẩng cúi camera (dọc ) cần CLAMP
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // giới hạn góc nhìn lên xuống
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); // xoay camera quanh trục x

    }
}
