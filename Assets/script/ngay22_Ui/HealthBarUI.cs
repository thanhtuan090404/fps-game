using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health targetHealth; // biến để tham chiếu đến đối tượng Health mà thanh máu sẽ hiển thị
    [SerializeField] private Image fillImage; // biến để tham chiếu đến hình ảnh thanh máu


    private void OnEnable()
    {
        Debug.Log("UI OnEnable: " + targetHealth.CurrentHealthPercent);

        if (targetHealth == null) return; // nếu targethealth chưa được gán thì thoát khỏi hàm
        {
            targetHealth.OnHealthChanged += UpdateBar; // đăng ký sự kiện OnHealthChanged để cập nhật thanh máu khi máu thay đổi
            UpdateBar(targetHealth.CurrentHealthPercent); // cập nhật thanh máu lần đầu tiên khi bật UI
        }
     }
    private void OnDisable()
    {
        if (targetHealth == null) return; // nếu targethealth chưa được gán thì thoát khỏi hàm
        
            targetHealth.OnHealthChanged -= UpdateBar; // hủy đăng ký sự kiện OnHealthChanged khi UI bị tắt
        
    }


    private void UpdateBar(float healthPercent)
    {
        Debug.Log("UpdateBar: " + healthPercent);

        fillImage.fillAmount = healthPercent;
        fillImage.color = Color.Lerp(Color.red,
                             Color.green,
                             healthPercent);
    }
}
