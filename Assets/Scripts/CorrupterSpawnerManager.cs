using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrupterSpawnerManager : MonoBehaviour 
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
	}
	
	// Update is called once per frame
	void Update () {
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
    }

    public List<BaseCorrupter> GetCorrupterByID(int corrupterId)
    {
        if (corrupters.ContainsKey(corrupterId))
            return corrupters[corrupterId];
        else
            return new List<BaseCorrupter>();
    }
}
