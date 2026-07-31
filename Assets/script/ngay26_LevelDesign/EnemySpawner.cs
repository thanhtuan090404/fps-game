using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyPrefab; // Prefab của EnemyAI để spawn
    [SerializeField] private int maxEnemies = 40; // Số lượng tối đa enemy có thể tồn tại cùng lúc
    [SerializeField] private Transform[] spawnPoints; // Các điểm spawn mà enemy có thể xuất hiện
     private int currentEnemy = 0; // Số lượng enemy hiện tại đang tồn tại
    [SerializeField] private float spawnInterval = 1f; // Thời gian giữa các lần spawn enemy
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2f); // Chờ 2 giây trước khi spawn enemy đầu tiên
        while ((GameManager.Instance != null && !GameManager.Instance.IsGameOver))
        {
            if (currentEnemy < maxEnemies)
            {
                SpawnEnemy();


            }
            yield return new WaitForSeconds(spawnInterval); // Chờ 5 giây trước khi spawn enemy tiếp theo


        }
    }
    

    void SpawnEnemy()
    {
        // Chọn một điểm spawn ngẫu nhiên từ mảng spawnPoints
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        // Tạo một instance của EnemyAI từ prefab
        EnemyAI newEnemy = Instantiate(enemyPrefab, spawnPoint.position , spawnPoint.rotation);
        currentEnemy++; // Tăng số lượng enemy hiện tại
        Health enemyHealth = newEnemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            void OnDead()
            {
                currentEnemy--;
                enemyHealth.OnDeath -= OnDead; // Hủy đăng ký sự kiện để tránh rò rỉ bộ nhớ
            }
            enemyHealth.OnDeath += OnDead; // Đăng ký sự kiện OnDeath của enemy để giảm số lượng enemy hiện tại khi nó chết
        }

    }
  
}


