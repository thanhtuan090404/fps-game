using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude)); // rung trong vòng duration giây độ mạnh magnitude
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f; // elapsed là thời gian đã trôi qua kể từ khi bắt đầu rung

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // Random.Range(-1f, 1f) * magnitude để tạo ra một giá trị ngẫu nhiên trong khoảng từ -magnitude đến +magnitude
            float y = Random.Range(-1f, 1f) * magnitude; // Random.Range(-1f, 1f) * magnitude để tạo ra một giá trị ngẫu nhiên trong khoảng từ -magnitude đến +magnitude

            transform.localPosition = originalPosition + new Vector3(x, y, 0); // đặt vị trí của camera bằng vị trí ban đầu cộng với giá trị ngẫu nhiên x và y
            elapsed += Time.deltaTime; // tăng elapsed lên Time.deltaTime để tính toán thời gian đã trôi qua
            yield return null; // chờ đến frame tiếp theo
        }
        
        transform.localPosition = originalPosition; // sau khi rung xong, đặt lại vị trí của camera về vị trí ban đầu
    }
}
