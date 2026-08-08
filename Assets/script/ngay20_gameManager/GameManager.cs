using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; private set; }

    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private int killsWin = 30;
    [SerializeField] private TextMeshProUGUI totalKillsText; // Thêm biến để hiển thị tổng số lượng kill

    private int _kills;

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
    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnPlayerDied += GameOver;

    }
    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnPlayerDied -= GameOver;
    }
  
    private void HandleEnemyKilled(Vector3 position)
    {
        AddKill();
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
    if (IsGameOver) return;
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
        IsGameOver = true;
        gameOver.SetActive(true);
        messageText.text = won ? "You Win!" : "Game Over!";
        totalKillsText.text =
    "Total Kills: " + ProgressManager.Instance.TotalKills;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  // Hiển thị con trỏ chuột


    }


    private void UpdateUI()
    {
        killText.text = $"Kills: {_kills}/{killsWin}";
    }
}

