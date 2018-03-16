using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrupterSpawnerManager : MonoBehaviour {

    [SerializeField]
    private Transform spawnPoint;

    private float cooldownSpawn = 3.0f;
    private float currentTime;
	// Use this for initialization
	void Start () {
        currentTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= cooldownSpawn)
        {
            Spawn();
            currentTime = 0;
        }

	}

    void Spawn()
    {
        BaseCorrupter tmpCorrupter = PoolManager.Inst.CreateCorrupter(0);
        tmpCorrupter.transform.position = spawnPoint.position;
    }
}
