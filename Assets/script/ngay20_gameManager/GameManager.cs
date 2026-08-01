using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; internal set; }

    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private int killsWin = 30;

    private int _kills;
    private bool _gameEnded;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        messageText.text = "";
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
       
    } 
    public void Replay()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("FPS_Game");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // Ẩn con trỏ chuột khi bắt đầu lại trò chơi
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
    }

    public void AddKill()
{
    if (_gameEnded) return;
    _kills++;
    UpdateUI();
    if (_kills >= killsWin)
    {
        EndGame(true);
    }
}


    public void GameOver()
    {
        EndGame(false);
    }

    private void EndGame(bool won)
    {
        _gameEnded = true;
        IsGameOver = true;
        gameOver.SetActive(true);
        messageText.text = won ? "You Win!" : "Game Over!";

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  // Hiển thị con trỏ chuột


    }


    private void UpdateUI()
    {
        killText.text = $"Kills: {_kills}/{killsWin}";
    }
}

