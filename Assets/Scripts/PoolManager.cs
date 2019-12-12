using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    static public PoolManager Inst;

    [SerializeField]
    private Transform CashObject;
	[SerializeField]
	private Transform CashProbObject;

    [SerializeField]
    private List<BaseCorrupter> CorrupterPrefabs;

    private List<Transform> CashPools;
    private Dictionary<BaseCorrupter,List<BaseCorrupter>> CorrupterPools;

    void Awake()
    {
        Inst = this;
    }

    void Start () 
    {
        CashPools = new List<Transform>();
        CorrupterPools = new Dictionary<BaseCorrupter,List<BaseCorrupter>>();
    }

	public Transform CreateCash(Vector3 position, bool isProb = false)
    {
        Transform tmp;
		tmp = GetCashFromPool(isProb);
        tmp.transform.position = position;
        tmp.gameObject.SetActive(true);
        return tmp;
    }

	private Transform GetCashFromPool(bool isProb = false)
    {
        foreach (var cash in CashPools)
        {
            if (!cash.gameObject.activeInHierarchy)
                return cash; 
        }

        Transform newCash = Instantiate(isProb ? CashProbObject : CashObject).transform;
        newCash.transform.parent = this.transform;
        CashPools.Add(newCash);
        return newCash;
    }

    public BaseCorrupter CreateCorrupter(int id)
    {
        BaseCorrupter tmp;
        tmp = GetCorrupterFromPool(CorrupterPrefabs[id]);
        tmp.transform.position = Vector3.zero;
        tmp.gameObject.SetActive(true);
        return tmp;
    }

    private BaseCorrupter GetCorrupterFromPool(BaseCorrupter corrupterObj)
    {
        if (!CorrupterPools.ContainsKey(corrupterObj))
            CorrupterPools.Add(corrupterObj, new List<BaseCorrupter>());
        
        foreach (var corrupter in CorrupterPools[corrupterObj])
        {
            if (!corrupter.gameObject.activeInHierarchy)
                return corrupter; 
        }

        BaseCorrupter newCorrupter = Instantiate (corrupterObj).GetComponent<BaseCorrupter>() as BaseCorrupter;
        newCorrupter.transform.parent = this.transform;
        CorrupterPools[corrupterObj].Add(newCorrupter);
        return newCorrupter;
    }
}
