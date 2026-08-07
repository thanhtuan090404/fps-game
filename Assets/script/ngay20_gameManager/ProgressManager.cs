using System;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    public int TotalKills { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HeandleEnemyKilled;
    }
    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HeandleEnemyKilled;
    }

    private void HeandleEnemyKilled(Vector3 position)
    {
        TotalKills++;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}

    
