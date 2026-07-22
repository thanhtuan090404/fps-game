using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private int killsWin = 3;

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
            if (_gameEnded && Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //

            }
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
        messageText.text = won ? "YOU WIN! (R to restart)" : "GAME OVER (R to restart)";
    }


    private void UpdateUI()
    {
        killText.text = $"Kills: {_kills}/{killsWin}";
    }
}

