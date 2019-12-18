using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    static public PoolManager Inst;

    [SerializeField]
    private Cash CashObject;
	[SerializeField]
	private Cash CashPropObject;

    [SerializeField]
    private List<BaseCorrupter> CorrupterPrefabs;

    private List<Cash> CashPools;
    private Dictionary<BaseCorrupter,List<BaseCorrupter>> CorrupterPools;

    void Awake()
    {
        Inst = this;
    }

    void Start () 
    {
        CashPools = new List<Cash>();
        CorrupterPools = new Dictionary<BaseCorrupter,List<BaseCorrupter>>();
    }

	public Cash CreateCash(Vector3 position, bool isProp = false)
    {
        Cash tmp;
                
		tmp = GetCashFromPool(isProp);
        tmp.gameObject.transform.position = position;
        tmp.gameObject.SetActive(true);

        return tmp;
    }

    private Cash GetCashFromPool(bool isProp = false)
    {
        foreach (Cash cash in CashPools)
        {
            if (!cash.gameObject.activeInHierarchy)
            {
                cash.gameObject.transform.eulerAngles = new Vector3(1, 270, 0);
                cash.Rigidbody.velocity = Vector3.zero;
                cash.Rigidbody.angularVelocity = Vector3.zero;
                return cash;
            }                
        }

        Cash newCash = Instantiate(isProp ? CashPropObject : CashObject);
        newCash.gameObject.transform.parent = this.transform;
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
