﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Officer : BaseCorrupter {
	
	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

	override protected void OnCollisionEnter (Collision col)
	{
		if(cashCheck(col))
		{
			isHappy = true;

			transform.localScale = Vector3.one;
			anim.Stop();
			anim.Play("Action");
			GotCash(col.gameObject);
		}
	}

    protected override void GotCash(GameObject cash)
    {
        base.GotCash(cash);

        SoundManager.inst.PlaySFXOneShot(16);
    }
}
