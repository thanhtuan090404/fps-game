using System;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private Gun gun; // tham chiếu đến đối tượng Gun
    [SerializeField] private TMPro.TextMeshProUGUI ammoText; // tham chiếu đến TextMeshProUGUI để hiển thị số đạn

    private void OnEnable()
    {
        gun.OnAmmoChanged += UpdateAmmoUI; // đăng ký sự kiện khi số đạn thay đổi

    }
    private void OnDisable()
    {
        gun.OnAmmoChanged -= UpdateAmmoUI; // hủy đăng ký sự kiện khi đối tượng bị vô hiệu hóa
    }

    private void UpdateAmmoUI(int arg1, int arg2)
    {
        ammoText.text = $"{arg1}/{arg2}"; // cập nhật số đạn hiển thị trên UI
    }
}
