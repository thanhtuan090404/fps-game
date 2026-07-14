using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Health health; // tham chiếu đến component Health của Enemy

    private void Awake()
    {
        health = GetComponent<Health>(); // lấy component Health của Enemy
        health.OnDeath += HandleDeath; // đăng ký sự kiện OnDeath để xử lý khi Enemy chết
    }

    private void HandleDeath()
    {
        // Xử lý khi Enemy chết
        Debug.Log("Enemy has died.");
        Destroy(gameObject); // hủy đối tượng Enemy

    }
    private void OnDestroy()
    {
        health.OnDeath -= HandleDeath; // hủy đăng ký sự kiện OnDeath khi Enemy bị hủy
    }
}



