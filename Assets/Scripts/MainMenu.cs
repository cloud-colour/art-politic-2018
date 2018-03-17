using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.inst.PlayBGM(0);
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.TitleThrow);
        Invoke("ChangeGameStateToWaitInput",3f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}


    void ChangeGameStateToWaitInput()
    {
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.TitleWaitInput);
    }
}
