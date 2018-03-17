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

    [SerializeField]
    private Transform spawnPoint;

    private float cooldownSpawn = 3.0f;
    private float currentTime;

    private Dictionary<int,List<BaseCorrupter>> corrupters;

	// Use this for initialization
	void Start () {
        currentTime = 0;
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
    }
	
	void Update () {

	}

    void Spawn(int corrupterId)
    {
        BaseCorrupter tmpCorrupter = PoolManager.Inst.CreateCorrupter(corrupterId);
        tmpCorrupter.CorrupterManager = this;
        tmpCorrupter.CorrupterID = corrupterId;
        tmpCorrupter.transform.position = spawnPoint.position;

        if (!corrupters.ContainsKey(corrupterId))
            corrupters[corrupterId] = new List<BaseCorrupter>();

        corrupters[corrupterId].Add(tmpCorrupter);
    }

    public void Despawn(BaseCorrupter corrupter)
    {
        corrupter.gameObject.SetActive(false);
        corrupters[corrupter.CorrupterID].Remove(corrupter);
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

        GameSceneManager.GetInstance().GoToNextStage();
    }
}
