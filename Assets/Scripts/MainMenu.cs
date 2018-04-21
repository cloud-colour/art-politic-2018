using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {


    [SerializeField] Animation overlay;
    bool isLoading;
	// Use this for initialization
	void Start () 
    {
        isLoading = false;
        overlay.gameObject.SetActive(true);
        SoundManager.inst.PlayBGM(0);
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.TitleThrow);
        Invoke("ChangeGameStateToWaitInput",6f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isLoading)
            return;
        
        if (GameStateManager.GetInstance().GetGameState() == GameStateManager.GameState.TitlePlayStart)
            StartCoroutine(LoadSceneCoroutine());
	}


    void ChangeGameStateToWaitInput()
    {
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.TitleStopThrow);
        StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(2.5f);
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.TitleWaitInput);
    }

    IEnumerator LoadSceneCoroutine()
    {   
        isLoading = true;
        ShowOverlay();
        yield return new WaitForSeconds(1);
        GameSceneManager.GetInstance().GoToNextStage();
    }

    void ShowOverlay(bool show = true)
    {
        if(show)
            overlay.Play("FadeIn");
        else
            overlay.Play("FadeOut");
    }
}
