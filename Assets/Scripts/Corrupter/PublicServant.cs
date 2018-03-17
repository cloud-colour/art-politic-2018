using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicServant : BaseCorrupter {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    override protected void OnCollisionEnter (Collision col)
    {
        if(cashCheck(col))
        {
            GotCash(col.gameObject);
        }
    }

    public void Die()
    {
        CorrupterManager.Despawn(this);
    }
}
