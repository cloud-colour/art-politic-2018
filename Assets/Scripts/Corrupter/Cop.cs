using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cop : BaseCorrupter {

	Animation anim;
	bool isHappy;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		Invoke("DoRandomStuff",Random.Range(1f,5f));
	}

	void DoRandomStuff()
	{
		if(isHappy)
			return;

		int rand = Random.Range(1,3);
		if(rand == 0)//flip facing
		{
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}
		else //play wiggle 1 or 2
		{
			anim.Play("Wiggle"+rand);
		}

		Invoke("DoRandomStuff",Random.Range(1f,5f));
	}
	
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

	override protected bool cashCheck(Collision col)
	{
		if(CorrupterManager.GetCorrupterByID((int)CorrupterType.Officer).Count <= 0)
			return col.gameObject.tag == "cash" && col.gameObject.activeInHierarchy;
		else
			return false;
	}

}
