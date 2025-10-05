using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}