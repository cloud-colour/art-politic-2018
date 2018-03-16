using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    static public PoolManager Inst;

    [SerializeField]
    private Transform CashObject;

    private List<Transform> CashPools;

    void Awake()
    {
        Inst = this;
    }

    void Start () 
    {
        CashPools = new List<Transform>();
    }

    public Transform CreateCash(Vector3 position)
    {
        Transform tmp;
        tmp = GetCashFromPool();
        tmp.transform.position = position;
        tmp.gameObject.SetActive(true);
        return tmp;
    }

    private Transform GetCashFromPool()
    {
        foreach (var cash in CashPools)
        {
            if (!cash.gameObject.activeInHierarchy)
                return cash; 
        }

        Transform newCash = Instantiate (CashObject).transform;
        newCash.transform.parent = this.transform;
        CashPools.Add(newCash);
        return newCash;
    }
}
