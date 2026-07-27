using UnityEngine;
public class FootstepController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private AudioClip[] footStep;
    [SerializeField] private float stepInterval = 0.4f; // Time interval between footsteps
    float timer ;
    // Update is called once per frame
    void Update()
    {
        // không có âm thanh thì thôi
        if (footStep.Length == 0) return;
        // chỉ đến khi player di chuyển thì mới phát âm thanh
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            timer += Time.deltaTime;
            // Nếu thời gian đã vượt quá khoảng cách giữa các bước chân, phát âm thanh bước chân
            if (timer >= stepInterval)
            {
                timer = 0f;
                // Phát âm thanh bước chân ngẫu nhiên từ mảng footStep
                AudioManager.Instance.PlaySFX(footStep[Random.Range(0, footStep.Length)]);
            }
        }
        else
        {
            timer = 0f; // Reset timer nếu không di chuyển hoặc nhảy
        }
    }
}
