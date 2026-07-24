using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public event Action OnDeath;    
    public event Action<float> OnHealthChanged; // event để thông báo khi máu thay đổi
    public float CurrentHealthPercent => currentHealth / maxHealth; // tính phần trăm máu hiện tại
    bool isAlive => currentHealth > 0; // kiểm tra xem còn sống hay không

    private void Awake()
    {
        Debug.Log("Health Awake: " + CurrentHealthPercent);

        currentHealth = maxHealth; // khởi tạo máu hiện tại bằng máu tối đa
    }
     void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealthPercent); // thông báo cho các listener biết máu đã thay đổi
        Debug.Log("Health Start: " + CurrentHealthPercent);
    }

    public void TakeDamage(float amount)
    {
        if(!isAlive) return; // nếu đã chết thì không nhận sát thương nữa
        currentHealth -= amount; // trừ máu hiện tại
        currentHealth = Mathf.Max(currentHealth, 0); // giới hạn máu hiện tại trong khoảng từ 0 đến máu tối đa
        OnHealthChanged?.Invoke(currentHealth/maxHealth); // thông báo cho các listener biết máu đã thay đổi
      if(currentHealth <= 0)
        {
            Die(); // nếu máu hiện tại <= 0 thì gọi hàm Die
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(); // thông báo cho các listener biết đã chết
    }
    public void Heal(float amount)
    {
        if (!isAlive) return; // nếu đã chết thì ko hồi máu nữa
        currentHealth += amount; // cộng máu hiện tại
        currentHealth = Mathf.Min(currentHealth, maxHealth); // giới hạn máu hiện tại trong khoảng từ 0 đến máu tối đa
        OnHealthChanged?.Invoke(currentHealth / maxHealth); // thông báo cho các listener biết máu đã thay đổi
    }


}
