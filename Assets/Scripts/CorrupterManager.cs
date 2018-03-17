using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ///Hard Code for now
        corrupters[0] = new List<BaseCorrupter>();
        var polices = gameObject.transform.GetComponentsInChildren<BaseCorrupter>();
        foreach (var police in polices)
        {
            police.CorrupterID = 0;
            police.CorrupterManager = this;
            corrupters[0].Add(police);
        }
        ///Hard Code for now
	}
	
	// Update is called once per frame
	void Update () {

        return;

        currentTime += Time.deltaTime;
        if (currentTime >= cooldownSpawn)
        {
            Spawn(Random.Range(0,2));
            currentTime = 0;
        }

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
