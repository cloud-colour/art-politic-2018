using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCorrupter {

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
            var publicServants = CorrupterManager.GetCorrupterByID((int)CorrupterType.PublicServant);
            if (publicServants.Count > 0)
            {
                ((PublicServant)publicServants[Random.Range(0, publicServants.Count)]).Die();
            }

            GotCash(col.gameObject);
        }
    }
}
