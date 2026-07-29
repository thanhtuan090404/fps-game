using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Health health; // tham chiếu đến component Health của Enemy
    private static readonly int isDeadHash = Animator.StringToHash("isDead"); // hash của tham số IsDead trong Animator để tăng hiệu suất
    private Animator animator; // biến lưu trữ component Animator của enemy
  
    private void Awake()
    {
        health = GetComponent<Health>(); // lấy component Health của Enemy
        animator = GetComponent<Animator>(); // lấy component Animator của Enemy
        health.OnDeath += HandleDeath; // đăng ký sự kiện OnDeath để xử lý khi Enemy chết
    }

    private void HandleDeath()
    {
        GameManager.Instance.AddKill(); // gọi phương thức Addkill() của GameManager khi Enemy chết
        // Xử lý khi Enemy chết
        animator.SetBool(isDeadHash, true);

        // cắt mọi thứ đang giành quyền điều khiển của Enemy
        GetComponent<Collider>().enabled = false; // tắt collider để không còn va chạm
        GetComponent<EnemyAI>().enabled = false; // tắt EnemyAI để không còn di chuyển hay tấn công
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false; // tắt NavMeshAgent để không còn di chuyển
        Destroy(gameObject, 2f); // hủy game object sau 2 giây để cho animation chết hoàn tất
    }
    private void OnDestroy()
    {
        if (health != null) health.OnDeath -= HandleDeath;   // nhớ unsubscribe!
    }
}



