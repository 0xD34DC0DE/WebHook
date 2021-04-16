using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        { 
            _instance = (T) this;
            if (_instance is GameManager)
            {
                DontDestroyOnLoad(_instance);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}