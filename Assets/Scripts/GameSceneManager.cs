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
            LoadCurrentScene(0);
            return;
        }

        LoadCurrentScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void GoToPrevStage()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        LoadCurrentScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void ReloadScene()
    {
        LoadCurrentScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadCurrentScene(int index)
    {

        SceneManager.LoadScene(index);
    }
}
