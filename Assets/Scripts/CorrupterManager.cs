using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CorrupterType
{
    Police,
    Officer,
    PublicServant,
    Soldier
}

public class CorrupterManager : MonoBehaviour 
{

    private Dictionary<int,List<BaseCorrupter>> corrupters;

	// Use this for initialization
	void Start () {
        corrupters = new Dictionary<int, List<BaseCorrupter>>();
        foreach (CorrupterType suit in Enum.GetValues(typeof(CorrupterType)))
        {
            corrupters[(int)suit] = new List<BaseCorrupter>();
        }
        Init();
	}

    void Init()
    {
        var corruptersList = gameObject.transform.GetComponentsInChildren<BaseCorrupter>();
        foreach (var corrupter in corruptersList)
        {
            if (corrupter is Cop)
            {
                corrupter.CorrupterID = (int)CorrupterType.Police;
                corrupters[(int)CorrupterType.Police].Add(corrupter);
            }
            if(corrupter is Officer)
            {
                corrupter.CorrupterID = (int)CorrupterType.Officer;
                corrupters[(int)CorrupterType.Officer].Add(corrupter);
            }
            if(corrupter is PublicServant)
            {
                corrupter.CorrupterID = (int)CorrupterType.PublicServant;
                corrupters[(int)CorrupterType.PublicServant].Add(corrupter);
            }
            if(corrupter is Soldier)
            {
                corrupter.CorrupterID = (int)CorrupterType.Soldier;
                corrupters[(int)CorrupterType.Soldier].Add(corrupter);
            }
            corrupter.CorrupterManager = this;
        }


        if (GetCorrupterByID((int)CorrupterType.Officer).Count > 0)
            SetAllActiveCollider(CorrupterType.Police, false);

    }
	
	void Update () {

	}

    public void Despawn(BaseCorrupter corrupter)
    {
        corrupters[corrupter.CorrupterID].Remove(corrupter);

        if (corrupter.CorrupterID == (int)CorrupterType.Officer)
        {
            if (GetCorrupterByID((int)CorrupterType.Officer).Count <= 0)
                SetAllActiveCollider(CorrupterType.Police, true);
        }
        CheckServivedTheLawsuit();
    }

    public List<BaseCorrupter> GetCorrupterByID(int corrupterId)
    {
        if (corrupters.ContainsKey(corrupterId))
            return corrupters[corrupterId];
        else
            return new List<BaseCorrupter>();
    }

    private void CheckServivedTheLawsuit()
    {
        foreach (var corrupterList in corrupters)
        {
            if (corrupterList.Value.Count > 0)
                return;
        }

        GameStateManager.GetInstance().ChangeState(GameStateManager.GameState.VictorySequence);
    }

    private void SetAllActiveCollider(CorrupterType type, bool enable)
    {
        foreach (var cop in GetCorrupterByID((int)type))
        {
			cop.GetComponent<BoxCollider>().enabled = enable;
        }
    }
}
