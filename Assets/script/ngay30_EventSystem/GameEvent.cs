using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<Vector3> OnEnemyKilled; // thông báo Enemy vừa chết kèm vị trí
    public static event Action OnPlayerDied; // thông báo player vừa chết

    // khi enemy vừa chết gọi hàm này để phát sóng cho tất cả các script đã nắng nghi
    public static void RaiseEnemyKilled(Vector3 position)
    {
        OnEnemyKilled?.Invoke(position);  // gọi tất cả các hàm đã đăng ký sự kiện OnEnemyKilled với vị trí của Enemy vừa chết
    }
    public static void RaisePlayerDied()
    {
        OnPlayerDied?.Invoke();
    }
}
