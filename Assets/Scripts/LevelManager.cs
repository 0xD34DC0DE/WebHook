using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        if (asyncOperation == null)
        {
            print("[LevelManager] Error loading scene: " + sceneName);
        }
        else
        {
            EventManager.Invoke("OnSceneLoaded");
        }
    }
}