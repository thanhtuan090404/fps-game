using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward; // đặt hướng của đối tượng billboard về phía camera chính
    }
}
