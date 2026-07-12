using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    private CharacterController controller; // biến lưu trữ tham chiếu đến CharacterController của người chơi
    [SerializeField] private float speed = 5f;
    private float h, v; // biến lưu trữ giá trị input từ bàn phím
    private float gravity = -9.81f; // giá trị trọng lực
    private float velocityY; // biến lưu trữ vận tốc rơi của người chơi
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); // lấy tham chiếu đến CharacterController
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = controller.isGrounded; // kiểm tra xem người chơi có đang đứng trên mặt đất hay không
        if (grounded && velocityY < 0)
        {
            velocityY = -2f;
        }
        h = Input.GetAxis("Horizontal"); // lấy giá trị input từ bàn phím (A,D)
        v = Input.GetAxis("Vertical"); // lấy giá trị input từ bàn phím (W,S)
        Vector3 movement = transform.right * h + transform.forward * v; // tính toán hướng di chuyển dựa trên hướng camera
        controller.Move(movement * speed * Time.deltaTime); // di chuyển người chơi theo hướng camera
        velocityY += gravity * Time.deltaTime; // tính toán vận tốc rơi
        controller.Move(new Vector3(0, velocityY, 0) * Time.deltaTime); // di chuyển người chơi theo trọng lực
    }
}
