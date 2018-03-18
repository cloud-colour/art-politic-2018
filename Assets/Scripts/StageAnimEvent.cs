using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnimEvent : MonoBehaviour
{
	[SerializeField] Animation uiAnim;
	[SerializeField] GameObject clickText;

	public void ShowUI()
	{
		uiAnim.gameObject.SetActive(true);
		uiAnim.Play();

        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.GamePlay);
	}

	public void SpawnStartMoney()
	{
		StartCoroutine(SpawnStartMoneyCoroutine());
	}

	IEnumerator SpawnStartMoneyCoroutine()
	{
		Vector3 tmpPos;
		for(int i = 0 ;i <= 100; i++)
		{
			tmpPos = new Vector3( Random.Range(0,Screen.width/8f),Screen.height , 0);
			tmpPos = Camera.main.ScreenToWorldPoint(tmpPos);
			tmpPos.z = 0.5f;
			var tmpCash = PoolManager.Inst.CreateCash(tmpPos,isProb:true);
			var body = tmpCash.GetComponent<Rigidbody>();
			body.sleepThreshold = .5f;
			body.AddForce(0,-20,0,ForceMode.VelocityChange);

			yield return 0;
		}
	}


    public void EndVictory()
    {
//        SoundManager.inst.PlaySFXOneShot(9);
		clickText.SetActive(true);
        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.VictoryWaintInput);
    }

	public void StartVictory()
	{
		GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.VictorySequence);
	}

    public void PlaySFXByID(int index)
    {
        SoundManager.inst.PlaySFXOneShot(index);
    }
}
