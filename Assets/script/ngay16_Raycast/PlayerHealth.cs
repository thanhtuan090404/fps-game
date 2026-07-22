using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Health health;



     void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += HandleDeath;
        health.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float obj)
    {
        Debug.Log("Player health changed: " + obj);
    }

    private void HandleDeath()
    {
        GameManager.Instance.GameOver();
        health.OnDeath -= HandleDeath;
        Debug.Log("Player has died.");
    }
    void OnDestroy()
    {
        health.OnDeath -= HandleDeath;
        health.OnHealthChanged -= HandleHealthChanged;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) health.TakeDamage(20f);   // test mất máu
        if (Input.GetKeyDown(KeyCode.H)) health.Heal(20f);          // test hồi máu
    }
}
