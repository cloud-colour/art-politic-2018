using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCorrupter : MonoBehaviour {
    [HideInInspector]
    public CorrupterManager CorrupterManager;
    [HideInInspector]
    public int CorrupterID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    virtual protected void Update () {
//        transform.Translate(Vector3.left * Time.deltaTime);
	}

    virtual protected void OnCollisionEnter (Collision col)
    {
        if(cashCheck(col))
        {
            GotCash(col.gameObject);
        }
    }

    protected bool cashCheck(Collision col)
    {
        return col.gameObject.tag == "cash" && col.gameObject.activeInHierarchy;
    }

    virtual protected void GotCash(GameObject cash)
    {
        CorrupterManager.Despawn(this);
        cash.SetActive(false);
		this.GetComponent<Collider>().enabled = false;
    }

	//called in "Action" animation
	public void DespawnCorrupter()
	{
		CorrupterManager.Despawn(this);
	}
}
