using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Health health;



     void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += HandleDeath;
        health.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float healthPercent)
    {
       
    }

    private void HandleDeath()
    {
        GameManager.Instance.GameOver();
        Debug.Log("Player has died.");
    }
    void OnDestroy()
    {
        health.OnDeath -= HandleDeath;
        health.OnHealthChanged -= HandleHealthChanged;
    }
  
}
