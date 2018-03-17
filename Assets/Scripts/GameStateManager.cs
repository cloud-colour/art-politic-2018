using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                SoundManager.inst.PlayAmbient(0, true);
                SoundManager.inst.PlayAmbient(2, true);
                SoundManager.inst.PlayAmbient(3, false);
    			break;
            case GameState.OpenSequence:
                SoundManager.inst.PlayBGM(1);
                SoundManager.inst.ClearAllAmbient();
    			break;
            case GameState.VictorySequence:
                //Trigger Victory
                SoundManager.inst.PlayBGM(2);
                ChangeState(GameState.VictoryWaintInput);
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