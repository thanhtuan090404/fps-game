using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("FPS_Game");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }


}
