using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScenes : MonoBehaviour
{
    public void LoadGameStart()
    {
        SceneManager.LoadScene("StartGame");
    }
    public void LoadGame()
    {
        PlayerPrefs.DeleteKey("LastWave");
        SceneManager.LoadScene("Game");
    }
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }   

}
