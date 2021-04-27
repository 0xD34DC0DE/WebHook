using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{  
    public EventManager.OnSceneLoaded OnSceneLoadedEvent;
    
    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        if (asyncOperation == null)
        {
            print("[LevelManager] Error loading scene: " + sceneName);
        }
        else
        {
            OnSceneLoadedEvent.Invoke(sceneName);
        }
    }
}