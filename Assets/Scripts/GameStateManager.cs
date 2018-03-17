using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameStateManager {

	public enum GameState
	{
        TitleThrow,
        TitleStopThrow,
        TitleWaitInput,
        TitlePlayStart,
		OpenSequence,
		GamePlay,
        VictorySequence,
        VictoryWaintInput,
	}

    private static GameStateManager inst;
	private GameState state;

    public static GameStateManager GetInstance()
    {
        if (inst == null)
            inst = new GameStateManager();

        return inst;
    }

	void Start ()
	{
        state = GameState.OpenSequence;
	}

#region State
	public void ChangeState(GameState newState)
	{
		state = newState;
		PlayStartState(state);
	}

	void PlayStartState(GameState state)
	{
		switch (state)
		{
            case GameState.GamePlay:
                if (SceneManager.GetActiveScene().buildIndex == 5)
                    SoundManager.inst.PlayAmbient(2, true);
                SoundManager.inst.PlayAmbient(3, false);
    			break;
            case GameState.OpenSequence:
                SoundManager.inst.PlayBGM(1);
                SoundManager.inst.ClearAllAmbient();
                if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4 )
                {
                    SoundManager.inst.PlayAmbient(1, true);
                }
                if (SceneManager.GetActiveScene().buildIndex == 5)
                {
                    SoundManager.inst.PlayAmbient(0, true);
                }
    			break;
            case GameState.VictorySequence:
                //Trigger Victory
                SoundManager.inst.PlayBGM(3);
    			break;

            case GameState.VictoryWaintInput:
                SoundManager.inst.ClearAllAmbient();
                break;
    		default:
    			break;
		}
	}

    public GameState GetGameState()
    {
        return state;
    }

#endregion
}