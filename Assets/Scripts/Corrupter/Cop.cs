using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BaseCorrupter {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    override protected void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag == "cash")
        {
            if (CorrupterManager.GetCorrupterByID(1).Count == 0)
            {
                CorrupterManager.Despawn(this);
                col.gameObject.SetActive(false);
            }
        }
    }
}
