﻿using System.Collections;
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
        if(cashCheck(col))
        {
            if (CorrupterManager.GetCorrupterByID((int)CorrupterType.Officer).Count == 0)
            {
                GotCash(col.gameObject);
            }
        }
    }
}
