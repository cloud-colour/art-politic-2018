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
        if(col.gameObject.tag == "cash")
        {
            if (CorrupterManager.GetCorrupterByID(1).Count == 0)
            {
				isHappy = true;
				col.gameObject.SetActive(false);
				this.GetComponent<Collider>().enabled = false;

				transform.localScale = Vector3.one;
				anim.Stop();
				anim.Play("Action");
            }
        }
    }

	//called in "Action" animation
	public void DespawnCorrupter()
	{
		CorrupterManager.Despawn(this);
	}
}
