using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private string nameOfPauseScene = "UI_Windows Scene";
    [SerializeField] private bool loadUIWindows = true;

    private void Awake()
    {
        if (!loadUIWindows)
            return;

        int countLoaded = SceneManager.sceneCount;
        for(int i = 0; i < countLoaded; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.name == nameOfPauseScene)
            {
                return;
            }
        }

        // load in the background pause menu
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    }

    public void SetPauseGame(bool isPaused)
    {

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
