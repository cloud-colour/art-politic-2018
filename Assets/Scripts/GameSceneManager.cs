using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager {

    static GameSceneManager Inst;

    private int currectScene = 0;

    public static GameSceneManager GetInstance()
    {
        if (Inst == null)
            Inst = new GameSceneManager();

        return Inst;
    }

    public void GoToNextStage()
    {
        if (currectScene == SceneManager.sceneCountInBuildSettings - 1)
        {
            currectScene = 0;
            LoadCurrentScene();
            return;
        }


        currectScene = SceneManager.GetActiveScene().buildIndex+1;
        LoadCurrentScene();
    }

    public void GoToPrevStage()
    {
        if (currectScene == 0)
            return;
        currectScene = SceneManager.GetActiveScene().buildIndex-1;
        LoadCurrentScene();
    }

    public void ReloadScene()
    {
        LoadCurrentScene();
    }

    private void LoadCurrentScene()
    {

        SceneManager.LoadScene(currectScene);
    }
}
