using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager {

    static GameSceneManager Inst;

    public static GameSceneManager GetInstance()
    {
        if (Inst == null)
            Inst = new GameSceneManager();

        return Inst;
    }

    public void GoToNextStage()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            ReturnToWelcome();
            return;
        }

        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void GoToPrevStage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void ReloadScene()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToWelcome()
    {
        LoadSceneByIndex(0);
    }

    private void LoadSceneByIndex(int index)
    {

        SceneManager.LoadScene(index);
    }
}
